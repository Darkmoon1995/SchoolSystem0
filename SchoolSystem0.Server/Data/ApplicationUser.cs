using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}