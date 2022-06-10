using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Abstract
{
    public interface IProductService
    {

        Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> filter = null);
        Task<Product> GetProductDetailsAsync(string url);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category,int page,int pageSize);
        Task<IEnumerable<Product>> GetApprovedProductsForPageAsync(int page,int pageSize);
        Task<IEnumerable<Product>> GetFilteredProductsForPageAsync(string searchString,int page,int pageSize);
        Task<Product> GetByIdAsync(int id);
        Task<int> GetCountByCategoryAsync(string category);
        Task Update(Product entity);
        Task Update(Product entity,int[] categoryIds);
        Task Delete(Product entity);
        Task Create(Product entity);
        Task<Product> GetByIdWithCategoriesAsync(int id);
    }
}
