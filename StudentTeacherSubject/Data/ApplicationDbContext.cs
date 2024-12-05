using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Class> Classes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<Teacher>()
             .HasMany(t => t.Subjects)
             .WithMany(s => s.Teachers)
             .UsingEntity(j => j.ToTable("TeacherSubjects"));

            modelBuilder.Entity<Teacher>()
               .HasMany(t => t.Classes)
               .WithMany(c => c.Teachers)
               .UsingEntity<Dictionary<string, object>>(
                   "TeacherClass",
                   j => j.HasOne<Class>().WithMany().HasForeignKey("ClassId"),
                   j => j.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId")
               );

            modelBuilder.Entity<Student>()
                  .HasMany(s => s.Subjects)
                  .WithMany(sub => sub.Students)
                  .UsingEntity<Dictionary<string, object>>(
                       "StudentSubject", // Join table name
                        j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"),
                        j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId")
                  );
        }

    }
}
