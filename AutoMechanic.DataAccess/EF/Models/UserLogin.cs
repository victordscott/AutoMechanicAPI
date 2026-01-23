using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class UserLogin
{
    public Guid UserLoginId { get; set; }

    public Guid UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime RefreshTokenExpiryTime { get; set; }

    public DateTime LoginDate { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
