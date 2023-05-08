using SocialNetworkEngine.Model;
using System.Collections.Generic;

namespace SocialNetworkEngine
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Post> Posters { get; set; }
        public List<User> Follower { get; set; }
        public List<User> Following { get; set; }
        public List<Chat> Chats { get; set; }

    }
}
