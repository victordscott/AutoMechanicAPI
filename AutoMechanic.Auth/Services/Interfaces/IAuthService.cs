using AutoMechanic.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginAsync(LoginModel loginModel);
        public Task<AuthResponse> RefreshTokenAsync(TokenModel tokenModel);
        public Task<ApiResponse> GetOTPCodeForLogin(string emailAddress);
        public Task<AuthResponse> LoginByOTPCode(LoginModel loginModel);
    }
}
