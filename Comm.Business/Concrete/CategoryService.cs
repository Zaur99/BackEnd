using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Concrete
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Create(Category entity)
        {
            await _categoryRepository.Create(entity);
        }

        public async Task Delete(Category entity)
        {
            await _categoryRepository.Delete(entity);

        }

        public void DeleteFromCategory(int categoryId, int productId)
        {
            _categoryRepository.DeleteFromCategory(categoryId, productId);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> filter = null)
        {
            var categories = (filter == null) ? _categoryRepository.GetAllAsync() : _categoryRepository.GetAllAsync(filter);
            return await categories;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetByIdWithProductsAsync(int id)
        {
            return await _categoryRepository.GetByIdWithProductsAsync(id);
        }
        //public List<Category> GetAllWithSubCategories(Expression<Func<Category, bool>> filter = null) {
        //    return filter == null ? _categoryRepository.GetAllWithSubCategories() : _categoryRepository.GetAllWithSubCategories(filter);
        //}
        public async Task Update(Category entity)
        {
            await _categoryRepository.Update(entity);

        }
    }
}
