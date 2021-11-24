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

                    var database = new ApplicationDb();
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

                var database = new ApplicationDb();

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
            var database = new ApplicationDb();
            List<Post> postList = new List<Post>();

            var user = database.Users.Include(f => f.Following).Include(f => f.Follower).First(u => u == me);

            if (database.Posters.Any(u => u.PostedBy == user) == true)
            {
                var posters = database.Posters.FirstOrDefault(u => u.PostedBy == user);
                postList.Add(posters);
            }

            bool loop = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Fakebook!");
                Console.WriteLine("My profile:");
                Console.WriteLine($"Hello {user.Name}!");
                Console.WriteLine($"My poster {postList.Count}");

                Console.WriteLine("");
                Console.WriteLine("1: Timeline");
                Console.WriteLine("2: Friends");
                Console.WriteLine("3: Follower");
                Console.WriteLine("4: Following");
                Console.WriteLine("5: Create new post");
                Console.WriteLine("6: Chats");
                Console.WriteLine("7: Send message");
                Console.WriteLine("8: Log out");

                string alternative = Console.ReadLine();
                switch (alternative)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("TimeLine");
                        TimeLine();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Get a new friend by follow someone in this list");
                        Console.WriteLine("Chose your options!");
                        Console.WriteLine("1 Follow someone");
                        Console.WriteLine("2 Unfollow someone");
                        Console.WriteLine("3 Go back");
                        Console.WriteLine("");
                        FriendsList(user);
                        var selectedOption = Console.ReadKey();
                        if (selectedOption.Key == ConsoleKey.NumPad1)
                        {
                            var userbehavior = new UserBehavior(database);
                            Console.WriteLine("");

                            Console.WriteLine("Who do you want to follow?");
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
                    case "3":
                        Console.Clear();
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
                        break;
                    case "4":
                        break;
                    case "5":
                        Console.Clear();
                        CreatePost(user);
                        break;
                    case "6":
                        Console.Clear();
                        Chats(user);
                        loop = false;
                        break;
                    case "7":
                        Console.Clear();
                        var userBehavior = new UserBehavior(database);
                        FriendsList(user);
                        Console.WriteLine("");
                        Console.WriteLine("To:");
                        string to = Console.ReadLine();
                        Console.WriteLine("Text:");
                        string message = Console.ReadLine();

                        var friend = database.Users.First(u => u.Username == to);
                        userBehavior.SendMessage(friend, user, message);
                        break;
                    case "8":
                        Console.Clear();
                        loop = true;
                        break;
                    default:
                        loop = false;
                        break;
                }
            } while (loop != true);
        }
        static void Chats(User me)
        {
            var database = new ApplicationDb();
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
            var database = new ApplicationDb();
            var userBehavior = new UserBehavior(database);
            var anotherUser = database.Users.First(u => u.Name == name);

            bool loop = true;
            do
            {
                if (database.Chats.Any(u => u.SendTo == anotherUser || u.SendFromId == me.Id))
                {
                    Console.Clear();
                    var chatList = database.Chats.Include(c => c.SendTo).Where(u => u.SendFromId == me.Id && u.SendTo == anotherUser).ToList();
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
                                Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name}){chat.Text}");
                            }
                        }
                        else if (chat.SendTo == me && chat.SendFromId != me.Id)
                        {
                            Console.WriteLine($"{chat.Date.ToString("g")} AM : ({database.Users.First(u => u.Id == chat.SendFromId).Name}){chat.Text}");
                        }
                    }
                    string message = Console.ReadLine();
                    if (message != "")
                    {
                        userBehavior.SendMessage(anotherUser, me, message);
                    }
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
        static void FriendsList(User user)
        {
            var database = new ApplicationDb();

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
            var database = new ApplicationDb();
            if (options == 1)
            {
                return database.Users.Include(f => f.Follower).Where(u => u == user).ToList();
            }
            else
                return database.Users.Include(f => f.Following).Where(u => u == user).ToList();
        }
        static void TimeLine()
        {
            var database = new ApplicationDb();
            var postList = database.Posters.Include(u => u.PostedBy).ToList();
            foreach (var post in postList)
            {
                Console.WriteLine($"Posted by: {post.PostedBy.Name} {post.Datum.ToString("d")}");
                Console.WriteLine($"{post.Datum.ToString("t")}: {post.Message}");
                Console.WriteLine("");
            }
            Console.WriteLine("Press any key to go back......");
            var key = Console.ReadKey();
        }
        static void CreatePost(User user)
        {
            bool loop = false;
            do
            {
                var database = new ApplicationDb();
                var post = new Post();

                Console.WriteLine("Write your post here:");
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
