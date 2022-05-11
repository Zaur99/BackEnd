using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.DataAccess.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductDetails(string url);
        IEnumerable<Product> GetProductsByCategory(string category,int page,int pageSize);
        int GetCountByCategory(string category);
        void Update(Product entity, int[] categoryIds);
        Product GetByIdWithCategories(int id);
        IEnumerable<Product> GetHomeProducts();
        IEnumerable<Product> GetPopularProducts();
    }
}
