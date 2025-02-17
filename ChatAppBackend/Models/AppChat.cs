﻿using System.ComponentModel.DataAnnotations;
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
        public Guid Creator_Id { get; set; }
        public List<Guid> Members { get; set; } = new List<Guid>();
        public DateTime Create_Date { get; set; } = DateTime.Now;
        public Boolean isPublic { get; set; } = false;
        public List<Guid> Messages { get; set; } = new List<Guid>();
        public int unreadMessageCount { get; set; } = 0;
    }
}
