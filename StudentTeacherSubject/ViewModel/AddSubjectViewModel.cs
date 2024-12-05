using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.ViewModel
{
    public class AddSubjectViewModel
    {
        public int StudentId { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
    public class SelectSubjectViewModel
    {
        public int StudentId { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
