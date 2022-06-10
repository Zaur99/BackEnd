using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductDetailsAsync(string url);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category,int page,int pageSize);
        Task<IEnumerable<Product>> GetFilteredProductsForPageAsync(string searchString, int page, int pageSize);
        Task<IEnumerable<Product>> GetApprovedProductsForPageAsync(int page, int pageSize);
        Task<int> GetCountByCategoryAsync(string category);
        Task Update(Product entity, int[] categoryIds);
        Task<Product> GetByIdWithCategoriesAsync(int id);
       

    }
}
