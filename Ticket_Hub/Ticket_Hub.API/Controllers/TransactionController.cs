using Microsoft.AspNetCore.Mvc;

namespace Ticket_Hub.API.Controllers;

public class TransactionController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}