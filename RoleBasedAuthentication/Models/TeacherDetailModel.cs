namespace RoleBasedAuthentication.Models
{
    public class TeacherDetailModel
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string UserName { get; set; }
        public int? SubjectId {  get; set; }
        public bool status { get; set; }
        public string className { get; set; }
    }

    public class TeacherDetailModelVM
    {
        public List<TeacherDetailModel> teacher_list { get; set; }
        public int count { get; set; }
    }

    public class TeacherSubjectModal
    {
        public int SubjectId { get; set; }
        public string Id { get; set; }
        public int? classId { get; set; }

        public string Name { get; set; }
    }

    //emailfilter=11&namefilter=tew&subjectId=1&classId=22
    public class TeacherFilterModal
    {
        public string emailfilter { get; set; }
        public string namefilter { get; set; }
        public int? subjectname { get; set; }
        public int? classname { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }   
    }
}
