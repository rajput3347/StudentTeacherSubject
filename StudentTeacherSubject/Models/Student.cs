using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StudentTeacherSubject.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(5, 20, ErrorMessage = "Age must be between 5 and 20")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Father's name is required")]
        public string? FatherName { get; set; } 

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }  

        public string? Class { get; set; }

        [Required(ErrorMessage = "Roll number is required")]
        public string? RollNumber { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Subject>? Subjects { get; set; } = new List<Subject>();

    }
}
