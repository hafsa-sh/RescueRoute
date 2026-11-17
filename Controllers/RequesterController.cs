
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RescueRoute.Data;
using RescueRoute.Models;

namespace RescueRoute.Controllers
{
    [Authorize(Roles = "Requester")]
    public class RequesterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RequesterController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var requests = await _context.EmergencyRequests
                .Where(r => r.RequesterId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string assistanceNeeded,
            string location,
            string urgency,
            string details)
        {
            if (string.IsNullOrWhiteSpace(assistanceNeeded) ||
                string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(details) ||
                !new[] { "Medium", "High", "Critical" }.Contains(urgency))
            {
                TempData["Error"] = "Please provide valid request details.";
                return RedirectToAction(nameof(Index));
            }

            var request = new EmergencyRequest
            {
                RequesterId = _userManager.GetUserId(User)!,
                AssistanceNeeded = assistanceNeeded.Trim(),
                Location = location.Trim(),
                Urgency = urgency,
                Details = details.Trim(),
                Status = "Pending Review"
            };

            _context.EmergencyRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request submitted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
