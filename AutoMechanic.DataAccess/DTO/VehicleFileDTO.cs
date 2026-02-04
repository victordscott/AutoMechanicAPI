using System;

namespace AutoMechanic.DataAccess.DTO
{
    public class VehicleFileDTO
    {
        public Guid VehicleFileId { get; set; }

        public Guid VehicleId { get; set; }

        public Guid FileUploadId { get; set; }

        public string? CustomerNote { get; set; }

        public string? ConsultantNote { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
