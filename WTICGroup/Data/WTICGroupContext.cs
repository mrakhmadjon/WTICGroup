using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WTICGroup.Entities;

namespace WTICGroup.Data
{
    public class WTICGroupContext : IdentityDbContext
    {
        public WTICGroupContext (DbContextOptions<WTICGroupContext> options)
            : base(options) { }

        public DbSet<WTICGroup.Entities.Product> Product { get; set; } = default!;
    }
}
