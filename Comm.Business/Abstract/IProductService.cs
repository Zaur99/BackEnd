using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface IProductService
    {

        IEnumerable<Product> GetAll(Expression<Func<Product, bool>> filter = null);
        Product GetProductDetails(string url);
        IEnumerable<Product> GetProductsByCategory(string category,int page,int pageSize);
        IEnumerable<Product> GetHomeProducts();
        Product GetById(int id);
        int GetCountByCategory(string category);
        void Update(Product entity);
        void Update(Product entity,int[] categoryIds);
        void Delete(Product entity);
        void Create(Product entity);
        Product GetByIdWithCategories(int id);
    }
}
