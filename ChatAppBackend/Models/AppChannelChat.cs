using System.ComponentModel.DataAnnotations;

namespace ChatAppBackend.Models
{
    public class AppChannelChat
    {
        [Required]
        public Guid ChannelId { get; set; }

        public AppChannel Channel { get; set; }

        [Required]
        public Guid ChatId { get; set; }

        public AppChat Chat { get; set; }
    }
}
