using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [DataRow(false, "Hello test1", "Natt123")]
        [DataRow(true, "Today is to cold", "Markus123")]
        public void PostTest(bool expected, string message, string user)
        {
            var _database = new ApplicationDb();
            User testUser = _database.Users.First(u => u.Username == user);

            var newPost = new Post();
            newPost.PostedBy = testUser;
            newPost.Message = message;

            var userBehavior = new UserBehavior(_database);
            var resultValue = userBehavior.Post(newPost);

            Assert.AreEqual(expected, resultValue);
        }
        [TestMethod]
        public void TimelineTest()
        {

        }
        [TestMethod]
        public void FollowTest()
        {
            using (var database = new ApplicationDb())
            {
                var User1 = new User()
                {
                    Name = "Natt",
                    Username = "Natt123",
                    Password = "tgefdg",
                };
                var newpost = new Post();
                newpost.Id = 1;
                newpost.Message = "sajfskaj sakjkjsa";
                newpost.PostedBy = User1;

                var User2 = new User()
                {
                    Name = "Markus",
                    Username = "Markus123",
                    Password = "tsdgsdgsd",
                };
                var newpost2 = new Post();
                newpost2.Id = 1;
                newpost2.Message = "Test jaksas skjsak kjsakf";
                newpost2.PostedBy = User2;

                var User3 = new User()
                {
                    Name = "Markus",
                    Username = "Markus123",
                    Password = "tsdgsdgsd",
                };
                var newpost3 = new Post();
                newpost3.Id = 1;
                newpost3.Message = "Test jaksas skjsak kjsakf";
                newpost3.PostedBy = User3;

                List<User> FollowerUsers = new List<User>();
                FollowerUsers.Add(User2);
                FollowerUsers.Add(User3);

                List<User> followingUsers = new List<User>();
                followingUsers.Add(User2);


                var testAccount = database.Users.First(u => u.Name == "Natta");
                testAccount.Follower = FollowerUsers;
                testAccount.Following = followingUsers;

                database.Update(testAccount);
                database.SaveChanges();

            }           
        }
    }
}
