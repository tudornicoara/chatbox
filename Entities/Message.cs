using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBox.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
    }
}