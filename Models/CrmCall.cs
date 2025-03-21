using System.ComponentModel.DataAnnotations;

namespace CRMServerApi.Models
{
    public class CrmCall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public DateTime CallTime { get; set; }
        public string Subject { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

    }
}
