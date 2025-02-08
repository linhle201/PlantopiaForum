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

        public IActionResult Create(int DiscussionId)
        {
            var discussion = _context.Discussion.FirstOrDefault(d => d.DiscussionId == DiscussionId);

            if (discussion == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                DiscussionId = DiscussionId
            };

            return View(comment);
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

                // Set the CreateDate for the comment
                comment.CreateDate = DateTime.Now;

                // Add the comment to the database
                _context.Add(comment);
                await _context.SaveChangesAsync();

                // Redirect to the discussion details page after creating the comment
                return RedirectToAction("GetDiscussion", "Home", new { id = comment.DiscussionId });
            }

            ViewData["DiscussionId"] = comment.DiscussionId;
            return View(comment);
        }

    }
}