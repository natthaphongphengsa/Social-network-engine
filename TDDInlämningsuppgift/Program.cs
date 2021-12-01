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

        public static SocialNetworkEngine socialEngine = new SocialNetworkEngine(database);
        public static User user { get; set; } = new User();
        public static command commado = new command();
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

                var result = socialEngine.Login(user.Username, user.Password);
                if (result == resultStatus.IsSuccess)
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
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("COMMAND LIST: [post, post @username, timeline, follow, wall, log_out, send_message, view_message]");
                Console.WriteLine("");
                Console.WriteLine("Welcome to facebook!");
                Console.WriteLine("My profile:");
                Console.WriteLine($"Hello {myProfile.Username}!");
                Console.WriteLine("");
                Console.Write($"> {myProfile.Username} /");
                string alternative = Console.ReadLine();

                if (alternative.IndexOf(' ') != -1)
                {
                    commandString = alternative.Remove(alternative.IndexOf(' '));
                    userString = alternative.Remove(0, commandString.Length + 1);
                    try
                    {
                        commado = (command)Enum.Parse(typeof(command), commandString);
                    }
                    catch
                    {
                        Console.WriteLine("Could not found command and user");
                        Console.ReadKey();
                        break;
                    }
                }
                else
                {
                    try
                    {
                        commado = (command)Enum.Parse(typeof(command), alternative);
                    }
                    catch
                    {
                        Console.WriteLine("Command could not found!");
                        Console.ReadKey();
                        break;
                    }
                }

                switch (commado)
                {
                    case command.timeline:
                        Console.Clear();
                        Timeline(userString);
                        Console.ReadKey();
                        break;
                    case command.follow:
                        Follow(myProfile, userString);
                        Console.ReadKey();
                        break;
                    case command.post:
                        Post(myProfile);
                        Console.ReadKey();
                        break;
                    case command.wall:
                        Wall(myProfile);
                        Console.ReadKey();
                        break;
                    case command.log_out:
                        loop = true;
                        break;
                    case command.send_message:
                        SendMessage(myProfile.Username, userString);
                        Console.ReadKey();
                        break;
                    case command.view_messages:
                        ViewMessage(myProfile);
                        Console.ReadKey();
                        break;
                    default:
                        loop = false;
                        break;
                }
            } while (loop != true);
        }
        public static void Post(User user)
        {
            //Post post = new Post();
            //post.Message = userString;
            //post.PostedBy = user;
            //post.Datum = DateTime.Now;

            Post result = socialEngine.Post(user.Username, userString);
            if (result != null)
            {
                database.Posters.Add(result);
                database.SaveChanges();
                Console.WriteLine("Successfully create a new post.");
            }
            else
            {
                Console.WriteLine("Failed post or could not find the user you want to link to!");
            }
        }
        public static void Timeline(string username)
        {
            var userTimelineList = socialEngine.GetTimeline(username);
            string anotherUsername = null;
            if (userTimelineList.Count() != 0)
            {
                anotherUsername = database.Users.Include(u => u.Follower).Include(u => u.Following).First(u => u.Username == username).Username;
            }

            if (userTimelineList.Count() == 0)
            {
                if (user.Username == username)
                {
                    Console.WriteLine($"You did not post anything.");
                }
                else if (anotherUsername == null)
                {
                    Console.WriteLine("Could not found this user OR You did not follow this user!");
                }
                else
                {
                    Console.WriteLine($"{anotherUsername} did not post anything.");
                }
            }
            else
            {
                Console.WriteLine($"{anotherUsername}s Timeline:");
                foreach (var post in userTimelineList)
                {
                    Console.WriteLine($"{post.Datum.ToString("f")} ({post.PostedBy.Username}): {post.Message}");
                }
            }
        }
        public static void CreateAccount()
        {
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Create new account");
                Console.Write("Username:");
                string username = Console.ReadLine();
                Console.Write("Password:");
                string password = Console.ReadLine();

                if (username != "" && password != "")
                {
                    var result = socialEngine.CreateNewAccount(username, password);
                    if (!database.Users.Any(u => u.Username == result.Username))
                    {
                        database.Users.Add(result);
                        database.SaveChanges();
                        var myProfile = database.Users.First(u => u.Username == result.Username && u.Password == result.Password);
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
            var myFollowingList = socialEngine.StartFollow(me.Username, username);
            if (myFollowingList != null)
            {
                database.Users.Update(myFollowingList);
                database.SaveChanges();
                Console.Clear();
                Console.WriteLine($"Successfully following {username}!");
                Console.WriteLine($"--- {username} following list ---");
                Console.WriteLine("");
                var users = database.Users.Include(f => f.Following).Include(f => f.Follower).FirstOrDefault(u => u.Username == username);
                if (users.Following != null)
                {
                    foreach (var anotherUser in users.Following.ToList())
                    {
                        if (anotherUser != null)
                        {
                            Console.WriteLine($"{anotherUser.Username} is following: ");
                            foreach (var thirdUser in database.Users.Include(f => f.Following).First(u => u.Username == anotherUser.Username).Following)
                            {
                                Console.WriteLine($"-{thirdUser.Username}");
                            }
                            Console.WriteLine("");
                        }
                        else
                        {
                            Console.WriteLine($"{anotherUser.Username} did not follow anyone yet!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{username} did not follow anyone yet!");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You did not write the right name Or you already follow this user!");
                if (database.Users.Any(u => u.Username == username))
                {
                    foreach (var user in database.Users.Include(f => f.Following).Where(u => u.Username == username))
                    {
                        Console.WriteLine($"{user.Username} is following:");
                        foreach (var followingList in user.Following)
                        {
                            Console.WriteLine($"-{followingList.Username}");
                        }
                    }
                }
            }

        }
        public static void SendMessage(string Sendfrom, string sendto)
        {
            Console.Clear();
            Console.Write("Your message: ");
            string text = Console.ReadLine();

            var resultChat = socialEngine.SendMessage(sendto, Sendfrom, text);
            if (resultChat != null)
            {
                database.Chats.Add(resultChat);
                database.SaveChanges();
                Console.WriteLine("Successfully send message");
            }
            else if (resultChat == null)
            {
                Console.WriteLine("Failed to send the message");
            }
        }
        public static void ViewMessage(User me)
        {
            if (database.Chats.Any(u => u.SendTo.Username == me.Username) || database.Chats.Any(u => u.SendFromId == me.Id))
            {
                Console.Clear();
                Console.WriteLine("Messenger");
                var chatList = database.Chats.Include(u => u.SendTo).Where(u => u.SendTo == me || u.SendFromId == me.Id).ToList();
                chatList.OrderBy(c => c.Date);

                foreach (var chat in chatList)
                {
                    if (chat.SendFromId != me.Id)
                    {
                        Console.WriteLine(
                            $"{chat.Date.ToString("g")} " +
                            $"AM : Message from ({database.Users.First(u => u.Id == chat.SendFromId).Username})! " +
                            $"Message: {chat.Text}");
                    }
                    else if (chat.SendTo == me)
                    {
                        Console.WriteLine(
                            $"{chat.Date.ToString("g")} " +
                            $"AM : Message from ({database.Users.First(u => u.Id == chat.SendFromId).Username})! " +
                            $"Message: {chat.Text}");
                    }
                    else if (chat.SendFromId == me.Id)
                    {
                        Console.WriteLine(
                            $"{chat.Date.ToString("g")} " +
                            $"AM : Message to ({database.Users.First(u => u == chat.SendTo).Username})! " +
                            $"Message: {chat.Text}");
                    }

                    //if (chat.SendTo == me)
                    //{
                    //    if (chat.SendFromId != me.Id && chat.SendTo == me)
                    //    {
                    //        var user = database.Users.Include(c => c.Chats).First(u => u.Id == chat.SendFromId);
                    //        Console.WriteLine($"{chat.Date.ToString("g")} AM : Message from ({user.Username})! Message: {database.Chats.First(u => u.SendFromId == chat.SendFromId).Text}");
                    //    }
                    //    else
                    //    {
                    //        var user = database.Users.Include(c => c.Chats).First(u => u.Id == chat.SendFromId);
                    //        Console.WriteLine($"{chat.Date.ToString("g")} AM : Message from ({user.Username})! Message: {database.Chats.First(u => u.SendFromId == chat.SendFromId).Text}");
                    //    }
                    //}
                    //else if (chat.SendFromId == me.Id)
                    //{
                    //    var user = database.Users.Include(c => c.Chats).First(u => u == chat.SendTo);
                    //    Console.WriteLine($"{chat.Date.ToString("g")} AM : Message to ({user.Username})! Message: {database.Chats.First(u => u.SendTo == chat.SendTo).Text}");
                    //}
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No messages!");
            }
        }
        public static void Wall(User me)
        {
            Console.Clear();
            if (me.Following.Count != 0)
            {
                var userList = me.Following.ToList();
                foreach (var user in userList)
                {
                    Console.WriteLine($"I am following ({user.Username})");

                    var innerUserList = socialEngine.ViewUserWall(user.Username);
                    if (innerUserList.Count != 0)
                    {
                        Console.Write($"{user.Username} is following: ");
                        foreach (var anotherUser in innerUserList)
                        {
                            Console.Write($"{anotherUser.Username}, ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{user.Username} did not follow anyone yet!");
                    }
                    Console.WriteLine(" ");
                    for (int i = 0; i < 50; i++)
                    {
                        Console.Write("_");
                    }
                    Console.WriteLine(" ");
                }
            }
            else
            {
                Console.WriteLine("Sorry no following list to show!");
            }
        }
    }
}
