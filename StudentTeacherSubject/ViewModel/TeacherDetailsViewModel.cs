namespace StudentTeacherSubject.ViewModel
{
    public class TeacherDetailsViewModel
    {
        public int TeacherId { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Sex { get; set; }
        public string? ImagePath { get; set; }

        public List<SubjectClassViewModel>? SubjectClasses { get; set; }
    }

    public class SubjectClassViewModel
    {
        public string? SubjectName { get; set; }
        public string? ClassName { get; set; }
    }
}
