
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RescueRoute.Data;

namespace RescueRoute.Controllers
{
    [Authorize(Roles = "Volunteer")]
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.EmergencyRequests
                .Where(r => r.Status != "Completed")
                .OrderByDescending(r => r.Urgency == "Critical")
                .ThenByDescending(r => r.Urgency == "High")
                .ThenByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }
    }
}

