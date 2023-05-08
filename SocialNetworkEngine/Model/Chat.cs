using System;

namespace SocialNetworkEngine.Model
{
    public class Chat
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public User SendTo { get; set; }
        public int SendFromId { get; set; }
    }
}
