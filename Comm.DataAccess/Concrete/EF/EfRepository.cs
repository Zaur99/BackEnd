using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public async Task Create(T entity)
        {

            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            
        }

        public async Task Delete(T entity)
        {

            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {

            return filter == null ? await context.Set<T>().ToListAsync() : await context.Set<T>().Where(filter).ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {

            return await context.Set<T>().FindAsync(id);

        }

        public async virtual Task Update(T entity)
        {

            context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync();

        }
    }
}
