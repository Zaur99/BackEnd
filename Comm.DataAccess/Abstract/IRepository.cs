using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Abstract
{
   public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter = null);

        Task<T> GetByIdAsync(int id);

        Task Update(T entity);
        Task Delete(T entity);
        Task Create(T entity);
    }
}
