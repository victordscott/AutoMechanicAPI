using AutoMapper;
using AutoMechanic.Configuration.Options;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class UserService(IUserRepository userRepository, IOptions<MiscOptions> miscOptions, IMapper mapper) : IUserService
    {
        public async Task<UserDetail?> GetUserByIdAsync(Guid userId)
        {
            return await userRepository.GetUserByIdAsync(userId);
        }

        public async Task<UserDetail?> GetUserByUserNameAsync(string userName)
        {
            return await userRepository.GetUserByUserNameAsync(userName);
        }

        public async Task<UserDetail?> GetUserByUserEmailAsync(string emailAddress)
        {
            return await userRepository.GetUserByUserEmailAsync(emailAddress);
        }

        public async Task<bool> InsertUserLoginOTPCodeAsync(Guid userId, string otpCode)
        {
            return await userRepository.InsertUserLoginOTPCodeAsync(userId, otpCode, miscOptions.Value.OTPCodeExpireMinutes);
        }

        public async Task<bool> VerifyUserLoginOTPCodeAsync(Guid userId, string otpCode)
        {
            return await userRepository.VerifyUserLoginOTPCodeAsync(userId, otpCode);
        }

        public async Task SaveUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime)
        {
            await userRepository.SaveUserRefreshTokenAsync(userId, refreshToken, expiryTime);
        }

        public async Task<bool> UserRefreshTokenExists(Guid userId, string refreshToken)
        {
            return await userRepository.UserRefreshTokenExists(userId, refreshToken);
        }

        public async Task<bool> DeleteUserRefreshToken(Guid userId, string refreshToken)
        {
            return await userRepository.DeleteUserRefreshToken(userId, refreshToken);
        }

        public async Task<List<UserDetail>> GetConsultantsAsync()
        {
            return await userRepository.GetConsultantsAsync();
        }

        public async Task<List<TimeZoneDTO>> GetTimeZonesAsync()
        {
            var timeZones = await userRepository.GetTimeZonesAsync();
            return mapper.Map<List<TimeZoneDTO>>(timeZones);
        }
    }
}
