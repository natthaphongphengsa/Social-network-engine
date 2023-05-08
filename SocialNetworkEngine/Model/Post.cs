using System;

namespace SocialNetworkEngine.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public User PostedBy { get; set; }
    }
}
