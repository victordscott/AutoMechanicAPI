using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class UserFile
{
    public Guid UserFileId { get; set; }

    public Guid UserId { get; set; }

    public Guid FileUploadId { get; set; }

    public string? CustomerNote { get; set; }

    public string? ConsultantNote { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual FileUpload FileUpload { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
