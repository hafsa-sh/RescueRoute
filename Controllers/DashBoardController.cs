using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RescueRoute.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");

            if (User.IsInRole("Coordinator"))
                return RedirectToAction("Index", "Coordinator");

            if (User.IsInRole("Volunteer"))
                return RedirectToAction("Index", "Volunteer");

            if (User.IsInRole("Requester"))
                return RedirectToAction("Index", "Requester");

            return Forbid();
        }
    }
}