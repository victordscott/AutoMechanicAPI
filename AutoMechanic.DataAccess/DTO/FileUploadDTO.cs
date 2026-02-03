using System;

namespace AutoMechanic.DataAccess.DTO
{
    public class FileUploadDTO
    {
        public Guid FileUploadId { get; set; }

        public Guid UploadedById { get; set; }

        public short FileTypeId { get; set; }

        public string FileName { get; set; } = null!;

        public string UrlDomain { get; set; } = null!;

        public string UrlPath { get; set; } = null!;

        public string OriginalFileName { get; set; } = null!;

        public string? FileNote { get; set; }

        public int FileSizeBytes { get; set; }

        public int? VideoLengthSec { get; set; }

        public bool IsPublic { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
