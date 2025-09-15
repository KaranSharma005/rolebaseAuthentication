using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthentication.Models.Enums
{
    enum Roles
    {
        Director,
        Teacher,
        Student
    }

    enum Subjects
    {
        [Display(Name = "C")]
        C = 1,
        [Display(Name = "C++")]
        Cpp = 2,
        [Display(Name = "C#")]
        Csharp = 3,
        [Display(Name = "Java")]
        Java = 4,
        [Display(Name = "Python")]
        Python = 5,
        [Display(Name = "JavaScript")]
        JavaScript = 6,
    }

    enum Courses
    {
        [Display(Name = "BCA")]
        BCA,
        [Display(Name = "MCA")]
        MCA,
        [Display(Name = "B.Tech")]
        BTECH,
        [Display(Name = "M.Tech")]
        MTECH,
        [Display(Name = "CS")]
        CS,
        [Display(Name = "IT")]
        IT
    }
}
