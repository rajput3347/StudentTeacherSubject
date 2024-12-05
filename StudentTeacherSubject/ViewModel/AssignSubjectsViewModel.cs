using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.ViewModel
{
    public class AssignSubjectsViewModel
    {
        public Teacher? Teacher { get; set; }
        public List<Subject>? Subjects { get; set; }
        public List<int>? AssignedSubjectIds { get; set; }
    }
}
