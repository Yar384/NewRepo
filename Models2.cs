using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieApp
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }

    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Range(1, int.MaxValue)]
        public int Year { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
