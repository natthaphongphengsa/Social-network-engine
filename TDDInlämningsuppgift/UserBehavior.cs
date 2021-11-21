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
        public bool FollowAnotherUser(string user1, string user2)
        {
            return false;
        }
    }
}
