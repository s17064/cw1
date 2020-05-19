using Cw10.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.DAL
{
    public class StudiesDbService : IDbService<Study>
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True";
        private static List<Study> _studies;

        static StudiesDbService()
        {
            _studies = new List<Study>();
            UpdateInfo();
        }


        public bool Add(Study study)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "INSERT INTO study(IdStudy, Name) VALUES (@idStudy, @name)";
                com.Parameters.AddWithValue("idStudy", study.IdStudy);
                com.Parameters.AddWithValue("name", study.Name);


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

        public Study Get(string name)
        {
            return _studies.Find(study => { return study.Name == name; });
        }

        public IEnumerable<Study> GetAll()
        {
            return _studies;
        }

        public bool Update(Study updated)
        {
            throw new NotImplementedException();
        }


        private static void UpdateInfo()
        {
            _studies.Clear();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from studies";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Study
                    {
                        Name = dr["Name"].ToString(),
                        IdStudy = int.Parse(dr["IdStudy"].ToString())
                    };
                    _studies.Add(st);
                }
            }
        }
    }
}
