using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthentication.Models
{
    public class MyModal
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "name cannot exceed 100 characters.")]
        public string name { get; set; }
        [Required(ErrorMessage = "Roll number is required.")]
        public int roll {  get; set; }
        [Required(ErrorMessage = "Adress is required.")]
        public string adress { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        public string state { get; set; }
    }
}
