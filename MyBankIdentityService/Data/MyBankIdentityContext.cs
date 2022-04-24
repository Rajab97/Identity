using Microsoft.EntityFrameworkCore;
using MyBankIdentityService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBankIdentityService.Data
{
    public class MyBankIdentityContext : DbContext
    {
        public MyBankIdentityContext(DbContextOptions<MyBankIdentityContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<User> Users { get; set; }
        //public DbSet<GlobalRole> GlobalRoles { get; set; }
        //public DbSet<GlobalRoleToRole> GlobalRoleToRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleModulesToPermission> RoleModulesToPermissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Branch> Branches { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<GlobalRoleToRole>()
            //    .HasKey(x => new { x.GlobalRoleId, x.RoleId });

            //modelBuilder.Entity<RoleToModule>()
            //    .HasKey(x => new { x.ModuleId, x.RoleId });

            //modelBuilder.Entity<RoleModulesToPermission>()
            //    .HasKey(x => new { x.PermissionId, x.RoleToModuleId });

            modelBuilder.Entity<RoleModulesToPermission>()
                .HasOne(i => i.Role)
                .WithMany(c => c.RoleModulesToPermissions)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(b => b.User)
                .WithOne(i => i.RefreshToken)
                .HasForeignKey<RefreshToken>(b => b.UserId);

            modelBuilder.Entity<Role>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .Property(b => b.Name)
                .IsRequired();


            modelBuilder.Entity<Module>()
                .HasIndex(u => u.Name)
                .IsUnique();    

            modelBuilder.Entity<Module>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Module>()
                .HasMany(m => m.Permissions)
                .WithOne(p => p.Module)
                .HasForeignKey(s => s.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserPrincipal)
                .IsUnique();

        }

    }
}
