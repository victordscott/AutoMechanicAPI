using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class FileType
{
    public short FileTypeId { get; set; }

    public string FileTypeName { get; set; } = null!;

    public virtual ICollection<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
}
