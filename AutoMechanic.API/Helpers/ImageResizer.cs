using System.Reflection;
using ImageResizer.AspNetCore.Funcs;
using ImageResizer.AspNetCore.Helpers;
using SkiaSharp;

namespace AutoMechanic.Api.Helpers
{
	public class ImageResizer
	{
		//private readonly ILogger<ImageResizer> _logger;
		//private readonly IMemoryCache _memoryCache;
  //      private readonly IOptions<AppSettings> appSettings;

        public ImageResizer()//ILogger<ImageResizer> logger, IMemoryCache memoryCache, IOptions<AppSettings> appSettings)
        {
	        //_logger = logger;
         //   _memoryCache = memoryCache;
         //   this.appSettings = appSettings;
        }

        public byte[] Resize(string imagePath, Dictionary<string, string> resizeParamDict)
        {
	        var resizeParams = GetResizeParams(resizeParamDict);

            // check file lastwrite
            var lastWriteTimeUtc = File.GetLastWriteTimeUtc(imagePath);
            var imageData = GetImageData(imagePath, resizeParams, lastWriteTimeUtc);

            return imageData.ToArray();
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
            //bool isCached = _memoryCache.TryGetValue<byte[]>(cacheKey, out imageBytes);
            //if (isCached)
            //{
            //    _logger.LogInformation("Serving from cache");
            //    return SKData.CreateCopy(imageBytes);
            //}

            SKEncodedOrigin origin; // this represents the EXIF orientation

            // File.OpenRead threw System.IO.IOException: The process cannot access the file because it is being used by another process
            // trying fix: https://stackoverflow.com/a/12942773/2030207
            //var bitmap = LoadBitmap(File.OpenRead(imagePath), out origin); // always load as 32bit (to overcome issues with indexed color)
            var bitmap = LoadBitmap(File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), out origin); // always load as 32bit (to overcome issues with indexed color)

            if (resizeParams.w == 0)
            {
                resizeParams.w = bitmap.Width;
            }
            if (resizeParams.h == 0)
            {
                resizeParams.h = bitmap.Height;
            }

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

            // resize - vds
            if (resizeParams.w == 0) { resizeParams.w = 1;  }
            if (resizeParams.h == 0) { resizeParams.h = 1; }

            var resizedImageInfo = new SKImageInfo(resizeParams.w, resizeParams.h, SKImageInfo.PlatformColorType, bitmap.AlphaType);
            var resizedBitmap = bitmap.Resize(resizedImageInfo, SKFilterQuality.High);

            // optionally pad
            if (resizeParams.mode == "pad")
                resizedBitmap = Padding.PaddingImage(resizedBitmap, paddedWidth, paddedHeight, resizeParams.format != "png");

            // encode
            var resizedImage = SKImage.FromBitmap(resizedBitmap);
            var encodeFormat = resizeParams.format == "png" ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg;
            imageData = resizedImage.Encode(encodeFormat, resizeParams.quality);

            // cache the result
            //_memoryCache.Set<byte[]>(cacheKey, imageData.ToArray());

            // cleanup
            resizedImage.Dispose();
            bitmap.Dispose();
            resizedBitmap.Dispose();

            return imageData;
        }

        public SKBitmap LoadBitmap(Stream stream, out SKEncodedOrigin origin)
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

        private ResizeParams GetResizeParams(Dictionary<string, string> query)
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
    }
}
