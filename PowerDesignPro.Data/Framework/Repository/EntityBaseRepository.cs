using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace PowerDesignPro.Data.Framework.Repository
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
       where T : class, IEntity, new()
    {

        private ApplicationDbContext _dataContext;
        protected readonly IDbSet<T> Dbset;

        public EntityBaseRepository(IDbFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            Dbset = DataContext.Set<T>();
        }

        protected IDbFactory DatabaseFactory
        {
            get; private set;
        }

        protected ApplicationDbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        public virtual T Add(T entity)
        {
            var result = Dbset.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
            return result;
        }

        public virtual T Update(T entity)
        {
            var result = Dbset.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
            return result;
        }

        public virtual void Delete(T entity)
        {
            Dbset.Remove(entity);
        }

        public virtual void Commit()
        {
            DataContext.SaveChanges();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return Dbset.Where(predicate);
        }

        public T Find(int id)
        {
            return Dbset.Find(id);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return Dbset.FirstOrDefault(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return Dbset;
        }

        public IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> set = Dbset;

            foreach (var includeProperty in includeProperties)
            {
                set = set.Include(includeProperty);
            }
            return set;
        }
    }
}
