using MyBlog.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Data.Abscract
{
    public interface IPostRepository:IGenericRepository<Post>
    {
        Post GetBySlug(string slug);
    }
}
