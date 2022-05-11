using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.DataAccess.Abstract
{
   public interface IRepository<T>
    {
        IEnumerable<T> GetAll(Expression<Func<T,bool>> filter = null);

        T GetById(int id);

        void Update(T entity);
        void Delete(T entity);
        void Create(T entity);
    }
}
