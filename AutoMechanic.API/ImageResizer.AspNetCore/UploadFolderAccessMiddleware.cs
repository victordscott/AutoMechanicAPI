using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Configuration.Options;
using HeyRed.Mime;
using ImageResizer.AspNetCore.Funcs;
using ImageResizer.AspNetCore.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace ImageResizer.AspNetCore
{
    public class UploadFolderAccessMiddleware
    {
	    private readonly RequestDelegate _req;
        private readonly ILogger<ImageResizerMiddleware> _logger;
        private readonly IHostingEnvironment _env;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<MiscOptions> miscOptions;
        private readonly ITokenService _tokenService;

        private static readonly string[] suffixes = new string[] {
            ".png",
            ".jpg",
            ".jpeg"
        };

        public UploadFolderAccessMiddleware(
            RequestDelegate req, 
            IHostingEnvironment env, 
            ILogger<ImageResizerMiddleware> logger, 
            IMemoryCache memoryCache, 
            IOptions<MiscOptions> miscOptions,
            ITokenService tokenService
        )
        {
            _req = req;
            _env = env;
            _logger = logger;
            _memoryCache = memoryCache;
            this.miscOptions = miscOptions;
            _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;


            if (!path.ToString().ToLower().Contains("/upload/"))
            {
                await _req.Invoke(context);
                return;
            }

            var tokenEncoded = GetToken(context.Request.Query);
            if (string.IsNullOrEmpty(tokenEncoded))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.StartAsync();
                return;
            }

            var token = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(tokenEncoded));
            try
            {
                var principal = _tokenService.GetPrincipalFromToken(token, validateLifetime: true);
                var userName = principal.Identity.Name;
                var userId = Guid.Parse(principal.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFile");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.StartAsync();
                return;
            }

            ResizeParams resizeParams = new ResizeParams { hasParams = false };

            // hand to next middleware if we are not dealing with an image
            var isImage = IsImagePath(path);            
            if (isImage)
            {
                resizeParams = GetResizeParams(path, context.Request.Query);
            }
            
            // if we got this far, resize it
            _logger.LogInformation($"Resizing {path.Value} with params {resizeParams}");

            // get the image location on disk

            string filePath = string.Empty;
            if (path.ToString().ToLower().Contains("/upload/") && (miscOptions.Value.UseFileShare))
            {
	            filePath = Path.Combine(
		            miscOptions.Value.FileShareLocation,
		            path.Value.Replace('/', Path.DirectorySeparatorChar)
			            .TrimStart(Path.DirectorySeparatorChar))
		            .ToLower()
		            .Replace("\\upload\\", $"\\{miscOptions.Value.UploadFolderName}\\");
            }
            else
            {
	            filePath = Path.Combine(
		            _env.ContentRootPath,
		            path.Value.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));
            }

            // check file lastwrite
            var lastWriteTimeUtc = File.GetLastWriteTimeUtc(filePath);
            if (lastWriteTimeUtc.Year == 1601) // file doesn't exist, pass to next middleware
            {
                await _req.Invoke(context);
                return;
            }

            //isImage = false;
            if (isImage)
            {
                var imageData = GetImageData(filePath, resizeParams, lastWriteTimeUtc);

                // write to stream
                context.Response.ContentType = resizeParams.format == "png" ? "image/png" : "image/jpeg";
                context.Response.ContentLength = imageData.Size;
                await context.Response.Body.WriteAsync(imageData.ToArray(), 0, (int)imageData.Size);

                // cleanup
                imageData.Dispose();
            }
            else
            {
                var bytes = System.IO.File.ReadAllBytes(filePath);

                string mimeType = MimeTypesMap.GetMimeType(filePath);
                context.Response.ContentType = mimeType;
                context.Response.ContentLength = bytes.Length;
                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private SKData GetImageData(string imagePath, ResizeParams resizeParams, DateTime lastWriteTimeUtc)
        {
            // check cache and return if cached
            long cacheKey;
            unchecked
            {
                cacheKey = imagePath.GetHashCode() + lastWriteTimeUtc.ToBinary() + resizeParams.ToString().GetHashCode();
            }

            SKData imageData;
            byte[] imageBytes;
            bool isCached = _memoryCache.TryGetValue<byte[]>(cacheKey, out imageBytes);
            if (isCached)
            {
                _logger.LogInformation("Serving from cache");
                return SKData.CreateCopy(imageBytes);
            }

            SKEncodedOrigin origin; // this represents the EXIF orientation

            // File.OpenRead threw System.IO.IOException: The process cannot access the file because it is being used by another process
            // trying fix: https://stackoverflow.com/a/12942773/2030207
            //var bitmap = LoadBitmap(File.OpenRead(imagePath), out origin); // always load as 32bit (to overcome issues with indexed color)
            var bitmap = LoadBitmap(File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), out origin); // always load as 32bit (to overcome issues with indexed color)

            // if autorotate = true, and origin isn't correct for the rotation, rotate it
            if (resizeParams.autorotate && origin != SKEncodedOrigin.TopLeft)
                bitmap = RotateAndFlip.RotateAndFlipImage(bitmap, origin);

            if (resizeParams.rotation != 0)
            {
                SKEncodedOrigin newOrigin = SKEncodedOrigin.Default;
                switch (resizeParams.rotation)
                {
                    case 90:
                        newOrigin = SKEncodedOrigin.RightTop;
                        break;
                    case 180:
                        newOrigin = SKEncodedOrigin.BottomRight;
                        break;
                    case 270:
                        newOrigin = SKEncodedOrigin.LeftBottom;
                        break;
                }
                bitmap = RotateAndFlip.RotateAndFlipImage(bitmap, newOrigin);
            }

            SKImage resizedImage = null;
            SKBitmap resizedBitmap = null;

            var encodeFormat = (string.IsNullOrEmpty(resizeParams.format) ? GetEncodedImageFormat(imagePath) : (resizeParams.format == "png" ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg));

            if (resizeParams.w == 0 && resizeParams.h == 0)
            {
                imageData = bitmap.Encode(encodeFormat, resizeParams.quality);
            }
            else
            {
                // if either w or h is 0, set it based on ratio of original image
                if (resizeParams.h == 0)
                    resizeParams.h = (int)Math.Round(bitmap.Height * (float)resizeParams.w / bitmap.Width);
                else if (resizeParams.w == 0)
                    resizeParams.w = (int)Math.Round(bitmap.Width * (float)resizeParams.h / bitmap.Height);

                // if we need to crop, crop the original before resizing
                if (resizeParams.mode == "crop")
                    bitmap = Crop.CropImage(bitmap, resizeParams);

                // store padded height and width
                var paddedHeight = resizeParams.h;
                var paddedWidth = resizeParams.w;

                // if we need to pad, or max, set the height or width according to ratio
                if (resizeParams.mode == "pad" || resizeParams.mode == "max")
                {
                    var bitmapRatio = (float)bitmap.Width / bitmap.Height;
                    var resizeRatio = (float)resizeParams.w / resizeParams.h;

                    if (bitmapRatio > resizeRatio) // original is more "landscape"
                        resizeParams.h = (int)Math.Round(bitmap.Height * ((float)resizeParams.w / bitmap.Width));
                    else
                        resizeParams.w = (int)Math.Round(bitmap.Width * ((float)resizeParams.h / bitmap.Height));
                }

                var resizedImageInfo = new SKImageInfo(resizeParams.w, resizeParams.h, SKImageInfo.PlatformColorType, bitmap.AlphaType);
                resizedBitmap = bitmap.Resize(resizedImageInfo, SKFilterQuality.High);

                // optionally pad
                if (resizeParams.mode == "pad")
                    resizedBitmap = Padding.PaddingImage(resizedBitmap, paddedWidth, paddedHeight, resizeParams.format != "png");

                // encode
                resizedImage = SKImage.FromBitmap(resizedBitmap);
                imageData = resizedImage.Encode(encodeFormat, resizeParams.quality);
            }

            // cache the result
            _memoryCache.Set<byte[]>(cacheKey, imageData.ToArray());

            // cleanup
            if (resizedImage != null)
                resizedImage.Dispose();
            bitmap.Dispose();
            if (resizedBitmap != null)
                resizedBitmap.Dispose();

            return imageData;
        }

        private SKBitmap LoadBitmap(Stream stream, out SKEncodedOrigin origin)
        {
            using (var s = new SKManagedStream(stream))
            {
                using (var codec = SKCodec.Create(s))
                {
                    origin = codec.EncodedOrigin;
                    var info = codec.Info;
                    var bitmap = new SKBitmap(info.Width, info.Height, SKImageInfo.PlatformColorType, info.IsOpaque ? SKAlphaType.Opaque : SKAlphaType.Premul);

                    IntPtr length;
                    var result = codec.GetPixels(bitmap.Info, bitmap.GetPixels(out length));
                    if (result == SKCodecResult.Success || result == SKCodecResult.IncompleteInput)
                    {
                        return bitmap;
                    }
                    else
                    {
                        throw new ArgumentException("Unable to load bitmap from provided data");
                    }
                }
            }
        }

        private bool IsImagePath(PathString path)
        {
            if (path == null || !path.HasValue)
                return false;

            return suffixes.Any(x => x.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private string GetToken(IQueryCollection query)
        {
            if (query.ContainsKey("t"))
            {
                return query["t"];
            }
            else
            {
                return null;
            }
        }

        private ResizeParams GetResizeParams(PathString path, IQueryCollection query)
        {
            ResizeParams resizeParams = new ResizeParams();

            // before we extract, do a quick check for resize params
            resizeParams.hasParams =
                resizeParams.GetType().GetTypeInfo()
                .GetFields().Where(f => f.Name != "hasParams")
                .Any(f => query.ContainsKey(f.Name));

            // if no params present, bug out
            if (!resizeParams.hasParams)
                return resizeParams;

            // extract resize params

            if (query.ContainsKey("format"))
                resizeParams.format = query["format"];
            else
                resizeParams.format = path.Value.Substring(path.Value.LastIndexOf('.') + 1);

            if (query.ContainsKey("autorotate"))
                bool.TryParse(query["autorotate"], out resizeParams.autorotate);

            int quality = 100;
            if (query.ContainsKey("quality"))
                int.TryParse(query["quality"], out quality);
            resizeParams.quality = quality;

            int w = 0;
            if (query.ContainsKey("w"))
                int.TryParse(query["w"], out w);
            resizeParams.w = w;

            int h = 0;
            if (query.ContainsKey("h"))
                int.TryParse(query["h"], out h);
            resizeParams.h = h;

            resizeParams.mode = "max";
            // only apply mode if it's a valid mode and both w and h are specified
            if (h != 0 && w != 0 && query.ContainsKey("mode") && ResizeParams.modes.Any(m => query["mode"] == m))
                resizeParams.mode = query["mode"];

            int rotation = 0;
            if (query.ContainsKey("rotation"))
                int.TryParse(query["rotation"], out rotation);
            resizeParams.rotation = rotation;

            return resizeParams;
        }

        private SKEncodedImageFormat GetEncodedImageFormat(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower().Equals(".png"))
            {
                return SKEncodedImageFormat.Png;
            }
            else
            {
                return SKEncodedImageFormat.Jpeg;
            }
        }
    }
}
