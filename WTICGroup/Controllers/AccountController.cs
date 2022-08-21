using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WTICGroup.ViewModels;

namespace WTICGroup.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userM;
    private readonly SignInManager<IdentityUser> _signInM;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userM = userManager;
        _signInM = signInManager;
        _logger = logger;
    }

    public  IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel() { ReturnUrl  = returnUrl });
    }

    public  IActionResult Register(string returnUrl)
    {
        return View(new RegisterViewModel() { ReturnUrl  = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var user = new IdentityUser(model.Username);
        var result = await _userM.CreateAsync(user, model.Password);
        var roleResult = await _userM.AddToRoleAsync(user, "user");

        if(!result.Succeeded)
        {
            return BadRequest();
        }
        return LocalRedirect($"/account/login?returnUrl={model.ReturnUrl}");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {

        var user = await _userM.FindByNameAsync(model.Username);
        if(user is null)
        {return BadRequest();}
        
        var result = await _signInM.PasswordSignInAsync(user, model.Password, false, false);

        return LocalRedirect($"{model.ReturnUrl ?? "/"}");
    }
}
