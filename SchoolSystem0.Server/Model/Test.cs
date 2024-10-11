using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem0.Server.Models
{
    public class Test
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Foreign key for Teacher
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }  

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string QuestionType { get; set; }  // e.g., "MultipleChoice", "TrueFalse"

        public ICollection<string> Options { get; set; } = new List<string>();  // For multiple-choice questions

        public string CorrectAnswer { get; set; }  // For multiple-choice or true/false questions

        public int Points { get; set; }  // Points for the question

        
        public string ImagePath { get; set; }  // File path or URL for the image

        // Foreign key for Test
        public int TestId { get; set; }
        public Test Test { get; set; }  // Navigation property
    }
    public class QuestionRequest
    {
        public string Text { get; set; }

        public string QuestionType { get; set; }

        public List<string> Options { get; set; } = new List<string>();

        public string CorrectAnswer { get; set; }

        public int Points { get; set; }

        // Optional Image
        public IFormFile Image { get; set; }
    }

}
