using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDDInlämningsuppgift;
using TDDInlämningsuppgift.Model;

namespace UserBehaviorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PostTest()
        {
            var post = new Post();
            post.Message = "Hello! today is my birthday!";

            var user = new User();
            user.Name = "Natt";
            user.Poster = post;

            string message = $"{user.Name}: {user.Poster.Message}";
            string expected = $"Natt: Hello! today is my birthday!";
            Assert.AreEqual(expected, message);
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
