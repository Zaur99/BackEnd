using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByIdWithProductsAsync(int id);
        void DeleteFromCategory(int categoryId, int productId);
        //Task<List<Category>> GetAllWithSubCategoriesAsync(Expression<Func<Category,bool>> filter= null);
    }
}
