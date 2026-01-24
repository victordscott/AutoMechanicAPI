using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class UserLoginOtpCode
{
    public Guid UserLoginOtpCodeId { get; set; }

    public Guid UserId { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime OtpCodeCreateDate { get; set; }

    public DateTime OtpCodeExpireDate { get; set; }

    public bool OtpCodeUsed { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
