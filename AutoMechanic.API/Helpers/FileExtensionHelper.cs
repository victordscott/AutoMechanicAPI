namespace AutoMechanic.Api.Helpers
{
    public class FileExtensionHelper
    {
        public static List<string> AudioFileExtensions = new List<string>()
        {
            ".aac",
            ".amr",
            ".flac",
            ".mp3",
            ".m4a",
            ".oga",
            ".ogg", // recommended to be audio, but a chance it could be video (opinion based on Wikipedia entry)
			".wma",
            ".wav"
        };
        public static List<string> VideoFileExtensions = new List<string>()
        {
            ".avi",
            ".asf",
            ".mp4",
            ".mov",
            ".wmv",
            ".ogv"
        };
        public static List<string> DualMediaFileExtensions = new List<string>()
        {
            ".3gp",
            ".3gpp",
            ".3g2"
        };
        public static List<string> ImageFileExtensions = new List<string>()
        {
            ".gif",
            ".jpg",
            ".jpeg",
            ".png"
        };
        public static List<string> ZipFileExtensions = new List<string>()
        {
            ".zip"
        };
        public static List<string> CsvFileExtensions = new List<string>()
        {
            ".csv"
        };

        //[JsonConverter(typeof(StringEnumConverter))]
        public enum MediaType
        {
            Image = 1,
            Audio = 2,
            Video = 3,
            DualMedia = 4,
            Other = 5,
            ZipFile = 6,
            CSV = 7
        }

        public static MediaType GetMediaType(string fileExtension)
        {
            if (ImageFileExtensions.Contains(fileExtension))
            {
                return MediaType.Image;
            }
            else if (AudioFileExtensions.Contains(fileExtension))
            {
                return MediaType.Audio;
            }
            else if (VideoFileExtensions.Contains(fileExtension))
            {
                return MediaType.Video;
            }
            else if (DualMediaFileExtensions.Contains(fileExtension))
            {
                return MediaType.DualMedia;
            }
            else if (ZipFileExtensions.Contains(fileExtension))
            {
                return MediaType.ZipFile;
            }
            else
            {
                return MediaType.Other;
            }
        }
    }
}
