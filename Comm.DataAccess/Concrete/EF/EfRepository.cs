using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfRepository<T> : IRepository<T>
        where T : class

    {
        protected readonly CommerceContext context;

        public EfRepository(CommerceContext context)
        {
            this.context = context;
        }
        public void Create(T entity)
        {

            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();

        }

        public void Delete(T entity)
        {

            context.Entry(entity).State = EntityState.Deleted;
            context.SaveChanges();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {

            return filter == null ? context.Set<T>().ToList() : context.Set<T>().Where(filter).ToList();

        }

        public T GetById(int id)
        {

            return context.Set<T>().Find(id);

        }

        public virtual void Update(T entity)
        {

            context.Entry(entity).State = EntityState.Modified;

            context.SaveChanges();

        }
    }
}
