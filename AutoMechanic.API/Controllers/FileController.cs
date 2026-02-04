using AutoMechanic.Api.Helpers;
using AutoMechanic.Auth.Helpers;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Configuration.Options;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.Services.Services.Interfaces;
using Hangfire.PostgreSql.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Web;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController(
        IFileUploadService fileUploadService,
        IHostingEnvironment hostingEnvironment,
        IOptions<MiscOptions> miscOptions,
        ITokenService tokenService,
        ILogger<FileController> logger
    ) : ControllerBase
    {
        private static readonly FormOptions defaultFormOptions = new FormOptions();

        [Authorize]
        [HttpPost]
        public async Task<dynamic> StreamUpload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                throw new Exception($"Expected a multipart request, but got {Request.ContentType}");
            }

            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            var userFolder = userId.ToString().Replace("-", "").ToString();
            var fileDir = Path.Combine(Path.Combine(hostingEnvironment.ContentRootPath, "Upload"), userFolder);

            DirectoryInfo di = new DirectoryInfo(fileDir);
            if (!di.Exists)
            {
                di.Create();
            }

            // Used to accumulate all the form url encoded key value pairs in the
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string uploadType = null;

            var results = new List<dynamic>();

            // https://stackoverflow.com/a/56342020/2030207
            // attempt to fix error: System.IO.InvalidDataException: Multipart body length limit 16384 exceeded
            var contentType = MediaTypeHeaderValue.Parse(Request.ContentType);
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            var reader = new MultipartReader(boundary.Value, HttpContext.Request.Body);

            //var boundary = MultipartRequestHelper.GetBoundary(
            //	MediaTypeHeaderValue.Parse(Request.ContentType),
            //	_defaultFormOptions.MultipartBoundaryLengthLimit);
            //var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            if (reader.BodyLengthLimit != null)
            {
                //this.logger.LogInformation($"StreamUpload - MultipartReader BodyLengthLimit: {reader.BodyLengthLimit}");
            }
            else
            {
                //this.logger.LogInformation($"StreamUpload - MultipartReader BodyLengthLimit was null");
            }
            // reader.BodyLengthLimit is null (in development)
            // setting it to null here may fix production
            //reader.BodyLengthLimit = null;

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        string originalFileName = contentDisposition.FileName.Value;
                        string fileExtension = System.IO.Path.GetExtension(originalFileName).ToLower();
                        FileExtensionHelper.MediaType mediaType = FileExtensionHelper.GetMediaType(fileExtension);
                        dynamic data = null;

                        if (mediaType == FileExtensionHelper.MediaType.Image)
                        {
                            data = await SaveImage(section.Body, userId, originalFileName, fileDir, userFolder, mediaType);
                        }

                        results.Add(data);
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.Value, value);

                            if (key.Equals("fileUpload") || (key.Value.ToLower().Contains("upload")))
                            {
                                uploadType = value;
                            }

                            if (formAccumulator.ValueCount > defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return base.Content(JsonSerializer.Serialize(results), "application/json", Encoding.UTF8);
        }

        [Authorize]
        [HttpGet("{urlPath}")]
        public IActionResult GetFile(string urlPath)
        {
            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);
            var fileName = Path.GetFileName(fileLocation);

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileLocation, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(fileLocation, contentType);
        }

        [HttpGet("{urlPath}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileByToken(string urlPath, string t)
        {
            var token = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(t));
            try
            {
                var principal = tokenService.GetPrincipalFromToken(token, validateLifetime: true);
                var userName = principal.Identity.Name;
                var userId = Guid.Parse(principal.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);
            }
            catch (Exception ex)
            {
                //Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException
                logger.LogError(ex, "GetFile");
                return StatusCode(401);
            }

            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);
            var fileName = Path.GetFileName(fileLocation);

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileLocation, out contentType))
            {
                contentType = "application/octet-stream";
            }

            //return PhysicalFile(fileLocation, contentType, fileName);	// downloads instead of opening in new tab
            var fileStream = System.IO.File.OpenRead(fileLocation);
            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileName);
            return File(fileStream, contentType);
        }

        [HttpGet("{urlPath}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageByToken(string urlPath, string t, int r, int w, int h)
        {
            var token = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(t));
            try
            {
                var principal = tokenService.GetPrincipalFromToken(token, validateLifetime: true);
                var userName = principal.Identity.Name;
                var userId = Guid.Parse(principal.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);
            }
            catch (Exception ex)
            {
                //Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException
                logger.LogError(ex, "GetFile");
                return StatusCode(401);
            }

            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);
            var fileName = Path.GetFileName(fileLocation);

            var imageResizer = new AutoMechanic.Api.Helpers.ImageResizer();
            Dictionary<string, string> resizeParamDict = new Dictionary<string, string>();
            resizeParamDict.Add("w", w.ToString());
            resizeParamDict.Add("h", h.ToString());
            resizeParamDict.Add("mode", "max");
            resizeParamDict.Add("rotation", r.ToString());

            byte[] b = imageResizer.Resize(fileLocation, resizeParamDict);

            //string mimeType = GetMimeType(fileLocation);
            //string dataUri = $"data:{mimeType};base64,{Convert.ToBase64String(b)}";
            //return Ok(dataUri); // Return as plain string

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileLocation, out contentType))
            {
                contentType = "application/octet-stream";
            }

            //return PhysicalFile(fileLocation, contentType, fileName);	// downloads instead of opening in new tab
            var ms = new MemoryStream(b);
            ms.Seek(0, SeekOrigin.Begin);
            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileName);
            return File(ms, contentType);
        }

        [Authorize]
        [HttpGet("{urlPath}")]
        public IActionResult GetImage(string urlPath)
        {
            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);

            var imageResizer = new AutoMechanic.Api.Helpers.ImageResizer();
            Dictionary<string, string> resizeParamDict = new Dictionary<string, string>();
            resizeParamDict.Add("w", "1060");
            resizeParamDict.Add("h", "550");
            resizeParamDict.Add("mode", "max");
            resizeParamDict.Add("rotation", "90");

            byte[] b = imageResizer.Resize(fileLocation, resizeParamDict);
            var base64String = "data:image/png;base64," + Convert.ToBase64String(b);
            var data = new
            {
                base64Url = base64String
            };
            var resultJson = JsonSerializer.Serialize(data);
            return base.Content(resultJson, "application/json", Encoding.UTF8);
        }

        [Authorize]
        [HttpGet("{urlPath}")]
        public IActionResult GetFullSizeImage(string urlPath)
        {
            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);

            var b = System.IO.File.ReadAllBytes(fileLocation);
            var base64String = "data:image/png;base64," + Convert.ToBase64String(b);
            var data = new
            {
                base64Url = base64String
            };
            var resultJson = JsonSerializer.Serialize(data);
            return base.Content(resultJson, "application/json", Encoding.UTF8);
        }

        [Authorize]
        [HttpGet("{urlPath}/{rotation}")]
        public IActionResult GetImageRotate(string urlPath, int rotation)
        {
            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);

            var imageResizer = new AutoMechanic.Api.Helpers.ImageResizer();
            Dictionary<string, string> resizeParamDict = new Dictionary<string, string>();
            resizeParamDict.Add("w", "1060");
            resizeParamDict.Add("h", "550");
            resizeParamDict.Add("mode", "max");
            resizeParamDict.Add("rotation", rotation.ToString());

            byte[] b = imageResizer.Resize(fileLocation, resizeParamDict);
            var base64String = "data:image/png;base64," + Convert.ToBase64String(b);
            var data = new
            {
                base64Url = base64String
            };
            var resultJson = JsonSerializer.Serialize(data);
            return base.Content(resultJson, "application/json", Encoding.UTF8);
        }

        [Authorize]
        [HttpGet("{urlPath}/{rotation}")]
        public IActionResult GetFullSizeImageRotate(string urlPath, int rotation)
        {
            string ContentRootPath = hostingEnvironment.ContentRootPath;
            string uploadDir = null;
            if (miscOptions.Value.UseFileShare)
            {
                uploadDir = Path.Combine(miscOptions.Value.FileShareLocation, miscOptions.Value.UploadFolderName);
            }
            else
            {
                uploadDir = Path.Combine(hostingEnvironment.ContentRootPath, "Upload");
            }
            urlPath = HttpUtility.UrlDecode(urlPath);
            if (urlPath.Contains("?"))
            {
                urlPath = urlPath.Split('?').First();
            }
            urlPath = urlPath.Replace("/Upload/", string.Empty).Replace("/", "\\");
            var fileLocation = Path.Combine(uploadDir, urlPath);

            byte[] b = null;
            if (rotation == 0)
            {
                b = System.IO.File.ReadAllBytes(fileLocation);
            }
            else
            {
                var imageResizer = new AutoMechanic.Api.Helpers.ImageResizer();
                Dictionary<string, string> resizeParamDict = new Dictionary<string, string>();
                resizeParamDict.Add("rotation", rotation.ToString());
                b = imageResizer.Resize(fileLocation, resizeParamDict);
            }
            var base64String = "data:image/png;base64," + Convert.ToBase64String(b);
            var data = new
            {
                base64Url = base64String
            };
            var resultJson = JsonSerializer.Serialize(data);
            return base.Content(resultJson, "application/json", Encoding.UTF8);
        }

        private async Task<dynamic> SaveImage(
            Stream stream,
            Guid userId,
            string originalFileName,
            string uploadDir,
            string userFolder,
            FileExtensionHelper.MediaType mediaType)
        {
            string ext = System.IO.Path.GetExtension(originalFileName);
            Guid fileUploadId = Guid.NewGuid();
            string fileName = fileUploadId + ext;
            long fileSize = 0;

            using (FileStream fileStream = System.IO.File.Create(Path.Combine(uploadDir, fileName)))
            {
                await stream.CopyToAsync(fileStream);
                fileSize = fileStream.Length;
            }
            stream.Close();

            if (originalFileName.Length > 100)
            {
                string nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(originalFileName);
                originalFileName = nameWithoutExt.Substring(0, 100 - ext.Length) + ext;
            }

            var fileUploadDTO = new FileUploadDTO
            {
                FileUploadId = fileUploadId,
                UploadedById = userId,
                FileTypeId = 1, //TODO - check enum
                FileName = fileName,
                UrlDomain = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host),
                UrlPath = string.Format("/Upload/{0}/{1}", userFolder, fileName),
                OriginalFileName = originalFileName,
                FileSizeBytes = (int)fileSize, //TODO
                IsPublic = false
            };

            var fileUpload = await fileUploadService.InsertFileUploadAsync(fileUploadDTO);

            //var data = new
            //{
            //    FileId = fileUploadId,
            //    //Url = string.Format("{0}://{1}/Upload/{2}/{3}", Request.RequestUri.Scheme, Request.RequestUri.Authority, customerFolder, fileName),
            //    UrlDomain = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host),
            //    UrlPath = string.Format("/Upload/{0}/{1}", userFolder, fileName),
            //    FileName = fileName,
            //    OriginalFileName = originalFileName,
            //    MediaType = mediaType.ToString(),
            //    MediaTypeId = (int)mediaType,
            //    Description = "",
            //    Uploaded = true,
            //    UploadDateUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
            //    IsDeleted = false
            //};

            return fileUpload;
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }

        private string GetMimeType(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
