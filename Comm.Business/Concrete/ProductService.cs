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
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task Create(Product entity)
        {
            await _productRepository.Create(entity);
        }

        public async Task Delete(Product entity)
        {
            await _productRepository.Delete(entity);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> filter = null)
        {
            return await _productRepository.GetAllAsync(filter);
        }

        public async Task<IEnumerable<Product>> GetApprovedProductsForPageAsync(int page, int pageSize)
        {
            return await _productRepository.GetApprovedProductsForPageAsync(page, pageSize);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> GetByIdWithCategoriesAsync(int id)
        {
            return await _productRepository.GetByIdWithCategoriesAsync(id);
        }

        public async Task<int> GetCountByCategoryAsync(string category)
        {
            return await _productRepository.GetCountByCategoryAsync(category);
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsForPageAsync(string searchString, int page, int pageSize)
        {
            return await _productRepository.GetFilteredProductsForPageAsync(searchString, page, pageSize);
        }

        public async Task<Product> GetProductDetailsAsync(string url)
        {
            return await _productRepository.GetProductDetailsAsync(url);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int page, int pageSize)
        {
            return await _productRepository.GetProductsByCategoryAsync(category, page, pageSize);
        }



        public async Task Update(Product entity)
        {
            await _productRepository.Update(entity);
        }

        public async Task Update(Product entity, int[] categoryIds)
        {
            await _productRepository.Update(entity, categoryIds);
        }
    }
}
