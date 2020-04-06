using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET: api/Students
        [HttpGet]
        public IActionResult GetStudents()
        {
            
            var list = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName, LastName, BirthDate, Semester, " +
                    "Name from student join " +
                    "Enrollment on Enrollment.IdEnrollment=Student.IdEnrollment " +
                    "join Studies on Studies.IdStudy = Enrollment.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                
                    while (dr.Read())
                    { 
                        var st = new Student();
                        
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        st.Semester = dr["Semester"].ToString();
                        st.Name = dr["Name"].ToString();
                    list.Add(st);
                        
                        
                    }
                
            }
            return Ok(list);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public IActionResult GetWpis(string id)
        {
            
            var list = new List<Wpis>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select Enrollment.IdEnrollment, Semester from Enrollment join Student on Student.IdEnrollment = Enrollment.IdEnrollment where Student.IndexNumber = '" + id+"'";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var wp = new Wpis();

                    wp.IdEnrollment = dr["IdEnrollment"].ToString();

                    wp.Semester = dr["Semester"].ToString();
                   
                    list.Add(wp);


                }

            }
            return Ok(list);
        }

        // POST: api/Students
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
