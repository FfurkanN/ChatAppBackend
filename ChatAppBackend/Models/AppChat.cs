using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ChatAppBackend.Models
{
    public sealed class AppChat
    {
        public AppChat()
        {
            Id = new Guid();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        [ForeignKey("UserId")]
        public Guid Creator_Id { get; set; }
        public DateTime Create_Date { get; set; } = DateTime.Now;
        public Boolean isPublic { get; set; } = false;
        public int unreadMessageCount { get; set; } = 0;
    }
}
