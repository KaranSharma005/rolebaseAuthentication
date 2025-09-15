namespace RoleBasedAuthentication.Models
{
    public class ClassModal
    {
        public int? classId { get; set; }
        public string classname { get; set; }
        public string description { get; set; }
    }

    public class ClassDropdownModal
    {
        public int classId { get; set; }
        public string classname { get; set; }
    }
}
