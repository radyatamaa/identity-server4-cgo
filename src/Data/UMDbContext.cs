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
            modelBuilder.Entity<Roles>(etb =>
            {
                etb.HasKey(k => k.Id);

                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UserRoles>(etb =>
            {
                etb.HasKey(k => k.Id);
                etb.HasOne(c => c.Roles).WithMany().HasForeignKey(o => o.RoleId);
                etb.HasOne(c => c.Users).WithMany().HasForeignKey(o => o.UserId);
                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<OTPTemp>(etb =>
            {
                etb.HasKey(k => k.Id);

                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PermissionRecord>(etb =>
            {
                etb.HasKey(k => k.Id);

                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PermissionRoleMapping>(etb =>
            {
                etb.HasKey(k => k.Id);
                etb.HasOne(c => c.Roles).WithMany().HasForeignKey(o => o.RoleId);
                etb.HasOne(c => c.Permission).WithMany().HasForeignKey(o => o.PermissionId);
                // etb.HasMany(o => o.Addresses).WithOne(f => f.Customer).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
