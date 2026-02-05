using AutoMechanic.Common.Enums;
using System;

namespace AutoMechanic.DataAccess.Models.Proc
{
    public class EntityFileDetail
    {
        public Guid EntityFileId { get; set; }

        public Guid FileUploadId { get; set; }

        public string FileTypeName { get; set; } = null!;

        public string? CustomerNote { get; set; }

        public string? ConsultantNote { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public int FileTypeId { get; set; }

        public string FileName { get; set; } = null!;

        public string UrlDomain { get; set; } = null!;

        public string UrlPath { get; set; } = null!;

        public string OriginalFileName { get; set; } = null!;

        public string? FileNote { get; set; }

        public long FileSizeBytes { get; set; }

        public bool IsPublic { get; set; }

        public DateTime UploadDateCreated { get; set; }

        public FrontEndState FrontEndState { get; set; } = FrontEndState.NotModified;
    }
}
