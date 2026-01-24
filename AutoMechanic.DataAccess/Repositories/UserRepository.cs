using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories
{
    public class UserRepository(IDbContextFactory<AutoMechanicDbContext> dbContextFactory) : IUserRepository
    {
        public async Task<UserDetail?> GetUserByIdAsync(Guid userId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.UserDetails.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            }
        }

        public async Task<UserDetail?> GetUserByUserNameAsync(string userName)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.UserDetails.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            }
        }

        public async Task<UserDetail?> GetUserByUserEmailAsync(string emailAddress)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.UserDetails.Where(u => u.NormalizedEmail == emailAddress.ToUpper()).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> InsertUserLoginOTPCodeAsync(Guid userId, string otpCode, int optCodeExpireMinutes)
        {
            var createDate = DateTime.UtcNow;
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.UserLoginOtpCodes.AddAsync(
                    new UserLoginOtpCode
                    {
                        UserId = userId,
                        OtpCode = otpCode,
                        OtpCodeCreateDate = createDate,
                        OtpCodeExpireDate = createDate.AddMinutes(optCodeExpireMinutes),
                        OtpCodeUsed = false
                    }
                );
            }
            return true;
        }

        public async Task<bool> VerifyUserLoginOTPCodeAsync(Guid userId, string otpCode)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var userLoginOtpCode = await dbContext.UserLoginOtpCodes.Where(u => u.UserId == userId 
                    && u.OtpCode == otpCode 
                    && u.OtpCodeUsed == false
                    && u.OtpCodeExpireDate < DateTime.UtcNow
                    ).FirstOrDefaultAsync();

                if (userLoginOtpCode is not null)
                {
                    userLoginOtpCode.OtpCodeUsed = true;
                    dbContext.UserLoginOtpCodes.Update(userLoginOtpCode);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task SaveUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime)
        {
            //var userLogins = await dbContext.UserLogins.ToListAsync();
            //System.Diagnostics.Debug.WriteLine($"{userLogins[0].LoginDate} - {userLogins[0].LoginDate.Kind}");

            var loginDate = DateTime.UtcNow;
            var loginDateOffset = DateTimeOffset.UtcNow;
            System.Diagnostics.Debug.WriteLine($"Kind: {loginDate.Kind}");

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var userLogin = await dbContext.UserLogins.AddAsync(new UserLogin
                {
                    UserId = userId,
                    LoginDate = loginDate,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = expiryTime
                });
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UserRefreshTokenExists(Guid userId, string refreshToken)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var userLogin = await dbContext.UserLogins.Where(u => u.UserId == userId
                    && u.RefreshToken == refreshToken
                    && u.RefreshTokenExpiryTime > DateTime.UtcNow).FirstOrDefaultAsync();
                return (userLogin is not null);
            }
        }

        public async Task<bool> DeleteUserRefreshToken(Guid userId, string refreshToken)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var userLogin = await dbContext.UserLogins.Where(u => u.UserId == userId
                    && u.RefreshToken == refreshToken).FirstOrDefaultAsync();
                if (userLogin is not null)
                {
                    dbContext.UserLogins.Remove(userLogin);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<List<UserDetail>> GetConsultantsAsync()
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                // TODO: create constants for user types or consider using RoleId
                return await dbContext.UserDetails.Where(u => u.RoleName == "Consultant").ToListAsync();
            }
        }

        public async Task<List<EF.Models.TimeZone>> GetTimeZonesAsync()
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
               return await dbContext.TimeZones.ToListAsync();
            }
        }
    }
}
