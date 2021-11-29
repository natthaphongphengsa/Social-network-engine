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
        public ResultStatus Login(string username, string userPassword)
        {
            if (database.Users.Any(u => u.Username == username && u.Password == userPassword))
            {
                return ResultStatus.IsSuccess;
            }
            else
            {
                return ResultStatus.IsFaild;
            }
        }
        public ResultStatus CreateNewAccount(User user)
        {
            if (!database.Users.Any(u => u.Username == user.Username))
            {
                database.Users.Add(user);
                database.SaveChanges();
                return ResultStatus.IsSuccess;
            }
            else
            {
                return ResultStatus.IsFaild;
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
        public List<User> StartFollow(User me, string user)
        {
            if (database.Users.Any(u => u.Username == me.Username) && database.Users.Any(u => u.Username == user))
            {
                //var user = database.Users.Include(u => u.Following).Include(u => u.Follower).First(u => u.Username == me.Username);
                List<User> anotheruser = database.Users.Include(u => u.Following).Include(u => u.Follower).Where(u => u.Username == user).ToList();
                
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
        public List<Post> GetTimeline(string user)
        {
            var postList = database.Posters.Include(user => user.PostedBy).Include(f => f.PostedBy.Following).Include(f => f.PostedBy.Follower).Where(u => u.PostedBy.Username == user).ToList();
            return postList;
        }
        public List<User> GetFollowingList(string username)
        {
            var followingList = database.Users.FirstOrDefault(u => u.Username == username).Following;
            return followingList;
        }
        public List<User> ViewUserWall(User user)
        {
            return database.Users.Include(f => f.Follower).Include(f => f.Following).Select(u => u.Follower.FirstOrDefault(u => u == user)).ToList(); ;
        }
    }
}
