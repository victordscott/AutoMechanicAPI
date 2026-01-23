using AutoMapper;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<UserDetail?> GetUserByIdAsync(Guid userId)
        {
            return await userRepository.GetUserByIdAsync(userId);
        }

        public async Task<UserDetail?> GetUserByUserNameAsync(string userName)
        {
            return await userRepository.GetUserByUserNameAsync(userName);
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
