using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DAL
{
    public class EnrollmentsDbService : IDbService<Enrollment>
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True";
        private static List<Enrollment> _enrollment;

        static EnrollmentsDbService()
        {
            _enrollment = new List<Enrollment>();
            UpdateInfo();
        }

        public bool Add(Enrollment enrollment)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "INSERT INTO enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES (@idEnrollment, @semester, @idStudy, @startDate)";
                com.Parameters.AddWithValue("idStudy", enrollment.IdStudy);
                com.Parameters.AddWithValue("semester", enrollment.Semeter);
                com.Parameters.AddWithValue("startDate", enrollment.StartDate);
                com.Parameters.AddWithValue("idEnrollment", enrollment.IdEnrollment);


                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    com.Transaction = transaction;
                    if (com.ExecuteNonQuery() > 0)
                    {
                        transaction.Commit();
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

        public bool Delete(string deletedId)
        {
            throw new NotImplementedException();
        }

        public Enrollment Get(string id)
        {
            return _enrollment.Find(enrollment => { return enrollment.IdEnrollment == int.Parse(id); });
        }

        public IEnumerable<Enrollment> GetAll()
        {
            return _enrollment;
        }

        public bool Update(Enrollment enrollment)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "promote";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("study", enrollment.Study);
                com.Parameters.AddWithValue("semester", enrollment.Semeter);

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    com.Transaction = transaction;
                    if (com.ExecuteNonQuery() > 0)
                    {
                        transaction.Commit();
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
            _enrollment.Clear();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from enrollment";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Enrollment
                    {
                        Semeter = int.Parse(dr["Semester"].ToString()),
                        IdStudy = int.Parse(dr["IdStudy"].ToString()),
                        StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                        IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                    };
                    _enrollment.Add(st);
                }
            }
        }
    }
}