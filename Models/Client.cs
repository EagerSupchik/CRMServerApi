using System.ComponentModel.DataAnnotations;

namespace CRMServerApi.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; } = string.Empty;

    }
}
