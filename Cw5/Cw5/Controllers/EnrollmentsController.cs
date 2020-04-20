using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DAL;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService<Enrollment> _dbService;
        private readonly IDbService<Study> _studyDbService;
        private readonly IDbService<Student> _studentdbService;
        public EnrollmentsController(IDbService<Enrollment> dbService, IDbService<Study> studyDbService, IDbService<Student> studentdbService)
        {
            _dbService = dbService;
            _studyDbService = studyDbService;
            _studentdbService = studentdbService;
        }
        [HttpPost]
        public IActionResult EnrollStudent([FromBody] StudentToEnroll studentToEnroll)
        {
            if (studentToEnroll.BirthDate == null || studentToEnroll.Enrollment == null || studentToEnroll.FirstName == null || studentToEnroll.IndexNumber == null || studentToEnroll.LastName == null || _studyDbService.Get(studentToEnroll.Enrollment) == null)
            {
                return BadRequest();
            }
            int studyId = _studyDbService.Get(studentToEnroll.Enrollment).IdStudy;
            int enrollmentId = (_dbService.GetAll().Where(enrollment => { return enrollment.Semeter == 1 && enrollment.IdStudy == studyId; }).FirstOrDefault()?.IdEnrollment).GetValueOrDefault(-1);
            if (enrollmentId == -1)
            {
                enrollmentId = _dbService.GetAll().Last().IdEnrollment + 1;
                Enrollment enrollment = new Enrollment
                {
                    IdEnrollment = enrollmentId,
                    IdStudy = studyId,
                    Semeter = 1,
                    StartDate = DateTime.Today
                };
                if (!_dbService.Add(enrollment))
                {
                    return BadRequest();
                }
            }


            Student student = new Student
            {
                FirstName = studentToEnroll.FirstName,
                LastName = studentToEnroll.LastName,
                BirthDate = studentToEnroll.BirthDate,
                IndexNumber = studentToEnroll.IndexNumber,
                IdEnrollment = enrollmentId,
            };
            if (_studentdbService.Add(student))
                return Created("", student);
            return BadRequest();
        }
        [HttpPost("promotions")]
        public IActionResult PromoteStudents([FromBody]JObject data)
        {
            string studies = data["studies"].ToString();
            int semester = data["semester"].ToObject<int>();
            int studyId = _studyDbService.Get(studies).IdStudy;
            int enrollmentId = (_dbService.GetAll().Where(enrollment => { return enrollment.Semeter == semester && enrollment.IdStudy == studyId; }).FirstOrDefault()?.IdEnrollment).GetValueOrDefault(-1);
            if (enrollmentId == -1)
            {
                return NotFound();
            }
            Enrollment enrollmentToUpdate = new Enrollment
            {
                Study = studies,
                Semeter = semester
            };
            if (_dbService.Update(enrollmentToUpdate))
            {
                return Created("", _dbService.GetAll().First(enroll => { return enroll.Semeter == semester + 1 && enroll.IdStudy == studyId; }));
            }
            return BadRequest();
        }
    }
}