using Microsoft.EntityFrameworkCore;
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
    public class UserBehaviorTest
    {
        [TestMethod]
        [DataRow(false, "What a wonderfully sunny day!", "Alice")]
        [DataRow(false, "Today is to cold ", "Bob")]
        public void PostTest(bool expected, string message, string user)
        {
            var _database = new ApplicationDb();
            User testUser = _database.Users.First(u => u.Username == user);

            var userBehavior = new UserBehavior(_database);
            var resultValue = userBehavior.Post(testUser, message);

            Assert.AreEqual(expected, resultValue);
        }
        [TestMethod]
        public void TimelineTest(string expected, string followUser, string FollowingUser)
        {

        }
        [TestMethod]
        [DataRow(Result.IsFaild, "Alice", "Bob")]
        public void FollowTest(Result ExpectedValue, string user, string anotherUser)
        {
            var database = new ApplicationDb();
            User me = database.Users.First(u => u.Username == user);

            var userBehavior = new UserBehavior(database);
            var actual = userBehavior.StartFollow(me, anotherUser);

            Assert.AreEqual(ExpectedValue, actual);
        }
        [TestMethod]
        public void GetTimelineTest()
        {
            var database = new ApplicationDb();
            var userBehavior = new UserBehavior(database);
            List<Post> result = userBehavior.GetTimeline("Alice").ToList();

            var user = database.Users.Include(u => u.Follower).Include(u => u.Following).First(u => u.Username == "Alice");

            Post newPost = new Post()
            {
                Message = "What a wonderfully sunny day!",
                PostedBy = user
            };
            List<Post> expectedList = new List<Post>();
            expectedList.Add(newPost);


            Assert.AreEqual(expectedList, result);
        }
        [TestMethod]
        public void SendMessageTest()
        {

        }
    }
}
