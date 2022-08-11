using HR.LeaveManagement.Application.Models.Identity;

namespace HR.LeaveManagement.Application.Contracts.Identity;
public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationRequest> Register(RegistrationRequest request);
}
