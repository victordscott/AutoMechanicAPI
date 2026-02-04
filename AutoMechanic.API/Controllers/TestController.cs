using AutoMechanic.Configuration.Options;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IDbContextFactory<AutoMechanicDbContext> dbContextFactory;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IProcCaller procCaller;
        private readonly ILogger<TestController> logger;
        private readonly IOptions<JWTOptions> jwtOptions;
        private readonly IUserService userService;

        public TestController(
            IDbContextFactory<AutoMechanicDbContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IProcCaller procCaller,
            ILogger<TestController> logger,
            IOptions<JWTOptions> jwtOptions,
            IUserService userService
        ) {
            this.dbContextFactory = dbContextFactory;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
            this.procCaller = procCaller;
            this.logger = logger;
            this.jwtOptions = jwtOptions;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<dynamic> TestLogin()
        {
            var password = "Vst!1971@";
            var user = await userManager.FindByNameAsync("admin1");
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.UTF8.GetBytes(jwtOptions.Value.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //    new Claim(ClaimTypes.Name, "vscott")
            //}),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    Issuer = jwtOptions.Value.ValidIssuer,
            //    Audience = jwtOptions.Value.ValidAudience,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.ValidIssuer,
                audience: jwtOptions.Value.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        [HttpGet]
        public async Task<dynamic> TestDB()
        {
            return await userService.GetUserByUserNameAsync("admin1");
        }

        [Authorize]
        [HttpGet]
        public dynamic TestProc()
        {
            var data = procCaller.CallProc<List<dynamic>>("get_data_test");

            return data;
        }

        [HttpGet]
        public async Task<ContentResult> TestGetVehiclesWithFiles()
        {
            var jsonString = await procCaller.CallProc<string>("get_vehicle_with_files", Guid.Parse("3e345111-a8ab-4d8e-b339-76fd09e124d7"));

            return Content(jsonString, "application/json");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public dynamic TestAdmin()
        {
            return new
            {
                message = "worked"
            };
        }

        [Authorize(Roles = "Consultant")]
        [HttpGet]
        public dynamic TestConsultant()
        {
            return new
            {
                message = "worked"
            };
        }

        [HttpGet]
        public dynamic TestLog()
        {
            logger.LogInformation("test info...");

            return Ok();
        }

        [HttpGet]
        public async Task<dynamic> AddUser()
        {
            var password = "Vst!1971@";
            var result = await this.userManager.CreateAsync(new ApplicationUser
            {
                UserName = "vscott",
                NormalizedUserName = "VSCOTT",
                Email = "victordscott@gmail.com",
                NormalizedEmail = "VSCOTT",
                PhoneNumber = "678-472-9866",
                FirstName = "Victor",
                LastName = "Scott",
                State = "Georgia",
                Country = "United States",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TimeZoneAbbrev = "EST"
            }, password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return new
                {
                    Core = result.Errors.First().Code,
                    Error = result.Errors.First().Description
                };
            }
        }

        [HttpGet]
        public async Task<dynamic> SetAllUserPasswords()
        {
            var password = "Vst!1971@";
            IEnumerable<ApplicationUser> users = userManager.Users;
            foreach (var user in users.ToList())
            {
                await userManager.UpdateSecurityStampAsync(user);
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, password);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<dynamic> GetConsultantAvailabilitySchedules()
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var schedules = await dbContext.ConsultantAvailabilitySchedules.ToListAsync();
                return schedules;
            }
        }
    }
}
