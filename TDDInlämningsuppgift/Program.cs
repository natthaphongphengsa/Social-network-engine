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
            bool loop = true;
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
                        loop = false;
                        break;
                    default:
                        loop = true;
                        break;
                }
            } while (loop != false);

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
                    Console.WriteLine("Invalid!");
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
        static void Profile(User user)
        {
            var database = new ApplicationDb();
            List<Post> postList = new List<Post>();
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
                Console.WriteLine("6: Delete post");
                Console.WriteLine("7: Log out");

                string alternative = Console.ReadLine();
                switch (alternative)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("TimeLine");
                        TimeLine(user);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("friends list in sweden");
                        FriendsList();
                        Console.ReadKey();
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        Console.Clear();
                        CreatePostPage(user);
                        break;
                    case "6":
                        break;
                    case "7":
                        Console.Clear();
                        loop = true;
                        break;
                    default:
                        loop = false;
                        break;

                }
            } while (loop != true);
        }

        static void FriendsList()
        {
            var database = new ApplicationDb();
            int count = 1;
            int followerCount = 0;
            int followingCount = 0;
            foreach (var users in database.Users.ToList())
            {
                if (users.Following != null || users.Follower != null)
                {
                    followingCount = users.Following.Count();
                    followerCount = users.Follower.Count();
                    Console.WriteLine($"Nr {count}: {users.Name} Follower: {followerCount} Following: {followingCount.ToString()}");
                    count++;
                }
                else
                {
                    Console.WriteLine($"Nr {count}: {users.Name} Follower: {followerCount} Following: {followingCount.ToString()}");
                    count++;
                }
            }
        }

        static void TimeLine(User user)
        {
            var database = new ApplicationDb();
            var postList = database.Posters.Include(u => u.PostedBy).Where(p => p.PostedBy == user).ToList();
            foreach (var post in postList)
            {
                Console.WriteLine($"{post.Datum.ToString("g")}AM {post.PostedBy.Username}: {post.Message}");
            }
            Console.WriteLine("Press any key to go back......");
            var key = Console.ReadKey();
            //if (key.Key == ConsoleKey.Escape) {}
        }
        static void CreatePostPage(User user)
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
