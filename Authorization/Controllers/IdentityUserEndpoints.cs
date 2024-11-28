using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.Controllers
{
    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {


            app.MapPost("/signup", CreateUser);
            app.MapPost("/signin", SignIn);
            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(UserManager<AppUser> userManager,
                [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
            };
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, userRegistrationModel.Role);

                // Adding claims based on role
                if (userRegistrationModel.Role == "Admin" || userRegistrationModel.Role == "User")
                {
                    await userManager.AddClaimAsync(user, new Claim("CanEditRacunStatus", "true"));
                }

                return Results.Ok(result);
            }
            return Results.BadRequest(result);
        }

        [AllowAnonymous]
        private static async Task<IResult> SignIn(
           UserManager<AppUser> userManager,
           [FromBody] LoginModel loginModel,
           IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                var userClaims = await userManager.GetClaimsAsync(user);

                // Include the "CanEditRacunStatus" claim in the list of claims
                var canEditRacunStatusClaim = userClaims.FirstOrDefault(c => c.Type == "CanEditRacunStatus");
                if (canEditRacunStatusClaim != null)
                {
                    userClaims.Add(canEditRacunStatusClaim);
                }

                // Create the claims identity with the user's claims and role
                ClaimsIdentity claims = new ClaimsIdentity(new[] {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty),
        });

                // Add additional claims from the user
                foreach (var claim in userClaims)
                {
                    claims.AddClaim(claim);
                }

                // Create JWT token
                var signInKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(10),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Results.Ok(new { token });
            }

            return Results.BadRequest(new { message = "Username or password is incorrect." });
        }


    }

}

  



