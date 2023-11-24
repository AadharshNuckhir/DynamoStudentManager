using Amazon.DynamoDBv2.DataModel;
using DynamoStudentManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoStudentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDynamoDBContext _context;
        public StudentController(IDynamoDBContext context)
        {
            _context = context;
        }
        [HttpGet("getbyid/{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            return Ok(student);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _context.ScanAsync<Student>(default).GetRemainingAsync();
            return Ok(student);
        }
        [HttpPost("add")]
        public async Task<IActionResult> CreateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student != null) return BadRequest($"Student with Id {studentRequest.Id} Already Exists");
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }
        [HttpDelete("delete/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student == null) return NotFound();
            await _context.DeleteAsync(student);
            return NoContent();
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student == null) return NotFound();
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetMessage()
        {
            return Ok("Hello World");
        }
    }
}
