using System.ComponentModel.DataAnnotations;

namespace StudentTeacherSubject.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public string? Class { get; set; }

        public ICollection<Language> Languages { get; set; } = new List<Language>();

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        public ICollection<Student>? Students { get; set; } = new List<Student>();
    }
}
