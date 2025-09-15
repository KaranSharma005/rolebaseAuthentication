using AspNetCoreHero.ToastNotification.Abstractions;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using RoleBasedAuthentication.Data;
using RoleBasedAuthentication.Models;
using RoleBasedAuthentication.Models.Enums;
using RoleBasedAuthentication.Services;
using System.Buffers;
using System.Drawing.Printing;

namespace RoleBasedAuthentication.RepoHelpers
{
    public class UsersHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _connectionString;
        private readonly INotyfService _notyf;

        public UsersHelper(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            INotyfService notyf
        )
        {
            _context = context;
            _userManager = userManager;
            _connectionString = config.GetConnectionString("DefaultConnection");
            _notyf = notyf;
        }

        public List<TeacherDetailModel> GetTeachers(TeacherFilterModal modal)
        {
            //return GetUsersInPages(modal);
            using (var connection = new SqlConnection(_connectionString))
            {
                var users = connection.Query<TeacherDetailModel>(
                    @"SELECT 
    a.Id, 
    a.UserName, 
    a.Name, 
    a.status, 
    f.SubjectId, 
    e.className
FROM AspNetUsers a
INNER JOIN AspNetUserRoles b ON a.Id = b.UserId
INNER JOIN AspNetRoles c ON b.RoleId = c.Id
LEFT JOIN subjects f ON f.Id = a.Id
LEFT JOIN teacherClassMap d ON d.userId = a.Id
LEFT JOIN classnameMap e ON e.classId = d.classId
WHERE 
    b.RoleId = 2
      and (
                        (a.UserName like '%' + @email + '%' or @email is null)
                        and (a.Name like '%' + @name + '%' or @name is null)
                        and (@classname is null or e.classId = @classname)
                        and (@subjectname is null or f.SubjectId = @subjectname)
                    )",
                    new { email = modal.emailfilter, name = modal.namefilter, classname = modal.classname, subjectname = modal.subjectname }
                    ).ToList();
                return users;
            }
        }

        public async Task<List<string>> GetStudents()
        {
            var users = _context.Users.ToList();
            List<string> students = new List<string>();
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                role = role.ToList();
                foreach (var r in role)
                {
                    if (r == "Student")
                    {
                        students.Add(user.Name);
                    }
                }
            }
            return students;
        }

        public void Toggleteacher(string id, bool status)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    connection.Execute("DELETE from subjects where Id = @id", new { id });
                    connection.Execute("DELETE from teacherclassmap where userid = @id", new { id });
                    connection.Execute("UPDATE AspNetUsers set status = @status where Id = @id", new { id, status });
                }
                _notyf.Success("Successfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<SelectListItem> GetAllSubjects(int? subjectVal)
        {
            return Enum.GetValues(typeof(Subjects))
                                       .Cast<Subjects>()
                                       .Select(e => new SelectListItem
                                       {
                                           Value = ((int)e).ToString(),
                                           Text = e.GetEnumDisplayName(),
                                           Selected = subjectVal.HasValue && (int)e == subjectVal.Value
                                       })
                                       .ToList();
        }

        public List<SelectListItem> GetSubjects(int? subjectVal)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var assignedSubjects = connection.Query<int>("select SubjectId from subjects");

                    var enumList = GetAllSubjects(subjectVal);
                    foreach (int subjectId in assignedSubjects)
                    {
                        enumList = enumList.Where(e => e.Value != subjectId.ToString() || e.Value == subjectVal.ToString()).ToList();
                    }
                    return enumList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<SelectListItem> GetCourses()
        {
            try
            {
                return Enum.GetValues(typeof(Courses))
                    .Cast<Courses>()
                    .Select(e => new SelectListItem
                    {
                        Value = e.ToString(),
                        Text = e.GetEnumDisplayName()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<int> PerformOperationsOnDb(SqlConnection connection, TeacherSubjectModal modal)
        {
            try
            {
                connection.Execute("update aspnetusers set Name = @name where id = @id", new { name = modal.Name, id = modal.Id });

                connection.Execute("delete from subjects where Id = @id", new { id = modal.Id });
                var assignedSubjects = connection.Query<int>("select SubjectId from subjects").ToList();

                connection.Execute("delete from teacherclassmap where userId = @id", new { modal.Id });
                connection.Execute("insert into teacherclassmap(userid, classid) values(@userid, @classid)", new { userid = modal.Id, classid = modal.classId });
                return assignedSubjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdateDetails(TeacherSubjectModal modal)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var assignedSubjects = PerformOperationsOnDb(connection, modal);

                    if (assignedSubjects.Contains(modal.SubjectId) || !IsValidSubject(modal.SubjectId))
                    {
                        _notyf.Success("Details changed successfully");
                        return;
                    }

                    connection.Execute("insert into Subjects(SubjectId, Id) values(@subId, @id)", new { subId = modal.SubjectId, id = modal.Id });
                }
                _notyf.Success("Successfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool IsValidSubject(int subId)
        {
            if (subId > 0 && subId <= 6)
                return true;
            return false;
        }

        public TeacherSubjectModal GetSelectedSubject(string id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = @"select aspnetusers.Name, subjects.subjectid, aspnetusers.Id 
                                   from AspNetUsers
                                   left join subjects on AspNetUsers.Id = Subjects.Id 
                                    where aspnetusers.Id = @id";

                    return connection.QuerySingleOrDefault<TeacherSubjectModal>(sql, new { id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public void assignClassToTeacher(string id, int classId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute("insert into teacherclassmap(userid, classid) values(@userid, @classid)", new { userid = id, classid = classId });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public List<TeacherDetailModel> GetUsersInPages(TeacherFilterModal modal)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            string sql = @"SELECT a.Id, a.UserName, a.Name, a.status, f.SubjectId, e.className
        //                            FROM AspNetUsers a 
        //                            INNER JOIN AspNetUserRoles b ON a.Id = b.UserId
        //                            INNER JOIN AspNetRoles c ON b.RoleId = c.Id
        //                            LEFT JOIN subjects f ON f.Id = a.Id
        //                            LEFT JOIN teacherClassMap d ON d.userId = a.Id
        //                            LEFT JOIN classnameMap e ON e.classId = d.classId
        //                            WHERE  b.RoleId = 2 and (
        //                            (a.UserName like '%' + @email + '%' or @email is null)
        //                            and (a.Name like '%' + @name + '%' or @name is null)
        //                            and (@classname is null or e.classId = @classname)
        //                            and (@subjectname is null or f.SubjectId = @subjectname)
        //                            )
        //                            ORDER BY a.Id
        //                            OFFSET (@PageNumber - 1) * @PageSize ROWS
        //                            FETCH NEXT @PageSize ROWS ONLY";

        //            return connection.Query<TeacherDetailModel>(sql, new { email = modal.emailfilter, name = modal.namefilter, classname = modal.classname, subjectname = modal.subjectname, PageNumber = modal.pageNumber, PageSize = modal.pageSize }).ToList() ;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
    }
}
