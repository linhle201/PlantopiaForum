using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantopiaForum.Data;
using PlantopiaForum.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlantopiaForum.Controllers
{
    public class CommentsController : Controller
    {
        private readonly PlantopiaForumContext _context;

        public CommentsController(PlantopiaForumContext context)
        {
            _context = context;
        }


        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {

               // Set the create date for the comment
                comment.CreateDate = DateTime.Now;

                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to the discussion details page after creating the comment
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }
            ViewData["DiscussionId"] = new SelectList(_context.Discussion, "DiscussionId", "DiscussionId", comment.DiscussionId);
            return View(comment);
        }

    }
}
