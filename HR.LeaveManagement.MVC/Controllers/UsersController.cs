using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers;
public class UsersController : Controller
{
    public UsersController(IAuthenticationService authenticationService)
    {
        AuthenticationService = authenticationService;
    }

    public IAuthenticationService AuthenticationService { get; }

    public IActionResult Login(string returnUrl = null)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        var isLoggedIn = await AuthenticationService.AuthenticateAsync(loginVM.Email, loginVM.Password);
        if (isLoggedIn)
        {
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Log In Attempt Failed! Please try again.");

        return View(loginVM);
    }
}
