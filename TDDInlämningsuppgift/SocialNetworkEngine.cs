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
        public resultStatus CreateNewAccount(User user)
        {
            if (!database.Users.Any(u => u.Username == user.Username))
            {
                database.Users.Add(user);
                database.SaveChanges();
                return resultStatus.IsSuccess;
            }
            else
            {
                return resultStatus.IsFaild;
            }
        }
        public bool Post(Post post)
        {
            if (!database.Posters.Include(u => u.PostedBy).Any(p => p.Message == post.Message && p.PostedBy == post.PostedBy))
            {
                database.Posters.Add(post);
                database.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public resultStatus SendMessage(string to, string from, string message)
        {
            var user1 = database.Users.FirstOrDefault(u => u.Username == to);
            var user2 = database.Users.FirstOrDefault(u => u.Username == from);
            var chat = database.Chats.FirstOrDefault(c => c.Text == message);

            if (user1 != null && user1 != null && chat == null && message != "")
            {
                database.Chats.Add(new Chat()
                {
                    SendFromId = database.Users.First(u => u.Username == from).Id,
                    SendTo = database.Users.First(u => u.Username == to),
                    Date = DateTime.Now,
                    Text = message,
                });
                database.SaveChanges();
                return resultStatus.IsSuccess;
            }
            else
            {
                return resultStatus.IsFaild;
            }
        }
        public List<User> StartFollow(User me, string user)
        {
            if (database.Users.Any(u => u.Username == me.Username) && database.Users.Any(u => u.Username == user))
            {
                List<User> anotheruser = database.Users
                    .Include(u => u.Following)
                    .Include(u => u.Follower)
                    .Where(u => u.Username == user).ToList();

                if (!anotheruser.Any(u => u == me))
                {
                    if (!me.Following.Any(u => u == anotheruser.Where(u => u.Username == user)))
                    {
                        var userTofollow = database.Users.First(u => u.Username == user);
                        me.Following.Add(userTofollow);
                        database.Update(me);
                        database.SaveChanges();
                        return anotheruser;
                    }
                }
                return null;
            }
            else
                return null;
        }
        public List<Post> GetTimeline(string username)
        {
            var postList = database.Posters
                .Include(user => user.PostedBy)
                .Include(f => f.PostedBy.Following)
                .Include(f => f.PostedBy.Follower)
                .Where(u => u.PostedBy.Username == username).ToList();
            return postList;
        }
        public List<User> GetFollowingList(string username)
        {
            var followingList = database.Users.FirstOrDefault(u => u.Username == username).Following;
            return followingList;
        }
        public List<User> ViewUserWall(string user)
        {
            return database.Users.Include(u => u.Follower).Include(u => u.Following).First(u => u.Username == user).Following.ToList();
        }
    }
}
