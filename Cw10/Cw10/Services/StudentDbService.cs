using Cw10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw10.Services
{
     public interface StudentDbService
    {
         List<Student> GetStudents();
        Student UpdateStudent(Student student);
         Student DeleteStudent(Student student);
         Student InsertStudent(Student student);

    }
}
