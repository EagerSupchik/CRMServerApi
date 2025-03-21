using System.ComponentModel.DataAnnotations;

namespace CRMServerApi.Models
{
    public class CrmDeal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; } 

        public DateTime? CloseDate { get; set; }

        public string Status { get; set; } = "New";

    }
}
