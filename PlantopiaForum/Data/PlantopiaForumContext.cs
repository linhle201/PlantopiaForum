using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantopiaForum.Models;

namespace PlantopiaForum.Data
{
    public class PlantopiaForumContext : IdentityDbContext<ApplicationUser>
    {
        public PlantopiaForumContext (DbContextOptions<PlantopiaForumContext> options)
            : base(options)
        {
        }

        public DbSet<PlantopiaForum.Models.Discussion> Discussion { get; set; } = default!;
        public DbSet<PlantopiaForum.Models.Comment> Comment { get; set; } = default!;
    }
}
