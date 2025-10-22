using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Linq;

namespace MVC.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View(new TheModel());
    
    // ChatGPT
    [HttpPost]
    public IActionResult Index(TheModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Valid = false;
            return View(model);
        }

        ViewBag.Valid = true;

        var charsNoSpaces = model.Phrase!
            .Where(c => !char.IsWhiteSpace(c))
            .ToList();

        model.Counts = charsNoSpaces
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        model.Lower = new string(charsNoSpaces.Select(char.ToLower).ToArray());
        model.Upper = new string(charsNoSpaces.Select(char.ToUpper).ToArray());

        return View(model);
    }
}
