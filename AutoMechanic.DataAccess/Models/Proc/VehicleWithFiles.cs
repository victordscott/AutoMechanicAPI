using AutoMechanic.DataAccess.DTO;
using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.Models.Proc
{
    public class VehicleWithFiles
    {
        public Guid VehicleId { get; set; }

        public Guid CustomerId { get; set; }

        public string? VehicleVin { get; set; }

        public int VehicleYear { get; set; }

        public string VehicleMake { get; set; } = null!;

        public string VehicleModel { get; set; } = null!;

        public string? VinLookupResult { get; set; }

        public int? CurrentMileage { get; set; }

        public string? CustomerNote { get; set; }

        public string? ConsultantNote { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public List<VehicleFileDetail> CurrentFiles { get; set; } = new();
        public List<FileUploadDTO> NewFiles { get; set; } = new();
        public List<VehicleFileDetail> DeletedFiles { get; set; } = new();
        public List<VehicleFileDetail> ModifiedFiles { get; set; } = new();
    }
}
