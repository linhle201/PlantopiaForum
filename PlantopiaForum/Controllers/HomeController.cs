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
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

            return View(discussions);
        }

        // Display a discussion by id - ../Home/Dicussion/328
        public async Task<IActionResult> Discussion(int id)
        {
            // get photo from db by id
            var discussions = await _context.Discussion
            .OrderByDescending(m => m.CreatedAt)  
            .ToListAsync();

            return View(discussions);
        }

        // Privacy page - ../Home/Privacy/
        public IActionResult Privacy()
        {
            return View();
        }
    }
}