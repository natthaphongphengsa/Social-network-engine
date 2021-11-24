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
        public bool FollowAnotherUser(string me, string anotherUser)
        {
            if (_database.Users.Any(u => u.Username == me) && _database.Users.Any(u => u.Username == anotherUser))
            {
                var user = _database.Users.First(u => u.Username == me);
                var anotheruser = _database.Users.First(u => u.Username == anotherUser);

                List<User> FollowongUserList = new List<User>();
                FollowongUserList.Add(anotheruser);
                user.Following = FollowongUserList;

                _database.Update(user);
                _database.SaveChanges();
                return true;
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
