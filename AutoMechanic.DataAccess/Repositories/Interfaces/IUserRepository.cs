using AutoMechanic.DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDetail?> GetUserByUserNameAsync(string userName);
        Task<UserDetail?> GetUserByIdAsync(Guid userId);
        Task<UserDetail?> GetUserByUserEmailAsync(string emailAddress);
        Task<bool> InsertUserLoginOTPCodeAsync(Guid userId, string otpCode, int optCodeExpireMinutes);
        Task<bool> VerifyUserLoginOTPCodeAsync(Guid userId, string otpCode);
        Task SaveUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime);
        Task<bool> UserRefreshTokenExists(Guid userId, string refreshToken);
        Task<bool> DeleteUserRefreshToken(Guid userId, string refreshToken);
        Task<List<UserDetail>> GetConsultantsAsync();
        Task<List<EF.Models.SupportedTimeZone>> GetTimeZonesAsync();
    }
}
