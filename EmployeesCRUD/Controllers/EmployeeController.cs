    using EmployeesCRUD.Model;
using EmployeesCRUD.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace EmployeesCRUD.Controllers
{
    [Route("api/companies")]
    [ApiController]
   
    public class CompaniesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public CompaniesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost("Insert")]
        [Authorize]
        public async Task<ActionResult> Insert(Employee employee)
        {
            Employee details= await employeeRepository.Insert(employee);
            return Ok(details);
        }

        [HttpGet("GetEmployees")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
           
                IEnumerable<Employee> employees = await employeeRepository.GetEmployees();
                return Ok(employees);
            
        }
        [HttpGet("GetEmployeesById/{empid}")]
        [Authorize]
        public async Task<ActionResult> GetEmployeesById(string empid)
        {
            Employee employee = await employeeRepository.GetEmployeesById(empid);
            if(employee == null) 
            {
                return NotFound("Employee not found");
            }
            return Ok(employee);
        }
        [HttpDelete("DeleteByEmployeeId/{empid}")]
        [Authorize]
        public async Task<ActionResult> DeleteByEmployeeId(string empid)
        {
            int employeeId = await employeeRepository.DeleteByEmployeeId(empid);
            if (employeeId == 0)
            {
                return NotFound("Employee not found");
            }
            return Ok(employeeId);
        }
        [HttpPut("UpdateEmployee")]
        [Authorize]
        public async Task<ActionResult> UpdateEmployee(Employee employee)
        {
            int employeeId = await employeeRepository.UpdateEmployee(employee);
            if (employeeId == 0)
            {
                return NotFound("Employee not found");
            }
            return Ok(employeeId);
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(Login login)
        {
            int count = await employeeRepository.Login(login);
            if (count > 0)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,"Jwt:Issuer"),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                    new Claim("UserName",login.UserName)

                  
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
                var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                       "Jwt:Issuer",
                       "Jwt:Audience",
                       claims,
                       expires: DateTime.Now.AddMinutes(20),
                       signingCredentials: sign);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Invalid Login");
            }
        }

    }
}
