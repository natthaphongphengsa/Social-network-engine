using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDDInlämningsuppgift;
using TDDInlämningsuppgift.Data;
using TDDInlämningsuppgift.Model;

namespace TestUserBehavior
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow(true, "Hello test1", "Alice123")]
        [DataRow(true, "Today is to cold ", "Bob123")]
        public void PostTest(bool expected, string message, string user)
        {
            var _database = new ApplicationDb();
            User testUser = _database.Users.First(u => u.Username == user);

            var newPost = new Post();
            newPost.PostedBy = testUser;
            newPost.Message = message;
            newPost.Datum = DateTime.Now;

            var userBehavior = new UserBehavior(_database);
            var resultValue = userBehavior.Post(newPost);

            Assert.AreEqual(expected, resultValue);
        }
        [TestMethod]
        public void TimelineTest(string expected, string followUser, string FollowingUser)
        {

        }
        [TestMethod]
        [DataRow(true, "Alice123", "Bob123")]
        public void FollowTest(bool succesFollow, string me, string anotherUser)
        {
            var database = new ApplicationDb();
            var userBehavior = new UserBehavior(database);
            bool actual = userBehavior.FollowAnotherUser(me, anotherUser);
            Assert.AreEqual(succesFollow, actual);
        }
    }
}
