using AutoMechanic.Api.Helpers;
using AutoMechanic.Auth.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController(
        IHostingEnvironment hostingEnvironment
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
            var userFolder = "c" + userId.ToString().Replace("-", "").ToString();
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
                            data = await SaveImage(section.Body, originalFileName, fileDir, userFolder, mediaType);
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

        private async Task<dynamic> SaveImage(
            Stream stream,
            string originalFileName,
            string uploadDir,
            string userFolder,
            FileExtensionHelper.MediaType mediaType)
        {
            string ext = System.IO.Path.GetExtension(originalFileName);
            string fileId = Guid.NewGuid().ToString();
            string fileName = fileId + ext;
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

            var data = new
            {
                FileId = fileId,
                //Url = string.Format("{0}://{1}/Upload/{2}/{3}", Request.RequestUri.Scheme, Request.RequestUri.Authority, customerFolder, fileName),
                UrlDomain = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host),
                UrlPath = string.Format("/Upload/{0}/{1}", userFolder, fileName),
                FileName = fileName,
                OriginalFileName = originalFileName,
                MediaType = mediaType.ToString(),
                MediaTypeId = (int)mediaType,
                Description = "",
                Uploaded = true,
                UploadDateUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                IsDeleted = false
            };

            return data;
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
    }
}
