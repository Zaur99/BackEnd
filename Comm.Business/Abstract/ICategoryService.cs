using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Abstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> filter = null);

        Task<Category> GetByIdAsync(int id);

        Task Update(Category entity);
        Task Delete(Category entity);
        Task Create(Category entity);
        //Category GetByIdWithProducts(int id);
        //List<Category> GetAllWithSubCategories(Expression<Func<Category,bool>> filter = null);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
