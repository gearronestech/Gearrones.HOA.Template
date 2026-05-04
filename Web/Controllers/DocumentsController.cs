using GearrOnes.HOA.Template.Core.Attributes;
using GearrOnes.HOA.Template.Core.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GearrOnes.HOA.Template.Web.Controllers;

[HoaFeature("FullManagement", 1, FeatureKey = FeatureKeys.Documents)]
public class DocumentsController : Controller
{
    public IActionResult Index() => View();
}