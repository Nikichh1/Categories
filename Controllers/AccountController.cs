using Microsoft.AspNetCore.Mvc;

namespace Categories.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}
