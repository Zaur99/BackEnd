using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll(Expression<Func<Category, bool>> filter = null);

        Category GetById(int id);

        void Update(Category entity);
        void Delete(Category entity);
        void Create(Category entity);
        Category GetByIdWithProducts(int id);
        List<Category> GetAllWithSubCategories(Expression<Func<Category,bool>> filter = null);
        void DeleteFromCategory(int categoryId, int productId);
    }
}
