
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RescueRoute.Data;

namespace RescueRoute.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new AdminUserViewModel
                {
                    Email = user.Email ?? "",
                    Role = string.Join(", ", roles),
                    Status = user.LockoutEnd.HasValue &&
                             user.LockoutEnd.Value > DateTimeOffset.UtcNow
                             ? "Locked"
                             : "Active"
                });
            }

            ViewBag.TotalUsers = users.Count;
            ViewBag.TotalVolunteers = userList.Count(
                u => u.Role.Split(", ").Contains("Volunteer"));

            ViewBag.OpenRequests = await _context.EmergencyRequests
                .CountAsync(r => r.Status != "Completed");

            return View(userList);
        }
    }

    public class AdminUserViewModel
    {
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Status { get; set; } = "";
    }
}

