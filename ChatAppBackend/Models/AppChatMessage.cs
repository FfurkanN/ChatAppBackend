using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackend.Models
{
    public class AppChatMessage
    {
        public Guid ChatId { get; set; }
        [ForeignKey(nameof(ChatId))]
        public AppChat Chat { get; set; }
        public Guid MessageId { get; set; }
        [ForeignKey(nameof(MessageId))]
        public AppMessage Message { get; set; }
    }
}
