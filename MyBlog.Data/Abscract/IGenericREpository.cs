using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlog.Data.Abscract
{
    public interface IGenericRepository<T> where T: class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
