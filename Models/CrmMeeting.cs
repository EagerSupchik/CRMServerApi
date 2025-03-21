using System.ComponentModel.DataAnnotations;

namespace CRMServerApi.Models
{
    public class CrmMeeting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public DateTime MeetingTime { get; set; }
        public string Subject { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
