using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cw10.Models;
using Cw10.Services;
using Cw10.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbService _dbService;
        public IConfiguration Configuration { get; set; }

        public StudentsController(StudentDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            Configuration = configuration;
        }


        

        public class AccountController : Controller
        {
            
            private List<Student> students = new List<Student>
        {
            new Student { Index="s15473", Password="dhjdsh", Role = "stud" },
            new Student { Index="s18746", Password="oridks", Role = "stud" }
        };

            [HttpPost("/token")]
            public IActionResult Token(string username, string password)
            {
                var identity = GetIdentity(username, password);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                        issuer: MyToken.ISSUER,
                        audience: MyToken.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(MyToken.LIFETIME)),
                        signingCredentials: new SigningCredentials(MyToken.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };

                return Json(response);
            }

            private ClaimsIdentity GetIdentity(string username, string password)
            {
                Student student = students.FirstOrDefault(x => x.Index == username && x.Password == password);
                if (student != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, student.Index),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, student.Role)
                };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }


                return null;
            }
        }
    }
}