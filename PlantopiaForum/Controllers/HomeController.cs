using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantopiaForum.Data;
using PlantopiaForum.Models;

namespace PlantopiaForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlantopiaForumContext _context;

        // Constructor
        public HomeController(PlantopiaForumContext context)
        {
            _context = context;
        }

        // Home page - ../ or ../Home/Index
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion
                 .Include(d => d.ApplicationUser)
                 .Include(d => d.Comments)
                 .OrderByDescending(m => m.CreatedAt)
                 .ToListAsync();

            return View(discussions);
        }

        // Display a discussion by id - ../Home/Dicussion/328
        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussion
                .Include(d => d.ApplicationUser)  
                .Include(d => d.Comments)  
                    .ThenInclude(c => c.ApplicationUser)  
                .OrderByDescending(m => m.CreatedAt)  
                .FirstOrDefaultAsync(d => d.DiscussionId == id);  

            if (discussion == null)
            {
                return NotFound();  // Return 404 if the discussion is not found
            }

            return View(discussion);  // Return the discussion to the view
        }

        // Profile page for a specific user (accessed via /home/{userId})
        public async Task<IActionResult> Profile(string userId)
        {
            // Fetch the user by userId
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            // If the user does not exist, return 404
            if (user == null)
            {
                return NotFound(); // If no user is found, return a 404 page
            }

            // Pass the user to the view
            return View(user);
        }

        // Privacy page - ../Home/Privacy/
        public IActionResult Privacy()
        {
            return View();
        }
    }
}