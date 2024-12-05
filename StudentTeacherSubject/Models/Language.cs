using System.ComponentModel.DataAnnotations;

namespace StudentTeacherSubject.Models
{
    public class Language
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public int SubjectId { get; set; } 
        public Subject? Subject { get; set; } 
    }
}
