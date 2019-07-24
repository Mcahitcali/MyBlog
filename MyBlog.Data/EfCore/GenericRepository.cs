using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abscract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Data.EfCore
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public BlogContext _context;
        public GenericRepository(BlogContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {

            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
           return _context.Set<T>().AsQueryable<T>();
        }

        public T GetById(int Id)
        {
            return _context.Set<T>().Find(Id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
