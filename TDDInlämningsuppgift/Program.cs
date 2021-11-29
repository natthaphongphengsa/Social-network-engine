using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using TDDInlämningsuppgift.Data;
using TDDInlämningsuppgift.Model;
using TDDInlämningsuppgift;

namespace TDDInlämningsuppgift
{
    public class Program
    {
        public static ApplicationDb database = new ApplicationDb();

        public static SocialNetworkEngine engine = new SocialNetworkEngine(database);
        public static User user { get; set; } = new User();

        public static string commandString { get; set; }
        public static string userString { get; set; }

        public static void Main(string[] args)
        {
            DataInitializer.SeedData();
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to facebook!");
                Console.WriteLine("Login");
                Console.Write($"Username: ");
                user.Username = Console.ReadLine();
                Console.Write($"Password: ");
                user.Password = Console.ReadLine();

                var result = engine.Login(user.Username, user.Password);
                if (result == ResultStatus.IsSuccess)
                {
                    var myProfile = database.Users.Include(f => f.Follower).Include(u => u.Following).Include(p => p.posters).First(u => u.Username == user.Username && u.Password == user.Password);
                    Profile(myProfile);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid!");
                    Console.WriteLine("Do you want to create a new account? (yes or no)");

                    string answer = Console.ReadLine();
                    if (answer.ToUpper() == "YES") CreateAccount(); else loop = false;
                }
            } while (loop != true);
        }
        public static void Profile(User myProfile)
        {
            //var myProfile = database.Users.Include(f => f.Following).Include(f => f.Follower).First(u => u == me);
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("COMMAND LIST: [post, timeline, follow, wall, post @username, friendlist, log_out, send_message, view_message]");
                Console.WriteLine("");
                Console.WriteLine("Welcome to facebook!");
                Console.WriteLine("My profile:");
                Console.WriteLine($"Hello {myProfile.Username}!");
                Console.WriteLine("");
                Console.Write($"> {user.Username} /");
                string alternative = Console.ReadLine();

                Command commado = new Command();
                if (alternative.IndexOf(' ') != -1)
                {
                    commandString = alternative.Remove(alternative.IndexOf(' '));
                    userString = alternative.Remove(0, commandString.Length + 1);
                    commado = (Command)Enum.Parse(typeof(Command), commandString);
                }
                else
                {
                    commado = (Command)Enum.Parse(typeof(Command), alternative);
                }

                switch (commado)
                {
                    case Command.timeline:
                        Console.Clear();
                        Timeline(userString);
                        Console.ReadKey();
                        break;
                    case Command.follow:
                        Follow(myProfile, userString);
                        Console.ReadKey();
                        break;
                    case Command.post:
                        Post(myProfile);
                        Console.ReadKey();
                        break;
                    case Command.wall:
                        Wall(myProfile);
                        Console.ReadKey();
                        break;
                    case Command.log_out:
                        loop = true;
                        break;
                    case Command.send_message:
                        break;
                    default:
                        loop = false;
                        break;
                }
            } while (loop != true);
        }
        public static void Post(User user)
        {
            Post post = new Post();
            post.Message = userString;
            post.PostedBy = user;
            post.Datum = DateTime.Now;
            bool result = engine.Post(post);
            if (result == true)
            {
                Console.WriteLine("Successfully create a new post.");
            }
            else
            {
                Console.WriteLine("Failed post");
            }
        }
        public static void Timeline(string username)
        {
            var userTimelineList = engine.GetTimeline(username);
            var userProfile = database.Users.Include(u => u.Follower).Include(u => u.Following).First(u => u.Username == username);

            //if (userProfile.Following.Count() == 0)
            //{
            //    Console.WriteLine($"{userProfile.Username} did not follow anyone");
            //}
            //else
            //{
            //    Console.WriteLine($"{userProfile.Username} is following:");
            //    foreach (var user in user.Following)
            //    {
            //        Console.WriteLine($"{user.Username}");
            //    }
            //}

            Console.WriteLine("");

            if (userTimelineList.Count == 0)
            {
                Console.WriteLine($"{userProfile.Username} did not post anything.");
            }
            else
            {
                Console.WriteLine($"{userProfile.Username}s Timeline:");
                foreach (var post in userTimelineList)
                {
                    Console.WriteLine($"{post.Datum.ToString("f")} ({post.PostedBy.Username}): {post.Message}");
                }
            }
            Console.ReadKey();
        }
        public static void CreateAccount()
        {
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Create new account");
                Console.Write("Username:");
                user.Username = Console.ReadLine();
                Console.Write("Password:");
                user.Password = Console.ReadLine();

                if (user.Username != "" && user.Password != "")
                {
                    var result = engine.CreateNewAccount(user);
                    if (result == ResultStatus.IsSuccess)
                    {
                        var myProfile = database.Users.First(u => u.Username == user.Username && u.Password == user.Password);
                        Profile(myProfile);
                    }
                    else
                    {
                        Console.WriteLine("This username already exit! try again!");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Please enter all infromations!");
                    Console.ReadKey();
                    loop = false;
                }

            } while (loop != true);
        }
        public static void Follow(User me, string username)
        {
            var myFollowingList = engine.StartFollow(me, username);
            if (myFollowingList != null)
            {
                Console.Clear();
                Console.WriteLine("Successfully following!");
                Console.WriteLine("");
                foreach (var anotherUser in myFollowingList)
                {
                    Console.WriteLine($"{anotherUser.Username} is following");
                    foreach (var thirdUser in anotherUser.Following)
                    {
                        Console.WriteLine($"{thirdUser.Username}");
                    }
                }
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("You did not write the right name Or you already follow this user!");
                Console.ReadKey();
            }

        }
        public static void Chats(User me)
        {
            var user = database.Users.Include(f => f.Following).Include(f => f.Follower).First(u => u == me);

            bool loop = true;
            do
            {
                if (database.Chats.Any(u => u.SendTo == me || u.SendFromId == me.Id))
                {
                    Console.Clear();
                    Console.WriteLine("Chats");
                    Console.WriteLine("Chose your chat by enter the name");
                    var chatList = database.Chats.Include(u => u.SendTo).Where(u => u.SendTo == me || u.SendFromId == me.Id).ToList();
                    chatList.OrderBy(c => c.Date);

                    foreach (var chat in chatList)
                    {
                        if (chat.SendTo == me || chat.SendFromId != me.Id)
                        {
                            if (chat.SendFromId != me.Id && chat.SendTo == me)
                            {
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Username})");
                            }
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Username})");
                        }
                        else if (chat.SendFromId == me.Id)
                        {
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u == chat.SendTo).Username})");
                        }
                    }

                    Console.Write($"> {user.Username} ");
                    string selectedChat = Console.ReadLine();
                    if (selectedChat != "")
                    {
                        ChatNow(me, selectedChat);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Press ESC to go back");
                    Console.WriteLine("Empty!");
                }
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    loop = false;
                }
            } while (loop != false);
        }
        public static void ChatNow(User me, string username)
        {
            var userBehavior = new SocialNetworkEngine(database);
            var anotherUser = database.Users.First(u => u.Username == username);
            List<Chat> chatList = new List<Chat>();

            bool loop = true;
            do
            {
                if (database.Chats.Any(c => c.SendTo == me || c.SendFromId == me.Id) || database.Chats.Any(c => c.SendFromId != me.Id))
                {
                    Console.Clear();

                    var anotherUserMessage = database.Chats.Include(c => c.SendTo).Where(u => u.SendFromId == anotherUser.Id || u.SendTo == me).ToList();
                    //anotherUserMessage.OrderBy(c => c.Date);
                    var myMessage = database.Chats.Include(c => c.SendTo).Where(u => u.SendFromId == me.Id && u.SendTo == anotherUser);

                    foreach (var anotherUserchats in anotherUserMessage)
                    {
                        chatList.Add(anotherUserchats);
                    }
                    foreach (var mychat in myMessage)
                    {
                        chatList.Add(mychat);
                    }
                    chatList.OrderBy(c => c.Date);

                    foreach (var chat in chatList)
                    {
                        if (chat.SendFromId == me.Id)
                        {
                            if (chat.SendFromId == me.Id)
                            {
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({chat.SendTo.Username}) {chat.Text}");
                            }
                            else if (chat.SendTo != me && chat.SendFromId == me.Id)
                            {
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Username}) {chat.Text}");
                            }
                        }
                        else if (chat.SendTo.Id == me.Id && chat.SendFromId != me.Id)
                        {
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Username}) {chat.Text}");
                        }
                    }
                    Console.Write($">{me.Username}/ ");
                    string message = Console.ReadLine();
                    if (message != "")
                    {
                        userBehavior.SendMessage(anotherUser, me, message);
                    }
                    else
                        Console.WriteLine("No message!");
                    loop = true;
                }
                else
                {
                    Console.WriteLine("Empty!");
                }
                Console.WriteLine("Press ESC to go back");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    loop = false;
                }
            } while (loop != false);
        }
        public static void Wall(User me)
        {
            var myself = database.Users.Include(f => f.Follower).Include(f => f.Following).Where(u => u.Username == me.Username).ToList();
            foreach (var FollowingUserList in myself)
            {
                if (FollowingUserList.Following != null)
                {
                    Console.WriteLine($"My following user list:");
                    foreach (var firstStepUser in FollowingUserList.Following.ToList())
                    {
                        var secoundStepUser = database.Users.Include(f => f.Follower).Include(f => f.Following).Where(u => u.Username == firstStepUser.Username).ToList();
                        Console.WriteLine($">{firstStepUser.Username} following list");
                        foreach (var thirdStepUser in secoundStepUser)
                        {
                            if (thirdStepUser.Following != null)
                            {
                                foreach (var secoundUser in thirdStepUser.Following)
                                {
                                    Console.WriteLine($">>{secoundUser.Username}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($">>{thirdStepUser.Username} did not follow anyone");
                            }
                        }
                    }
                    //Console.WriteLine($"{users.Username} Follower: {users.Follower.Count} Following: {users.Following.Count()}");
                }
                else
                {
                    //Console.WriteLine($"{users.Username} Follower: {users.Follower.Count()} Following: {users.Following.Count()}");
                }
            }
        }
    }
}
