using System.ComponentModel.DataAnnotations;

namespace CRMServerApi.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

    }
}
