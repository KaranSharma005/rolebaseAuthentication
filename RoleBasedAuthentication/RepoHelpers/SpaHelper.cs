using Dapper;
using Microsoft.Data.SqlClient;
using RoleBasedAuthentication.Models;
using System.Security.Cryptography;

namespace RoleBasedAuthentication.RepoHelpers
{
    public class SpaHelper
    {
        private readonly string _connectionString;

        public SpaHelper(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void Add(MyModal mdl)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "insert into spatable(name, roll, adress, state) values(@name, @roll, @adress, @state)";
                    connection.Execute(query, new { mdl.name, mdl.roll, mdl.adress, mdl.state });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<MyModal> Get()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "select * from spatable";
                    return connection.Query<MyModal>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public void delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM spatable WHERE roll = @id";
                    connection.Execute(sql, new {id = id });
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Update(MyModal mdl)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "update spatable set name = @name, adress = @adress, state = @state where roll = @roll";
                    connection.Execute(sql, new { name = mdl.name, adress = mdl.adress, state = mdl.state, roll = mdl.roll});
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MyModal getStudent(int roll)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.QuerySingleOrDefault<MyModal>("SELECT * FROM spatable WHERE roll = @roll", new { roll = roll });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
