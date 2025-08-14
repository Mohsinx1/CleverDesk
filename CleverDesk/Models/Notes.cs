using System;
using System.ComponentModel.DataAnnotations;

namespace CleverDesk.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int NotebookId { get; set; }

        public Notebook? Notebook { get; set; }
    }
}
