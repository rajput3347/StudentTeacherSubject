namespace StudentTeacherSubject.ViewModel
{
    public class StudentSubjectsViewModel
    {
        public string? StudentName { get; set; }
        public List<SubjectWithTeacherAndClass>? Subjects { get; set; }

        public class SubjectWithTeacherAndClass
        {
            public string? SubjectName { get; set; }
            public string? TeacherName { get; set; }
            public string? Class { get; set; }
            public List<string>? Teachers { get; internal set; }
        }
    }

    public class SubjectWithTeacherAndClass
    {
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }
        public string? Class { get; set; }
        public List<string?>? Teachers { get; internal set; }
    }
}
