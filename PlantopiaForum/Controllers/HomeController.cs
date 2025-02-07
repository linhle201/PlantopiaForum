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
         .Include(d => d.Comments) 
         .OrderByDescending(m => m.CreatedAt)
         .ToListAsync();

           

            return View(discussions);
        }

        // Display a discussion by id - ../Home/Dicussion/328
        public async Task<IActionResult> GetDiscussion(int id)
        {

            var discussion = _context.Discussion.Include(d => d.Comments).OrderByDescending(m => m.CreatedAt).FirstOrDefault(d => d.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

       

        // Privacy page - ../Home/Privacy/
        public IActionResult Privacy()
        {
            return View();
        }
    }
}