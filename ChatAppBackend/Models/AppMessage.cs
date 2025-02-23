using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ChatAppBackend.Models
{
    public class AppMessage
    {
        public AppMessage()
        {
            Id = new Guid();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid Chat_Id { get; set; }
        [Required]
        public Guid Sender_Id { get; set; }
        [Required]
        public string MessageType { get; set; } = "text";
        [AllowNull]
        public string Content { get; set; } = "";
        [AllowNull]
        public string? FileName { get; set; } = "";
        [AllowNull]
        public string? FileUrl { get; set; } = "";
        [AllowNull]
        public long? FileSize { get; set; }  

        [Required]
        public DateTime Send_Date { get; set; }
        [AllowNull]
        public DateTime Receive_Date { get; set; }
    }
}
