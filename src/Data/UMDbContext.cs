using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.Data
{
    public class UMDbContext : DbContext, IDbContext
    {
        public UMDbContext(DbContextOptions<UMDbContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(etb =>
            {
                etb.HasKey(k => k.Id);

                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
