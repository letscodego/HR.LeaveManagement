using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HR.LeaveManagement.Identity.Services;
public class AuthService : IAuthService
{
    public AuthService(UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager)
    {
        UserManager = userManager;
        JwtSettings = jwtSettings.Value;
        SignInManager = signInManager;
    }

    public UserManager<ApplicationUser> UserManager { get; }
    public JwtSettings JwtSettings { get; }
    public SignInManager<ApplicationUser> SignInManager { get; }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await UserManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception($"User with email: {request.Email} not found!");
        }
        var result = await SignInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
        if (!result.Succeeded)
        {
            throw new Exception($"Credentials for {request.Email} are not valid.");
        }

        JwtSecurityToken jwtSecutiryToken = await GenerateToken(user);

        AuthResponse response = new AuthResponse()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecutiryToken),
        };
        return response;
    }

    public Task<RegistrationRequest> Register(RegistrationRequest request)
    {
        throw new NotImplementedException();
    }


    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await UserManager.GetClaimsAsync(user);
        var roles = await UserManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: JwtSettings.Issuer,
            audience: JwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(JwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
            );
        
        return jwtSecurityToken;
    }
}
