using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Server.Models;
using SchoolSystem0.Server.Models;
using System.Reflection.Emit;

namespace SchoolSystem0.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for each model
        //Forstudents
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Cost> Costs { get; set; }

        //Teacher
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        //Manager And base manager
        public DbSet<BaseManager> BaseManagers { get; set; }
        public DbSet<Manager> Managers { get; set; }

        //For adding questions And tests and stuff like that
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Owned Types for ContactInformation and SchoolDetails
            builder.Entity<Student>()
                .OwnsOne(s => s.ContactInformation);
            builder.Entity<ApplicationUser>()
              .HasIndex(u => u.FullName)
              .IsUnique();
            builder.Entity<Student>()
                .OwnsOne(s => s.SchoolDetails);

            // Configure relationships for Grades and Costs
            builder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne() 
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Student>()
                .HasMany(s => s.Costs)
                .WithOne()
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Teacher>()
               .HasMany(t => t.Classes)
               .WithOne(); // Assuming one teacher per class

            builder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithOne();

            builder.Entity<BaseManager>()
                .HasMany(bm => bm.Teachers)
                .WithOne(); // Assuming a base manager manages multiple teachers

            builder.Entity<BaseManager>()
                .HasMany(bm => bm.Classes)
                .WithOne(); // Assuming a base manager manages multiple classes

            // Configure relationships for Manager
            builder.Entity<Manager>()
                .HasMany(m => m.BaseManagers)
                .WithOne(); 

            builder.Entity<Manager>()
                .HasMany(m => m.Teachers)
                .WithOne();

            builder.Entity<Manager>()
                .HasMany(m => m.Classes)
                .WithOne(); // Manager can directly manage classes

            //TestManagment
            builder.Entity<Test>()
                .HasMany(t => t.Questions)
                .WithOne(q => q.Test)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
