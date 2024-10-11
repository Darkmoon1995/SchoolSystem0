using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem0.Server.Models
{
    public class Manager
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
        [Range(0, double.MaxValue)] // Salary for the top-level manager
        public decimal Salary { get; set; }

        // Navigation property for the base managers they manage
        public ICollection<BaseManager> BaseManagers { get; set; } = new List<BaseManager>();

        // Navigation property for the teachers they manage directly
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        // A list of classes that the manager may directly manage
        public ICollection<Class> Classes { get; set; } = new List<Class>();

        // Administrative rights over school operations (e.g., budget, curriculum)
        public bool HasBudgetControl { get; set; }
        public bool HasCurriculumControl { get; set; }
    }
}
