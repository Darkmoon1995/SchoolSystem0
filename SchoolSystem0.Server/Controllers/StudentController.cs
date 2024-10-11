using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Server.Models;
using SchoolSystem0.Server.Data;
using SchoolSystem0.Server.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolSystem0.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            // Get user who is associated with the student
            var studentUser = await _userManager.FindByEmailAsync(student.ContactInformation.Email);

            // 1. Allow access if the user is a Teacher, BaseManager, or Manager
            if (currentUserRoles.Contains("Teacher") || currentUserRoles.Contains("BaseManager") || currentUserRoles.Contains("Manager"))
            {
                return Ok(student);
            }

            // 2. Allow access if the student is accessing their own data
            if (studentUser != null && currentUserId == studentUser.Id)
            {
                return Ok(student);
            }

            // 3. If the user is not authorized
            return Forbid();
        }
        [HttpGet("ByToken")]
        [Authorize] // Ensure the user is authenticated
        public async Task<ActionResult<Student>> GetStudentByToken()
        {
            // Get the current authenticated user from the JWT token
            var currentUser = await _userManager.GetUserAsync(User);

            // Check if the current user is found
            if (currentUser == null)
            {
                return Unauthorized(new { message = "User is not authenticated or could not be found" });
            }

            // Extract the user's email or other identifier from the token
            var currentUserEmail = currentUser.Email;

            // Fetch the student associated with the current user's email
            var student = await _context.Students
                .Include(s => s.Grades)
                .Include(s => s.Costs)
                .FirstOrDefaultAsync(s => s.ContactInformation.Email.ToLower() == currentUserEmail.ToLower());

            // If no student is found, return NotFound
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            // Get the roles of the current user
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            // 1. Allow access if the user is a Teacher, BaseManager, or Manager
            if (currentUserRoles.Contains("Teacher") || currentUserRoles.Contains("BaseManager") || currentUserRoles.Contains("Manager"))
            {
                return Ok(student);
            }

            // 2. Allow access if the student is accessing their own data
            if (currentUserEmail == student.ContactInformation.Email)
            {
                return Ok(student);
            }

            // 3. If the user is not authorized
            return Forbid();
        }


        // PUT: api/Students/{id} (For updating student data)
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, BaseManager, Manager")] // Only these roles can update student data
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

            // Update logic (only teachers/managers can update)
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

        // POST: api/Students (For adding new students - Only Teacher, BaseManager, Manager can add)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/{id} (Only Teacher, BaseManager, Manager can delete students)
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

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
