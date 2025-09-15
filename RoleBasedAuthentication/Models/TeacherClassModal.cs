using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoleBasedAuthentication.Models
{
    public class TeacherClassModal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string userId { get; set; }

        public int classId { get; set; }

        public string description { get; set; }
    }

    public class ClassNameMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int classId { get; set; }
        public string className {  get; set; }
    }
}
