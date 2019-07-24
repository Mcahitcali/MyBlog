using MyBlog.Data.Abscract;
using MyBlog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Data.EfCore
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(BlogContext context):base(context)
        {
           
        }
       
        public Post GetBySlug(string slug)
        {
            return _context.Posts.FirstOrDefault(q => q.Slug == slug);
        }
    }
}
