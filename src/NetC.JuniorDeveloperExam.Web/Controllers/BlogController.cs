using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace NetC.JuniorDeveloperExam.Web.Controllers
{
    public class BlogController : Controller
    {
        public static Models.BlogPostCollection GetBlogPostCollection(HttpServerUtilityBase httpServer)
        {
            Models.BlogPostCollection blogPostCollection;
            using (StreamReader sr = new StreamReader(httpServer.MapPath("~/App_Data/Blog-Posts.json")))
            {
                blogPostCollection = JsonConvert.DeserializeObject<Models.BlogPostCollection>(sr.ReadToEnd());
            }
            return blogPostCollection;
        }

        [Route("blog/{id?}")]
        [Route("blog")]
        public ActionResult Index(int id = 1)
        {
            Models.BlogPostCollection blogPosts = GetBlogPostCollection(Server);
            IEnumerable<Models.BlogPost> selectedBlogPost = blogPosts.blogPosts.Where(post => post.id == id);
            if(selectedBlogPost.Any())
            {
                // We've found the blog post //
                return View(selectedBlogPost.First());
            }

            return View();
        }

        [HttpPost]
        [Route("blog/addcomment")]
        public ActionResult AddComment(string Name, string EmailAddress, string Message, int ReplyingTo, int id)
        {
            // Get all the comments //
            Models.BlogPostCollection blogPosts = GetBlogPostCollection(Server);
            Models.BlogPost selectedBlogPost = blogPosts.blogPosts.Where(post => post.id == id).First();
            int postPosition = blogPosts.blogPosts.IndexOf(selectedBlogPost); // Remember the position of this object //


            // Make a new comment //
            Models.Comment comment = new Models.Comment() { name = Name, date = DateTime.Now, emailAddress = EmailAddress, message = Message };

            if (ReplyingTo >= 0)
            {
                // We are replying to a comment //
                selectedBlogPost.comments[ReplyingTo].replies.Add(comment);
            }
            else
            {
                selectedBlogPost.comments.Add(comment);
            }

            // Replace this updated object in the collection //
            blogPosts.blogPosts[postPosition] = selectedBlogPost;

            // Convert it all back to JSON //
            var resultingJSON = JsonConvert.SerializeObject(blogPosts);

            // Overwrite the original contents of the file with the new data //
            using (StreamWriter sw = new StreamWriter(Server.MapPath("~/App_Data/Blog-Posts.json")))
            {
                sw.Write(resultingJSON);
            }

            // Redirect back to the same page, showing the comment //
            return RedirectToAction(id.ToString());
        }
    }
}