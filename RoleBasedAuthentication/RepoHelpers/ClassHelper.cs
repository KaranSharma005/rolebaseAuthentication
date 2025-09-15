using AspNetCoreHero.ToastNotification.Abstractions;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RoleBasedAuthentication.Models;

namespace RoleBasedAuthentication.RepoHelpers
{
    public class ClassHelper
    {
        private readonly string _connectionString;
        private readonly INotyfService _notyf;
        public ClassHelper(
            IConfiguration config,
            INotyfService notyf
        )
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _notyf = notyf;
        }
        public bool CheckClassName(string name)
        {
            if (name == null || name.Trim() == "")
                return false;
            return true;
        }

        public void addClass(ClassModal mdl)
        {
            try
            {
                if(!CheckClassName(mdl.classname))
                {
                    return;
                }
                using (var connection = new SqlConnection(_connectionString))
                {
                    var allClasses = connection.Query<string>("select classname from classnamemap").ToList().Select(item => item.ToLower());

                    if (!allClasses.Contains(mdl.classname.ToLower()))
                    {
                        connection.Execute("insert into classnamemap(classname, description) values(@cName, @description)", new { cName = mdl.classname, description = mdl?.description });
                        _notyf.Success("Class added successfully");
                    }
                    else
                    {
                        _notyf.Error("Class with this name already exist");
                    }
                }
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
            }
        }

        public List<ClassModal> GetList()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.Query<ClassModal>("select * from classnamemap").ToList();
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Error in getting class list");
                return null;
            }
        }

        public void DeleteClass(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute("delete from classnamemap where classid = @id", new { id });
                }
                _notyf.Success("Deleted Successfully");
            }
            catch (Exception ex)
            {
                _notyf.Error("Can't delete class!");
            }
        }

        public ClassModal GetDetails(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.QuerySingleOrDefault<ClassModal>("select * from classnamemap where classid = @id", new { id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdatedClass(ClassModal mdl)
        {
            try
            {
                if (!CheckClassName(mdl.classname))
                {
                    _notyf.Error("Can't update class to be set to null");
                    return;
                }
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Execute("update classnamemap set  description = @description where classid = @classid", new { description = mdl.description, classid = mdl.classId });
                    var result = connection.QuerySingleOrDefault("select classname from classnamemap where classname = @classname", new { classname = mdl.classname });
                    if (result != null)
                    {
                        _notyf.Error("The class already exists");
                    }
                    else
                    {
                        connection.Execute("update classnamemap set classname = @classname where classid = @classid", new { classname = mdl.classname, classid = mdl.classId });
                        _notyf.Success("Updated Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public List<SelectListItem> GetClassesDropDown(string id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var assignedClasses = connection.Query<string>(@"select c.classname from AspNetUsers a
                                          inner join teacherClassMap b on a.Id = b.userId
                                          inner join classnameMap c on c.classId = b.classId");

                    var allClasses = connection.Query<ClassDropdownModal>("select classid,classname from classnamemap").ToList();

                    string assignedClassToThis = connection.QuerySingleOrDefault<string>(@"select b.className from teacherClassMap a 
                                                                                            inner join classnameMap b on a.classid = b.classid 
                                                                                            where a.userid = @id", new {id});

                    List<SelectListItem> classDropdown = new List<SelectListItem>();

                    foreach (var item in allClasses)
                    {
                        if (assignedClassToThis == item.classname)
                        classDropdown.Add(new SelectListItem(item.classname, item.classId.ToString(), selected: true));
                        if (!assignedClasses.Contains(item.classname))
                        {
                            classDropdown.Add(new SelectListItem(item.classname, item.classId.ToString()));
                        }
                    }
                    return classDropdown;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<SelectListItem> GetAllClasses()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var allClasses = connection.Query<ClassDropdownModal>("select classid,classname from classnamemap").ToList();
                    List<SelectListItem> classDropdown = new List<SelectListItem>();
                    foreach (var item in allClasses)
                    {
                        classDropdown.Add(new SelectListItem(item.classname, item.classId.ToString()));
                    }
                    return classDropdown;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
