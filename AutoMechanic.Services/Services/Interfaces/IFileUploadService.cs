using AutoMechanic.DataAccess.DTO;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<FileUploadDTO> InsertFileUploadAsync(FileUploadDTO fileUploadDTO);
    }
}
