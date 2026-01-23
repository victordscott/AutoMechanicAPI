using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class ConsultantDetail
{
    public Guid UserId { get; set; }

    public string Description { get; set; } = null!;

    public Guid? PrimaryImageUploadId { get; set; }

    public Guid? PrimaryVideoUploadId { get; set; }

    public virtual FileUpload? PrimaryImageUpload { get; set; }

    public virtual FileUpload? PrimaryVideoUpload { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
