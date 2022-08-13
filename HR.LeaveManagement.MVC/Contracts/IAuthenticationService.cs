using HR.LeaveManagement.MVC.Models;

namespace HR.LeaveManagement.MVC.Contracts;
public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(string email, string password);
    Task<bool> RegisterAsync(RegisterVM registerVM);
    Task Logout();
}
