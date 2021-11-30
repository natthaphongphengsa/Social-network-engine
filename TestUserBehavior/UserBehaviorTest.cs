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
        public ApplicationDb database = new ApplicationDb();
        public SocialNetworkEngine socialNetworkEngine => new SocialNetworkEngine(database);

        [TestMethod]
        [DataRow(false, "What a wonderfully sunny day!", "Alice")]
        [DataRow(false, "Today is to cold ", "Bob")]
        public void PostTest(bool expected, string message, string user)
        {
            //var _database = new ApplicationDb();
            User testUser = database.Users.First(u => u.Username == user);

            Post post = new Post();
            post.Message = message;
            post.PostedBy = testUser;
            post.Datum = DateTime.Now;

            var userBehavior = new SocialNetworkEngine(database);
            var resultValue = userBehavior.Post(post);

            Assert.AreEqual(expected, resultValue);
        }

        [TestMethod]
        [DataRow(resultStatus.IsFaild, "Charlie", "Alice")]
        public void FollowTest(resultStatus ExpectedValue, string user, string anotherUser)
        {
            //var database = new ApplicationDb();
            User me = database.Users.First(u => u.Username == user);
            var userBehavior = new SocialNetworkEngine(database);
            resultStatus actual;

            if (userBehavior.StartFollow(me, anotherUser) != null)
            {
                actual = resultStatus.IsFaild;
            }
            else
                actual = resultStatus.IsSuccess;

            Assert.AreEqual(ExpectedValue, actual);
        }

        [TestMethod]
        [DataRow("What a wonderfully sunny day!", "Alice")]
        [DataRow("Today is to cold ", "Bob")]
        public void GetTimelineTest(string expectedMessage, string username)
        {
            var actualResult = socialNetworkEngine.GetTimeline(username).FirstOrDefault(u => u.PostedBy.Username == username).Message.ToString();

            Assert.AreEqual(expectedMessage, actualResult);
        }

        [TestMethod]
        [DataRow(2, "Alice")]
        [DataRow(2, "Bob")]
        [DataRow(1, "Charlie")]
        public void WallTest(int expected, string username)
        {
            List<User> actualUsers = socialNetworkEngine.ViewUserWall(username);

            Assert.AreEqual(expected, actualUsers.Count());
        }

        [TestMethod]
        [DataRow(resultStatus.IsFaild, "Alice", "Mallory", "Hello Alice! how are you today?")]
        [DataRow(resultStatus.IsFaild, "Mallory", "Alice", "Hello mallory! I am fine, how about you?")]
        public void SendMessageTest(resultStatus expectedResult, string sendTo, string sendFrom, string message)
        {
            var actualResult = socialNetworkEngine.SendMessage(sendTo, sendFrom, message);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [DataRow(resultStatus.IsSuccess, "Alice", "1234")]
        [DataRow(resultStatus.IsFaild, "Natta", "1321")]
        [DataRow(resultStatus.IsSuccess, "Bob", "5678")]
        public void LoginTest(resultStatus expextedResult, string username, string password)
        {
            var actualResult = socialNetworkEngine.Login(username, password);

            Assert.AreEqual(expextedResult, actualResult);
        }

        [TestMethod]
        [DataRow(resultStatus.IsFaild, "Alice", "1234")]
        public void CreateAccountTest(resultStatus expectedValue, string testUsernameValue, string testPasswordValue)
        {
            User newUser = new User();
            newUser.Username = testUsernameValue;
            newUser.Password = testPasswordValue;

            resultStatus result = socialNetworkEngine.CreateNewAccount(newUser);

            Assert.AreEqual(expectedValue, result);
        }
    }
}
