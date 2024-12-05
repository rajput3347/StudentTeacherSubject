using Microsoft.AspNetCore.Mvc.Rendering;
using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.ViewModel
{
    public class ClassGroupViewModel
    {
        public List<Student> Students { get; set; }
        public SelectList Classes { get; set; }
        public string StudentClass { get; set; }
        public string SearchString { get; set; }


    }
}
