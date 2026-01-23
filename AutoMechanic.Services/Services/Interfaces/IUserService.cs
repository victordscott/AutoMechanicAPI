using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetail?> GetUserByUserNameAsync(string userName);
        Task<UserDetail?> GetUserByIdAsync(Guid userId);
        Task SaveUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime);
        Task<bool> UserRefreshTokenExists(Guid userId, string refreshToken);
        Task<bool> DeleteUserRefreshToken(Guid userId, string refreshToken);
        Task<List<UserDetail>> GetConsultantsAsync();
        Task<List<TimeZoneDTO>> GetTimeZonesAsync();
    }
}
