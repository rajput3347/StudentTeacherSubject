using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace StudentTeacherSubject.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public string? Sex { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Class>? Classes { get; set; } = new List<Class>();
       
    }
}
