using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class FileUploadService(IFileUploadRepository fileUploadRepository) : IFileUploadService
    {
        public async Task<FileUploadDTO> InsertFileUploadAsync(FileUploadDTO fileUploadDTO)
        {
            return await fileUploadRepository.InsertFileUploadAsync(fileUploadDTO);
        }
    }
}
