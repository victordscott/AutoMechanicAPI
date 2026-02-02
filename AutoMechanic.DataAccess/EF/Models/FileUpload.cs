using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class FileUpload
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

    public virtual ICollection<AppointmentFile> AppointmentFiles { get; set; } = new List<AppointmentFile>();

    public virtual ICollection<ConsultantDetail> ConsultantDetailPrimaryImageUploads { get; set; } = new List<ConsultantDetail>();

    public virtual ICollection<ConsultantDetail> ConsultantDetailPrimaryVideoUploads { get; set; } = new List<ConsultantDetail>();

    public virtual FileType FileType { get; set; } = null!;

    public virtual AspNetUser UploadedBy { get; set; } = null!;

    public virtual ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();

    public virtual ICollection<VehicleFile> VehicleFiles { get; set; } = new List<VehicleFile>();
}
