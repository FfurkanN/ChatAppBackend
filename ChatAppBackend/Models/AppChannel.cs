using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ChatAppBackend.Models
{
    public class AppChannel
    {
        public AppChannel()
        {
            Id = new Guid();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [AllowNull]
        public string ChannelImageUrl { get; set; }
        [AllowNull]
        public string Description { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public Guid Creator_Id { get; set; }
        public DateTime Create_Date { get; set; } = DateTime.Now;
        public Boolean isPublic { get; set; } = false;
        [JsonIgnore]
        public ICollection<AppChannelChat> ChannelChats { get; set; }
        public ICollection<AppChannelUser> ChannelUser { get; set; }
    }
}
