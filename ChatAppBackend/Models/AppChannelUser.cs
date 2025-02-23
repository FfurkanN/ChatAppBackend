using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackend.Models
{
    public class AppChannelUser
    {
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        [Required]
        public Guid ChannelId { get; set; }

        [ForeignKey(nameof(ChannelId))]
        public AppChannel Channel { get; set; }

        public int UnreadMessageCount { get; set; } = 0;
    }
}
