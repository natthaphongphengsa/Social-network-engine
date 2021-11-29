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
        [DataRow(ResultStatus.IsFaild, "Charlie", "Alice")]
        public void FollowTest(ResultStatus ExpectedValue, string user, string anotherUser)
        {
            //var database = new ApplicationDb();
            User me = database.Users.First(u => u.Username == user);

            var userBehavior = new SocialNetworkEngine(database);
            var actual = userBehavior.StartFollow(me, anotherUser);

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
        [DataRow(ResultStatus.IsSuccess,"Charlie")]
        public void WallTest(string username)
        {

        }
        [TestMethod]
        public void SendMessageTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        [DataRow(ResultStatus.IsSuccess, "Alice", "1234")]
        [DataRow(ResultStatus.IsFaild, "Natta", "1321")]
        [DataRow(ResultStatus.IsSuccess, "Bob", "5678")]
        public void LoginTest(ResultStatus expextedResult, string username, string password)
        {
            var actualResult = socialNetworkEngine.Login(username, password);

            Assert.AreEqual(expextedResult, actualResult);
        }

        [TestMethod]
        [DataRow(ResultStatus.IsFaild, "Alice", "1234")]
        public void CreateAccountTest(ResultStatus expectedValue, string testUsernameValue, string testPasswordValue)
        {
            //Arrange
            User newUser = new User();
            newUser.Username = testUsernameValue;
            newUser.Password = testPasswordValue;
            //Act
            ResultStatus result = socialNetworkEngine.CreateNewAccount(newUser);
            //Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
