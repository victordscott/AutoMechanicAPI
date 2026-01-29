using System;

namespace AutoMechanic.DataAccess.DTO
{
    public class VehicleDTO
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
    }
}
