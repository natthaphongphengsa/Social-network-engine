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
        [DataRow("Natt: Hello test1", "Hello test1")]
        public void PostTest(string expected, string message)
        {
            var databas = new ApplicationDb();
            User testUser = databas.Users.First(u => u.Username == "Natt123");

            var newPost = new Post();
            newPost.PostedBy = testUser;
            newPost.Message = message;

            var userBehavior = new UserBehavior(databas);
            var result = userBehavior.Post(newPost);

            var postFromDatabase = databas.Posters.First(p => p.PostedBy == newPost.PostedBy);
            string testValue = $"{postFromDatabase.PostedBy.Name}: {postFromDatabase.Message}";

            Assert.AreEqual(expected, testValue);
        }
        [TestMethod]
        public void TimelineTest()
        {

        }
        [TestMethod]
        public void FollowTest()
        {

        }
        [TestMethod]
        public void GetFollowingUserListTest()
        {

        }
    }
}
