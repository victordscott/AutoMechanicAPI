using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class Vehicle
{
    public Guid VehicleId { get; set; }

    public Guid CustomerId { get; set; }

    public string? VehicleVin { get; set; }

    public int VehicleYear { get; set; }

    public string VehicleMake { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public string? VehicleSubmodel { get; set; }

    public int? ExternalMakeId { get; set; }

    public int? ExternalModelId { get; set; }

    public int? ExternalSubmodelId { get; set; }

    public string? VinLookupResult { get; set; }

    public int? CurrentMileage { get; set; }

    public string? CustomerNote { get; set; }

    public string? ConsultantNote { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual AspNetUser Customer { get; set; } = null!;

    public virtual ICollection<VehicleFile> VehicleFiles { get; set; } = new List<VehicleFile>();

    public virtual ICollection<VehicleMileage> VehicleMileages { get; set; } = new List<VehicleMileage>();
}
