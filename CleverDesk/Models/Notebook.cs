using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CleverDesk.Models
{
    public class Notebook
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        [Required]
        public int UserId { get; set; }

        public List<Note> Notes { get; set; } = new();
    }
}
