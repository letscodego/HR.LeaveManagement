using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Services;
public class UserService : IUserService
{
    public UserService(UserManager<ApplicationUser> userManager)
    {
        UserManager = userManager;
    }

    public UserManager<ApplicationUser> UserManager { get; }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await UserManager.GetUsersInRoleAsync("Employee");
        return employees.Select(employee => new Employee
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email
        }).ToList();
    }
}
