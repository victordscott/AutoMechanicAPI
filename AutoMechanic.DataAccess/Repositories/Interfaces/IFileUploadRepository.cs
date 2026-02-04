using AutoMechanic.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<FileUploadDTO> InsertFileUploadAsync(FileUploadDTO fileUploadDTO);
    }
}
