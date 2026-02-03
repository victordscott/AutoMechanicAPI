using AutoMapper;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories
{
    public class FileUploadRepository(
        IDbContextFactory<AutoMechanicDbContext> dbContextFactory,
        IMapper mapper
    )
    {
        public async Task<FileUploadDTO> InsertFileUploadAsync(FileUploadDTO fileUploadDTO)
        {
            var fileUpload = mapper.Map<FileUpload>(fileUploadDTO);
            if (fileUpload.FileUploadId == Guid.Empty)
                fileUpload.FileUploadId = Guid.NewGuid();

            var now = DateTime.UtcNow;
            fileUpload.DateCreated = now;
            fileUpload.DateUpdated = now;

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.FileUploads.AddAsync(fileUpload);
                await dbContext.SaveChangesAsync();
            }
            return mapper.Map<FileUploadDTO>(fileUpload);
        }




    }
}
