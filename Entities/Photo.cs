using System;

namespace ChatBox.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string PictureUrl { get; set; }
        public string FileName { get; set; }
        public bool IsMain { get; set; }
        public string AppUserId { get; set; }
    }
}
