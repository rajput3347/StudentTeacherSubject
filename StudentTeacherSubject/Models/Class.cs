using System.ComponentModel.DataAnnotations;

namespace StudentTeacherSubject.Models
{
    public class Class
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Class is required")]
        public string? Name { get; set; }

        // Navigation properties
        public ICollection<Teacher>? Teachers { get; set; } = new List<Teacher>(); 
    }
}
