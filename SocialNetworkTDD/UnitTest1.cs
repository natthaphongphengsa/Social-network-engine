using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using SocialNetworkEngine;
using SocialNetworkEngine.Data;
using SocialNetworkEngine.Model;

namespace SocialNetworkTDD
{
    [TestClass]
    public class SocialNetWorkTestTDD
    {
        public ApplicationDb database = new ApplicationDb();
        public SocialNetworkEngineIntegration socialNetworkEngine => new SocialNetworkEngineIntegration(database);

        [TestMethod]
        [DataRow(resultStatus.IsSuccess, "What a wonderfully sunny day!", "Alice")]
        [DataRow(resultStatus.IsSuccess, "Today is to cold", "Bob")]
        public void PostTest(resultStatus expected, string message, string user)
        {
            //var _database = new ApplicationDb();
            User testUser = database.Users.First(u => u.Username == user);

            Post expectedPost = new Post();
            expectedPost.Message = message;
            expectedPost.PostedBy = testUser;

            var userBehavior = new SocialNetworkEngineIntegration(database);
            Post actualPost = userBehavior.Post(user, message);

            resultStatus actualResult;
            if (actualPost.Message == message && actualPost.PostedBy == expectedPost.PostedBy)
            {
                actualResult = resultStatus.IsSuccess;
            }
            else
            {
                actualResult = resultStatus.IsFaild;
            }

            Assert.AreEqual(expected, actualResult);
        }

        [TestMethod]
        [DataRow(resultStatus.IsFaild, "Charlie", "Alicde")]
        [DataRow(resultStatus.IsSuccess, "Charlie", "Alice")]
        public void FollowTest(resultStatus ExpectedValue, string user, string anotherUser)
        {
            //var database = new ApplicationDb();
            //User me = database.Users.First(u => u.Username == user);

            var userBehavior = new SocialNetworkEngineIntegration(database);
            var userProfile = userBehavior.StartFollow(user, anotherUser);

            resultStatus actual = new resultStatus();
            if (userProfile == null)
            {
                actual = resultStatus.IsFaild;
            }
            else if (userProfile.Following.Any(u => u.Username == anotherUser))
            {
                actual = resultStatus.IsSuccess;
            }
            else
            {
                actual = resultStatus.IsFaild;
            }

            Assert.AreEqual(ExpectedValue, actual);
        }

        [TestMethod]
        [DataRow("What a wonderfully sunny day!", "Alice")]
        [DataRow("Today is to cold", "Bob")]
        public void GetTimelineTest(string expectedMessage, string username)
        {
            string actualMessage;
            if (socialNetworkEngine.GetTimeline(username).Any(u => u.Message != ""))
            {
                actualMessage = socialNetworkEngine.GetTimeline(username).First(u => u.PostedBy.Username == username).Message;
            }
            else
            {
                actualMessage = null;
            }

            Assert.AreNotEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        [DataRow(0, "Alice")]
        [DataRow(0, "Bob")]
        [DataRow(0, "Charlie")]
        public void WallTest(int expectedFollowingCount, string username)
        {
            List<User> actualUsers = socialNetworkEngine.ViewUserWall(username);

            Assert.AreEqual(expectedFollowingCount, actualUsers.Count());
        }

        [TestMethod]
        [DataRow(resultStatus.IsSuccess, "Alice", "Mallory", "Hello Alice! how are you today?")]
        [DataRow(resultStatus.IsSuccess, "Mallory", "Alice", "Hello mallory! I am fine, how about you?")]
        public void SendMessageTest(resultStatus expectedResult, string sendTo, string sendFrom, string message)
        {
            var actualChat = socialNetworkEngine.SendMessage(sendTo, sendFrom, message);

            var sendToThisUser = database.Users.First(u => u.Username == sendTo);
            var sendFromThisUser = database.Users.First(u => u.Username == sendFrom).Id;

            resultStatus actualResult;
            if (actualChat.SendFromId == sendFromThisUser && actualChat.SendTo == sendToThisUser)
            {
                actualResult = resultStatus.IsSuccess;
            }
            else
            {
                actualResult = resultStatus.IsFaild;
            }

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
        [DataRow(resultStatus.IsSuccess, "Alice", "1234")]
        public void CreateAccountTest(resultStatus expectedValue, string testUsernameValue, string testPasswordValue)
        {
            User newUser = new User();
            newUser.Username = testUsernameValue;
            newUser.Password = testPasswordValue;

            User user = socialNetworkEngine.CreateNewAccount(testUsernameValue, testPasswordValue);

            resultStatus actualresult;
            if (user.Username == testUsernameValue && user.Password == testPasswordValue)
            {
                actualresult = resultStatus.IsSuccess;
            }
            else
            {
                actualresult = resultStatus.IsFaild;
            }

            Assert.AreEqual(expectedValue, actualresult);
        }
    }
}
