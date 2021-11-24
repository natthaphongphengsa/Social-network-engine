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
        public ApplicationDb _database;
        public UserBehavior(ApplicationDb dbcontext)
        {
            _database = dbcontext;
        }
        public bool Post(Post post)
        {
            if (_database.Posters.Any(p => p.Message == post.Message && p.PostedBy == post.PostedBy) != true)
            {
                _database.Posters.Add(post);
                _database.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SendMessage(User to, User from, string message)
        {
            Chat chat = new Chat();
            chat.Text = message;
            chat.SendTo = to;
            chat.SendFromId = from.Id;
            chat.Date = DateTime.Now;

            _database.Chats.Add(chat);
            _database.SaveChanges();
        }
        public bool StartFollow(User me, string anotherUser)
        {
            if (_database.Users.Any(u => u.Username == me.Username) && _database.Users.Any(u => u.Username == anotherUser))
            {
                var user = _database.Users.First(u => u.Username == me.Username);
                var anotheruser = _database.Users.First(u => u.Username == anotherUser);

                List<User> FollowingUserList = new List<User>();
                FollowingUserList.Add(anotheruser);

                if (user != anotheruser)
                {
                    if (!user.Following.Any(u => u == anotheruser))
                    {
                        user.Following = FollowingUserList;
                        _database.Update(user);
                        _database.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            else
                return false;
        }
        public bool StopFollow(User me, string anotherUser)
        {
            if (_database.Users.Any(u => u.Username == anotherUser))
            {
                var user = _database.Users.Include(u => u.Following).First(u => u.Username == me.Username);
                var anotheruser = _database.Users.First(u => u.Username == anotherUser);

                if (user != anotheruser)
                {
                    if (!user.Following.Any(u => u == anotheruser))
                    {
                        user.Following.Remove(anotheruser);

                        _database.Update(user);
                        _database.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            else
                return false;
        }
        public List<Post> TimeLine(User user)
        {
            var post = _database.Posters.Where(p => p.PostedBy == user).Include(User => user.Following).ToList();

            List<Post> postList = new List<Post>();
            return postList;
        }
    }
}
