using AutoMapper;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService authService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<AuthResponse> Login(LoginModel loginModel)
        {
            var authResponse = await authService.LoginAsync(loginModel);
            return authResponse;
        }

        [HttpPost]
        public async Task<AuthResponse> RefreshToken(TokenModel tokenModel)
        {
            var authResponse = await authService.RefreshTokenAsync(tokenModel);
            return authResponse;
        }

        [HttpPost]
        public ApplicationUser CreateConsultant(CreateConsultantRequest createConsultantRequest)
        {
            var user = mapper.Map<ApplicationUser>(createConsultantRequest);
            return user;
        }
    }
}
