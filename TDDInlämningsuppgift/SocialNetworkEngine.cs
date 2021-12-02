using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDInlämningsuppgift.Data;
using TDDInlämningsuppgift.Model;

namespace TDDInlämningsuppgift
{
    public class SocialNetworkEngine
    {
        public ApplicationDb database = new ApplicationDb();
        public SocialNetworkEngine(ApplicationDb dbcontext)
        {
            database = dbcontext;
        }
        public resultStatus Login(string username, string userPassword)
        {
            if (database.Users.Any(u => u.Username == username && u.Password == userPassword))
            {
                return resultStatus.IsSuccess;
            }
            else
            {
                return resultStatus.IsFaild;
            }
        }
        public User CreateNewAccount(string username, string password)
        {
            User newUser = new User()
            {
                Username = username,
                Password = password
            };
            return newUser;
        }
        public Post Post(string username, string message)
        {
            var user = database.Users.First(u => u.Username == username);

            Post newPost = new Post();

            //string newMessage = "";
            if (message.IndexOf('@') != -1)
            {
                string messageString = message.TrimStart('@');
                string userString = messageString.Remove(messageString.IndexOf(' '));
                if (database.Users.Any(u => u.Username == userString))
                {
                    newPost.Message = messageString;
                    newPost.Date = DateTime.Now;
                    newPost.PostedBy = user;
                    return newPost;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                newPost.Message = message;
                newPost.PostedBy = user;
                newPost.Date = DateTime.Now;
                return newPost;
            }
        }
        public Chat SendMessage(string to, string from, string message)
        {
            var user1 = database.Users.FirstOrDefault(u => u.Username == to);
            var user2 = database.Users.FirstOrDefault(u => u.Username == from);
            var chat = database.Chats.FirstOrDefault(c => c.Text == message);

            if (user1 != null && user1 != null && chat == null && message != "")
            {
                Chat newChat = new Chat();
                newChat.SendFromId = database.Users.First(u => u.Username == from).Id;
                newChat.SendTo = database.Users.First(u => u.Username == to);
                newChat.Date = DateTime.Now;
                newChat.Text = message;
                //database.Chats.Add(new Chat()
                //{
                //    SendFromId = database.Users.First(u => u.Username == from).Id,
                //    SendTo = database.Users.First(u => u.Username == to),
                //    Date = DateTime.Now,
                //    Text = message,
                //});
                //database.SaveChanges();
                return newChat;
            }
            else
            {
                return null;
            }
        }
        public User StartFollow(string user, string anotherUser)
        {
            if (database.Users.Any(u => u.Username == user) && database.Users.Any(u => u.Username == anotherUser))
            {
                User userToFollow = database.Users.First(u => u.Username == anotherUser);

                User myself = database.Users.Include(u => u.Following).First(u => u.Username == user);
                if (!myself.Following.Any(u => u.Username == userToFollow.Username))
                {
                    myself.Following.Add(userToFollow);
                    return myself;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<Post> GetTimeline(string username)
        {
            var postList = database.Posters
                .Include(user => user.PostedBy)
                .Include(f => f.PostedBy.Following)
                .Include(f => f.PostedBy.Follower)
                .Where(u => u.PostedBy.Username == username).ToList();
            if (postList != null)
            {
                return postList;
            }
            return null;
        }
        public List<User> GetFollowingList(string username)
        {
            var followingList = database.Users.FirstOrDefault(u => u.Username == username).Following;
            return followingList;
        }
        public List<User> ViewUserWall(string user)
        {
            //return database.Users.FirstOrDefault(u => u.Username == user).Following.Select(u=>u.Following);

            return database.Users
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Username == user)
                .Following.ToList();
            //.Following
            //.Include(p => p.Posters)
            //.Include(c => c.Chats)
            //.Include(u => u.Follower)
            //.Where(u => u.Username == user)

        }
    }
}
