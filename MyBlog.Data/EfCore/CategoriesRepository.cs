using MyBlog.Data.Abscract;
using MyBlog.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Data.EfCore
{
    public class CategoriesRepository:GenericRepository<Categories>, ICategoriesRepository
    {
        public CategoriesRepository(BlogContext context):base(context)
        {

        }
    }
}
