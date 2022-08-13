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
        if (ModelState.IsValid)
        {
            returnUrl ??= Url.Content("~/");
            var isLoggedIn = await AuthenticationService.AuthenticateAsync(loginVM.Email, loginVM.Password);
            if (isLoggedIn)
                return LocalRedirect(returnUrl);
        }
        ModelState.AddModelError("", "Log In Attempt Failed. Please try again.");
        return View(loginVM);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (ModelState.IsValid)
        {
            var returnUrl = Url.Content("~/");
            var isCreated = await AuthenticationService.RegisterAsync(registerVM);
            if (isCreated)
                return LocalRedirect(returnUrl);
        }
        ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
        return View(registerVM);
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        await AuthenticationService.Logout();
        return LocalRedirect(returnUrl);
    }
}
