using Microsoft.AspNetCore.Mvc.Rendering;
using MyBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.WebUI.Models
{
    public class NewPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }

        public int CategoryId { get; set; }
        public IList<Categories> Categories { get; set; }
    }
}
