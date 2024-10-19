using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSystem.Server.Models;
using SchoolSystem0.Server.Data;
using SchoolSystem0.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SchoolSystem0.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // GET: api/Students/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id;
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            var student = await _context.Students
                .Include(s => s.Grades)
                .Include(s => s.Costs)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var studentUser = await _userManager.FindByEmailAsync(student.ContactInformation.Email);

            // Allow access if the user is a Teacher, BaseManager, or Manager
            if (currentUserRoles.Contains("Teacher") || currentUserRoles.Contains("BaseManager") || currentUserRoles.Contains("Manager"))
            {
                return Ok(student);
            }

            // Allow access if the student is accessing their own data
            if (studentUser != null && currentUserId == studentUser.Id)
            {
                return Ok(student);
            }

            return Forbid();
        }

        // GET: api/Students/Myself (Get the current authenticated student's data)
        [HttpGet("Myself")]
        [Authorize]
        public async Task<ActionResult<Student>> GetStudentByToken()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized(new { message = "User is not authenticated or could not be found" });
            }

            var currentUserEmail = currentUser.Email;

            var student = await _context.Students
                .Include(s => s.Grades)
                .Include(s => s.Costs)
                .FirstOrDefaultAsync(s => s.ContactInformation.Email.ToLower() == currentUserEmail.ToLower());

            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            // Allow access if the user is a Teacher, BaseManager, or Manager
            if (currentUserRoles.Contains("Teacher") || currentUserRoles.Contains("BaseManager") || currentUserRoles.Contains("Manager"))
            {
                return Ok(student);
            }

            // Allow access if the student is accessing their own data
            if (currentUserEmail == student.ContactInformation.Email)
            {
                return Ok(student);
            }

            return Forbid();
        }

        // PUT: api/Students/{id} (Update student data)
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, BaseManager, Manager")]
        public async Task<IActionResult> UpdateStudent(int id, Student updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Update logic
            student.FullName = updatedStudent.FullName;
            student.DateOfBirth = updatedStudent.DateOfBirth;
            student.Gender = updatedStudent.Gender;
            student.ContactInformation = updatedStudent.ContactInformation;
            student.SchoolDetails = updatedStudent.SchoolDetails;

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students (Create new student)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher, BaseManager, Manager")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Students/{id}/grade (Get a student's grade)
        [HttpGet("{id}/grade")]
        [Authorize]
        public async Task<ActionResult<double>> GetGrade(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.Grades);
        }

        // GET: api/Students/{id}/cost (Get a student's cost)
        [HttpGet("{id}/cost")]
        [Authorize]
        public async Task<ActionResult<decimal>> GetCost(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.Costs);
        }

        // POST: api/Students/{id}/absences (Add a new absence)
        [HttpPost("{id}/absences")]
        [Authorize(Roles = "Teacher, BaseManager, Manager")]
        public async Task<IActionResult> AddAbsence(int id, [FromBody] DateTime absenceDate)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.Absences.Add(absenceDate);
            await _context.SaveChangesAsync();

            return Ok(student.Absences);
        }

        // GET: api/Students/{id}/absences (Get all absences)
        [HttpGet("{id}/absences")]
        [Authorize]
        public async Task<ActionResult<List<DateTime>>> GetAbsences(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student.Absences);
        }

        // API for interacting with Neshan distance matrix
        [HttpGet("group-near-school")]
        [Authorize]
        public async Task<ActionResult<List<List<Student>>>> GetGroupsNearSchool([FromQuery] double schoolLat, [FromQuery] double schoolLon)
        {
            var students = await _context.Students.Include(s => s.ContactInformation).ToListAsync();
            var distanceService = new DistanceService(_httpClient, _configuration);

            var studentsWithDistance = new List<(Student student, double distance)>();

            // Calculate distance of each student from the school
            foreach (var student in students)
            {
                var distance = await distanceService.GetDistanceAsync(
                    student.ContactInformation.Latitude,
                    student.ContactInformation.Longitude,
                    schoolLat,
                    schoolLon);

                studentsWithDistance.Add((student, distance));
            }

            // Sort students by distance
            var sortedStudents = studentsWithDistance.OrderBy(s => s.distance).ToList();

            // Group students in groups of 4
            var groupedStudents = new List<List<Student>>();
            for (int i = 0; i < sortedStudents.Count; i += 4)
            {
                groupedStudents.Add(sortedStudents.Skip(i).Take(4).Select(s => s.student).ToList());
            }

            return Ok(groupedStudents);
        }

        // Helper function to check if a student exists
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
