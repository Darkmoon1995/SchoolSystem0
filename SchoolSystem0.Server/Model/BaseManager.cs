using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem0.Server.Models
{
    public class BaseManager
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Contact { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [Range(0, double.MaxValue)] // Salary for the base manager
        public decimal Salary { get; set; }

        // Navigation property for the teachers they manage
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        // Navigation property for the classes they manage
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
