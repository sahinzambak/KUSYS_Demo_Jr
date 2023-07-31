using KUSYS_Demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KUSYS_Demo.Data
{
    public class KUSYSDbContext : DbContext
    {
        //efdbContext için dbSetleri koydum
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public KUSYSDbContext(DbContextOptions<KUSYSDbContext> options) : base(options)
        {
            SeedData(); // standart kursları constructardan çağırıyorum
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //efDbContext'de courseId'yi string koyduğum için PK işlemini burada yaptım
            modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
        }

        private void SeedData()
        {
            try
            {
                if (!Courses.Any())
                {
                    var courses = new List<Course>
                {
                    new Course { CourseId = "CSI101", CourseName = "Introduction to Computer Science" },
                    new Course { CourseId = "CSI102", CourseName = "Algorithms" },
                    new Course { CourseId = "MAT101", CourseName = "Calculus" },
                    new Course { CourseId = "PHY101", CourseName = "Physics" }
                };
                    Courses.AddRange(courses);
                    SaveChanges();
                }
            }
            catch (Exception ex) { }
        }
    }
}
