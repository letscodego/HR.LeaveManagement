using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.LeaveManagement.MVC.Services;
public class AuthenticationService : BaseHttpService, Contracts.IAuthenticationService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public AuthenticationService(IClient httpclient, ILocalStorageService localStorageService, IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
            : base(httpclient, localStorageService)
    {
        LocalStorageService = localStorageService;
        HttpContextAccessor = httpContextAccessor;
		Mapper = mapper;
		_tokenHandler = new JwtSecurityTokenHandler();
    }

    public ILocalStorageService LocalStorageService { get; }
    public IHttpContextAccessor HttpContextAccessor { get; }
	public IMapper Mapper { get; }

	public async Task<bool> AuthenticateAsync(string email, string password)
    {
        try
        {
            AuthRequest request = new() { Email = email, Password = password };
            var authResponse = await Client.LoginAsync(request);
            if (authResponse == null) return false;

            if (authResponse.Token != string.Empty)
            {
                JwtSecurityToken? token = _tokenHandler.ReadJwtToken(authResponse.Token);
                var claims = ParseClaims(token);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                var login = HttpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                LocalStorageService.SetStorageValue("token", authResponse.Token);

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task Logout()
    {
        LocalStorageService.ClearStorage(new List<string> { "token" });
        await HttpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> RegisterAsync(RegisterVM registerVM)
    {

        var registeration = Mapper.Map<RegistrationRequest>(registerVM);
        var response = await Client.RegisterAsync(registeration);
        if (response == null) return false;
        if (!string.IsNullOrEmpty(response.UserId))
        {
            await AuthenticateAsync(registeration.Email, registeration.Password);
            return true;
        }
        return false;
    }
    private IList<Claim> ParseClaims(JwtSecurityToken token)
    {
        var claims = token.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, token.Subject));
        return claims;
    }
}
