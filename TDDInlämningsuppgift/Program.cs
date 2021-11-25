using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using TDDInlämningsuppgift.Data;
using TDDInlämningsuppgift.Model;

namespace TDDInlämningsuppgift
{
    public class Program
    {
        static ApplicationDb database => new ApplicationDb();
        static void Main(string[] args)
        {
            DataInitializer.SeedData();
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to facebook!");
                Console.WriteLine("1: Login");
                Console.WriteLine("2: Create Account");
                Console.WriteLine("3: Exit");
                string altenative = Console.ReadLine();
                switch (altenative)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        CreateAccount();
                        break;
                    case "3":
                        Console.WriteLine("Press any key to exit!");
                        Console.ReadKey();
                        loop = true;
                        break;
                    default:
                        loop = false;
                        break;
                }
            } while (loop != true);

        }
        static void CreateAccount()
        {
            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Username:");
                string username = Console.ReadLine();
                Console.WriteLine("Your name:");
                string name = Console.ReadLine();
                Console.WriteLine("Password:");
                string password = Console.ReadLine();

                if (name != null && username != null && password != null)
                {
                    var user = new User();
                    user.Name = name;
                    user.Username = username;
                    user.Password = password;

                    if (database.Users.Any(u => u.Username == user.Username) == false)
                    {
                        database.Users.Add(user);
                        database.SaveChanges();
                        loop = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Username already exit!");
                        Console.WriteLine("Try again with another username.");
                        Console.ReadKey();
                        loop = false;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Please fill in all the information!");
                    Console.ReadKey();
                    loop = false;
                }
            } while (loop != true);
        }
        static void Login()
        {
            bool loginStatus = false;
            do
            {
                Console.Clear();
                Console.WriteLine("LOGIN TO FACEBOOK");
                Console.WriteLine($"Username: ");
                string username = Console.ReadLine();
                Console.WriteLine($"Password: ");
                string password = Console.ReadLine();

                if (database.Users.Any(u => u.Username == username && u.Password == password))
                {
                    var user = database.Users.First(u => u.Username == username && u.Password == password);
                    Profile(user);
                }
                else if (!database.Users.Any(u => u.Username == username && u.Password == password))
                {
                    Console.WriteLine("No account was found!");
                    Console.WriteLine("Press ESC to go back to menu or press any key to try again");
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        loginStatus = true;
                    }
                    else
                        loginStatus = false;
                }
                else
                {
                    Console.WriteLine("Invalid username or password!");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    loginStatus = false;
                }
            } while (loginStatus != true);
        }
        static void Profile(User me)
        {
            var user = database.Users.Include(f => f.Following).Include(f => f.Follower).First(u => u == me);
            var posters = database.Posters.Where(u => u.PostedBy == user).ToList();

            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Fakebook!");
                Console.WriteLine("My profile:");
                Console.WriteLine($"Hello {user.Name}!");
                Console.WriteLine($"My poster {posters.Count}");

                Console.WriteLine("");
                Console.WriteLine("Timeline");
                Console.WriteLine("Friends");
                Console.WriteLine("Follower");
                Console.WriteLine("Following");
                Console.WriteLine("Create new post");
                Console.WriteLine("My Chats");
                Console.WriteLine("Start chat");
                Console.WriteLine("Log out");

                Console.Write($">{user.Username} ");
                string alternative = Console.ReadLine();

                string commandString = alternative.Remove(alternative.IndexOf(' '));
                string User = alternative.Remove(0, commandString.Length + 1);
                command command = (command)Enum.Parse(typeof(command), commandString);
                Console.WriteLine(alternative);

                //command command = (command)Enum.Parse(typeof(command), alternative);

                Console.Clear();
                switch (command)
                {
                    case command.timeline:

                        Console.WriteLine("TimeLine");
                        break;
                    case command.follow:
                        Console.WriteLine("1 Follow someone");
                        Console.WriteLine("2 Unfollow someone");
                        Console.WriteLine("");

                        FriendsList();

                        Console.Write($">{user.Username} ");

                        var selectedOption = Console.ReadKey();
                        if (selectedOption.Key == ConsoleKey.NumPad1)
                        {
                            var userbehavior = new UserBehavior(database);
                            Console.WriteLine("");

                            Console.WriteLine("Who do you want to follow?");

                            Console.Write($">{user.Username} ");
                            string anotherUser = Console.ReadLine();

                            bool result = userbehavior.StartFollow(user, anotherUser);
                            if (result != true)
                            {
                                Console.WriteLine("You did not write the right name Or you already follow this user!");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("Successfully following!");
                                Console.ReadKey();
                            }
                        }
                        else if (selectedOption.Key == ConsoleKey.NumPad2)
                        {

                        }

                        break;
                    case command.followers:
                        Console.WriteLine("My follower");
                        foreach (var myself in MyFollowList(user, 1))
                        {
                            if (myself.Follower.Count != 0)
                            {
                                foreach (var anotherUser in myself.Follower)
                                {
                                    Console.WriteLine($"{anotherUser.Username}");
                                }
                            }
                            else
                                Console.WriteLine("No follower!");
                        }
                        Console.ReadKey();
                        break;
                    case command.post:
                        CreatePost(user);
                        break;
                    case command.chats:
                        Chats(user);
                        loop = false;
                        break;
                    case command.friends:
                        var userBehavior = new UserBehavior(database);
                        FriendsList();
                        Console.WriteLine("");
                        Console.WriteLine("To:");
                        Console.Write($"{user.Username} > ");

                        string to = Console.ReadLine();

                        Console.WriteLine("Text:");
                        Console.Write($"> {user.Username} ");

                        string message = Console.ReadLine();

                        var anotheruser = database.Users.First(u => u.Username == to);
                        userBehavior.SendMessage(anotheruser, user, message);
                        loop = true;
                        break;
                    default:
                        loop = false;
                        break;
                }


                //switch (alternative)
                //{
                //    case "1":
                //        Console.WriteLine("TimeLine");
                //        TimeLine();
                //        break;
                //    case "2":
                //        Console.WriteLine("Get a new friend by follow someone in this list");
                //        Console.WriteLine("Chose your options!");
                //        Console.WriteLine("1 Follow someone");
                //        Console.WriteLine("2 Unfollow someone");
                //        Console.WriteLine("3 Go back");
                //        Console.WriteLine("");

                //        FriendsList();

                //        Console.Write($">{user.Username} ");

                //        var selectedOption = Console.ReadKey();
                //        if (selectedOption.Key == ConsoleKey.NumPad1)
                //        {
                //            var userbehavior = new UserBehavior(database);
                //            Console.WriteLine("");

                //            Console.WriteLine("Who do you want to follow?");

                //            Console.Write($">{user.Username} ");
                //            string anotherUser = Console.ReadLine();

                //            bool result = userbehavior.StartFollow(user, anotherUser);
                //            if (result != true)
                //            {
                //                Console.WriteLine("You did not write the right name Or you already follow this user!");
                //                Console.ReadKey();
                //            }
                //            else
                //            {
                //                Console.WriteLine("Successfully following!");
                //                Console.ReadKey();
                //            }
                //        }
                //        else if (selectedOption.Key == ConsoleKey.NumPad2)
                //        {

                //        }

                //        break;
                //    case "3":
                //        Console.WriteLine("My follower");
                //        foreach (var myself in MyFollowList(user, 1))
                //        {
                //            if (myself.Follower.Count != 0)
                //            {
                //                foreach (var anotherUser in myself.Follower)
                //                {
                //                    Console.WriteLine($"{anotherUser.Username}");
                //                }
                //            }
                //            else
                //                Console.WriteLine("No follower!");
                //        }
                //        Console.ReadKey();
                //        break;
                //    case "4":
                //        break;
                //    case "5":
                //        CreatePost(user);
                //        break;
                //    case "6":
                //        Chats(user);
                //        loop = false;
                //        break;
                //    case "7":
                //        var userBehavior = new UserBehavior(database);
                //        FriendsList();
                //        Console.WriteLine("");
                //        Console.WriteLine("To:");
                //        Console.Write($"{user.Username} > ");

                //        string to = Console.ReadLine();

                //        Console.WriteLine("Text:");
                //        Console.Write($"> {user.Username} ");

                //        string message = Console.ReadLine();

                //        var anotheruser = database.Users.First(u => u.Username == to);
                //        userBehavior.SendMessage(anotheruser, user, message);
                //        break;
                //    case "8":
                //        loop = true;
                //        break;
                //    default:
                //        loop = false;
                //        break;
                //}
            } while (loop != true);
        }
        static void Chats(User me)
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
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name})");
                            }
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name})");
                        }
                        else if (chat.SendFromId == me.Id)
                        {
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u == chat.SendTo).Name})");
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
        static void ChatNow(User me, string name)
        {
            var userBehavior = new UserBehavior(database);
            var anotherUser = database.Users.First(u => u.Name == name);
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
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({chat.SendTo.Name}) {chat.Text}");
                            }
                            else if (chat.SendTo != me && chat.SendFromId == me.Id)
                            {
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name}) {chat.Text}");
                            }
                        }
                        else if (chat.SendTo.Id == me.Id && chat.SendFromId != me.Id)
                        {
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name}) {chat.Text}");
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
        static void FriendsList()
        {
            foreach (var users in database.Users.Include(f => f.Follower).Include(f => f.Following).ToList())
            {
                if (users.Following != null || users.Follower != null)
                {
                    Console.WriteLine($"{users.Username} Follower: {users.Follower.Count()} Following: {users.Following.Count()}");
                }
                else
                {
                    Console.WriteLine($"{users.Username} Follower: {users.Follower.Count()} Following: {users.Following.Count()}");
                }
            }
        }
        static List<User> MyFollowList(User user, int options)
        {
            if (options == 1)
            {
                return database.Users.Include(f => f.Follower).Where(u => u == user).ToList();
            }
            else
                return database.Users.Include(f => f.Following).Where(u => u == user).ToList();
        }
        //static void TimeLine()
        //{
        //    var userBehavior = new UserBehavior(database);
        //    //var postList = userBehavior.GetTimeLine();

        //    foreach (var post in postList)
        //    {
        //        Console.WriteLine($"Posted by: {post.PostedBy.Name} {post.Datum.ToString("d")}");
        //        Console.WriteLine($"{post.Datum.ToString("t")}: {post.Message}");
        //        Console.WriteLine("");
        //    }
        //    Console.WriteLine("Press any key to go back......");
        //    Console.ReadKey();
        //}
        static void CreatePost(User user)
        {
            var post = new Post();

            bool loop = false;
            do
            {
                Console.WriteLine("Write your post:");
                post.Message = Console.ReadLine();
                post.PostedBy = database.Users.First(u => u.Username == user.Username);
                post.Datum = DateTime.Now;

                var userBehavior = new UserBehavior(database);
                var result = userBehavior.Post(post);

                if (result == true)
                {
                    Console.Clear();
                    Console.WriteLine("Successfully added new post");
                    Console.WriteLine("Press any key to go back to profile");
                    Console.ReadKey();
                    loop = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("This post already exist! Try again");
                    Console.ReadKey();
                    loop = false;
                }

            } while (loop != true);

        }
    }
}
