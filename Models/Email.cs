using System.ComponentModel.DataAnnotations;
namespace CRMServerApi.Models
{
    public class EmailRequest
    {
        [Required]
        public List<string> Emails { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
