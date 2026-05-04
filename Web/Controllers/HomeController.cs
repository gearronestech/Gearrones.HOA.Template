using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}