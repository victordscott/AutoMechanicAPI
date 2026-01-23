using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class VehicleMileage
{
    public Guid VehicleMileageId { get; set; }

    public Guid VehicleId { get; set; }

    public int MileageId { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
