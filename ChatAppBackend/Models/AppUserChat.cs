using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackend.Models
{
    public class AppUserChat
    {
        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        [Required]
        [ForeignKey("ChatId")]
        public Guid ChatId { get; set; }
        public int UnreadMessageCount { get; set; } = 0;
    }
    }
