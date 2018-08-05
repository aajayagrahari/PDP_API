using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PowerDesignPro.Data.Framework.Interface
{
    public interface IEntityBaseRepository<T> where T : class, IEntity, new()
    {
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        //IEnumerable<T> AllIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        //IEnumerable<T> GetAll();
        //int Count();
        //T GetSingle(int id);
        //T GetSingle(Expression<Func<T, bool>> predicate);
        //T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        //IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Find(int id);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        T GetSingle(Expression<Func<T, bool>> predicate);

        void Commit();
    }
}
