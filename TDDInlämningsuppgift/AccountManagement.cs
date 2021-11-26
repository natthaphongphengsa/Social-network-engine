using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDInlämningsuppgift.Data;

namespace TDDInlämningsuppgift
{
    public class AccountManagement
    {
        public ApplicationDb database;
        public AccountManagement(ApplicationDb dbcontext)
        {
            database = dbcontext;
        }
        public Result Login(User user)
        {
            if (database.Users.Any(u => u.Username == user.Username && u.Password == user.Password))
            {
                return Result.IsSuccess;
            }
            else
            {
                return Result.IsFaild;
            }
        }
        public Result CreateNewAccount(User user)
        {
            if (!database.Users.Any(u => u.Username == user.Username))
            {
                database.Users.Add(user);
                database.SaveChanges();
                return Result.IsSuccess;
            }
            else
            {
                return Result.IsFaild;
            }
        }
    }
}
