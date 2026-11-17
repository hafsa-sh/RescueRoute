
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RescueRoute.Data;

namespace RescueRoute.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.EmergencyRequests
                .OrderByDescending(r => r.Urgency == "Critical")
                .ThenByDescending(r => r.Urgency == "High")
                .ThenByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id, string status)
        {
            string[] allowedStatuses =
            {
                "Pending Review",
                "Under Review",
                "Volunteer Assigned",
                "Completed"
            };

            if (!allowedStatuses.Contains(status))
            {
                return BadRequest();
            }

            var request = await _context.EmergencyRequests
                .FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = status;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Request status updated successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}

