using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw5.Models;

namespace Cw5.DAL
{
    public class StudentsDbService : IDbService<Student>
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True";
        private static List<Student> _students;

        static StudentsDbService()
        {
            _students = new List<Student>();
            UpdateInfo();
        }

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public Student Get(string id)
        {
            return _students.Find(stud => { return stud.IndexNumber == id; });
        }


        public bool Add(Student student)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "INSERT INTO Student VALUES(@s, @imie, @nazwisko, @DoB, @idEnrollment)";
                com.Parameters.AddWithValue("s", student.IndexNumber);
                com.Parameters.AddWithValue("imie", student.FirstName);
                com.Parameters.AddWithValue("nazwisko", student.LastName);
                com.Parameters.AddWithValue("DoB", student.BirthDate);
                com.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    com.Transaction = transaction;
                    var res = com.ExecuteNonQuery();
                    transaction.Commit();
                    if (res != 0)
                    {
                        UpdateInfo();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }

        public bool Update(Student student)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "UPDATE Student SET FirstName = @imie, LastName = @nazwisko, BirthDate = @DoB, IdEnrollment = @idEnrollment WHERE IndexNumber = @s";
                com.Parameters.AddWithValue("s", student.IndexNumber);
                com.Parameters.AddWithValue("imie", student.FirstName);
                com.Parameters.AddWithValue("nazwisko", student.LastName);
                com.Parameters.AddWithValue("DoB", student.BirthDate);
                com.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    com.Transaction = transaction;
                    var res = com.ExecuteNonQuery();
                    transaction.Commit();
                    if (res != 0)
                    {
                        UpdateInfo();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }

        public bool Delete(string studentId)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "DELETE FROM Student WHERE IndexNumber = @ska";
                com.Parameters.AddWithValue("ska", studentId);

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    com.Transaction = transaction;
                    var res = com.ExecuteNonQuery();
                    transaction.Commit();
                    if (res != 0)
                    {
                        UpdateInfo();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        public static void UpdateInfo()
        {
            _students.Clear();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student
                    {
                        IndexNumber = dr["IndexNumber"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                    };
                    _students.Add(st);
                }
            }
        }
    }
}