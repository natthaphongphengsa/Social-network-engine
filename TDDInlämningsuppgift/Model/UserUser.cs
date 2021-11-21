using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDInlämningsuppgift.Model
{
    public class UserUser
    {
        public int UserId { get; set; }
        public List<User> FollowerId { get; set; }
        public List<User> FollowingId { get; set; }
    }
}
