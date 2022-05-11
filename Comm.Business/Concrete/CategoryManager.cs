using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);

        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryRepository.DeleteFromCategory(categoryId,productId);
        }

        public IEnumerable<Category> GetAll(Expression<Func<Category,bool>> filter = null)
        {
            return filter == null ? _categoryRepository.GetAll().ToList() : _categoryRepository.GetAll(filter).ToList();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int id)
        {
            return _categoryRepository.GetByIdWithProducts(id);
        }
        public List<Category> GetAllWithSubCategories(Expression<Func<Category, bool>> filter = null) {
            return filter == null ? _categoryRepository.GetAllWithSubCategories() : _categoryRepository.GetAllWithSubCategories(filter);
        }
        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);

        }
    }
}
