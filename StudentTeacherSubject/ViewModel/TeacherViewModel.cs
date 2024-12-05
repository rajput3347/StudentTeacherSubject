namespace StudentTeacherSubject.ViewModel
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Sex { get; set; }
        public string? ImagePath { get; set; }

        // For selecting multiple subjects
        public List<int>? SubjectIds { get; set; } = new List<int>();
    }
}
