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
    public class UserBehavior
    {
        public ApplicationDb database;
        public UserBehavior(ApplicationDb dbcontext)
        {
            database = dbcontext;
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
        public void SendMessage(User to, User from, string message)
        {
            database.Chats.Add(new Chat()
            {
                SendFromId = from.Id,
                SendTo = database.Users.First(u => u.Id == to.Id),
                Date = DateTime.Now,
                Text = message,
            });
            database.SaveChanges();
        }
        public Result StartFollow(User me, string anotherUser)
        {
            if (database.Users.Any(u => u.Username == me.Username) && database.Users.Any(u => u.Username == anotherUser))
            {
                var user = database.Users.Include(u => u.Following).Include(u => u.Follower).First(u => u.Username == me.Username);
                var anotheruser = database.Users.Include(u => u.Following).Include(u => u.Follower).First(u => u.Username == anotherUser);

                if (user != anotheruser)
                {
                    if (!user.Following.Any(u => u == anotheruser))
                    {
                        user.Following.Add(anotheruser);
                        database.Update(user);
                        database.SaveChanges();
                        return Result.IsSuccess;
                    }
                }
                return Result.IsFaild;
            }
            else
                return Result.IsFaild;
        }
        public List<Post> GetTimeline(string user)
        {
            var postList = database.Posters.Include(user => user.PostedBy).Include(f => f.PostedBy.Following).Include(f => f.PostedBy.Follower).Where(u => u.PostedBy.Username == user).ToList();
            return postList;
        }
        public List<User> GetFollowingList(string username)
        {
            var followingList = database.Users.First(u => u.Username == username).Following;
            return followingList;
        }
    }
}
