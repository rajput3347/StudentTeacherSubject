using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.ViewModel
{
    public class AssignTeachersViewModel
    {
        public Class? Class { get; set; }
        public IList<Teacher>? AvailableTeachers { get; set; }
        public int[]? SelectedTeachers { get; set; }
    }
}
