using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetC.JuniorDeveloperExam.Web.Models
{
    public class BlogPost
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string htmlContent { get; set; }
        public List<Comment> comments = new List<Comment>();
    }

    public class Comment
    {
        [Required(ErrorMessage = "Please enter a name.")]
        public string name { get; set; }
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "This email address is not valid.")]
        public string emailAddress { get; set; }
        [Required(ErrorMessage = "Please enter a message.")]
        public string message { get; set; }
        public List<Comment> replies = new List<Comment>();
    }
}