using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DAL;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService<Student> _dbService;
        public StudentsController(IDbService<Student> dbService)
        {
            _dbService = dbService;
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _dbService.GetAll();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(string id)
        {
            var res = _dbService.Get(id);
            if (res == null)
            {
                return NotFound("Student not found");
            }
            return Ok(res);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            if (_dbService.Add(student))
                return Ok(student);
            return BadRequest();
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            if (_dbService.Update(student))
                return Ok("Aktualizacja dokonczona");
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteStudent(Student student)
        {
            if (_dbService.Delete(student.IndexNumber))
                return Ok("Usuwanie ukonczone");
            return BadRequest();
        }
    }
}