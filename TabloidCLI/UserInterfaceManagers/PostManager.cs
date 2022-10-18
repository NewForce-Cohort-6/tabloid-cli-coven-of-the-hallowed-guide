using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        //return new PostDetailManager(this, _connectionString, post.Id);
                        throw new NotImplementedException();
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                    throw new NotImplementedException();
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine(post);
            }
        }

        private Post Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Post:";
            }

            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.WriteLine("Title: ");
            post.Title = Console.ReadLine();

            Console.WriteLine("URL: ");
            post.Url = Console.ReadLine();

            while (post.Author == null)
            {
                post.Author = ChooseAuthor("Please choose post's Author: ");
            }

            while (post.Blog == null)
            {
                post.Blog = ChooseBlog("Please choose post's Blog: ");
            }

            post.PublishDateTime = DateTime.Now;

            _postRepository.Insert(post);
        }

        private void Edit()
        {
            Post post = Choose("Which post would you like to edit?");
            if (post == null)
            {
                return;
            }

            Console.Write("\nNew title (blank to leave unchanged): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                post.Title = newTitle;
            }

            Console.Write("\nNew URL (blank to leave unchanged): ");
            string newUrl = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUrl))
            {
                post.Url = newUrl;
            }

            Console.Write("\nNew publication date now (Enter 'NOW' to change, blank to leave unchanged): ");
            string isNewDateTime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(isNewDateTime))
            {
                post.PublishDateTime = DateTime.Now;
            }

            Author newAuthor = ChooseAuthor("New author (blank to leave unchanged): ");
            if(newAuthor != null)
            {
                post.Author = newAuthor;
            }

            Blog newBlog = ChooseBlog("New blog (blank to leave unchaged): ");
            if (newBlog != null)
            {
                post.Blog = newBlog;
            }

            _postRepository.Update(post);
        }

        private void Remove()
        {
            Post post = Choose("Which post would you like to remove?");

            if (post != null)
            {
                _postRepository.Delete(post.Id);
            }
        }

        private Author ChooseAuthor(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Author> authors = _authorRepository.GetAll();

            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($" {i + 1}) {author.FullName}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return authors[choice - 1];
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private Blog ChooseBlog(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Blog:";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i =0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Invalid Selection");
                return null;
            }
        }
    }
}
