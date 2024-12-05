using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.ViewModel
{
    public class AssignClassesViewModel
    {
        public Teacher? Teacher { get; set; }
        public IList<Class>? AvailableClasses { get; set; }
    }
}
