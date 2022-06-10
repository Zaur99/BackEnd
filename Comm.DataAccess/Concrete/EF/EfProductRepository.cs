using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfProductRepository : EfRepository<Product>, IProductRepository
    {
        protected new readonly CommerceContext context;

        public EfProductRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Product>> GetApprovedProductsForPageAsync(int page, int pageSize)
        {
            var products = context.Products.Where(i=>i.IsApproved);

            return  await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        }

        public async Task<Product> GetByIdWithCategoriesAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(i => i.Id == id);

        }

        public async Task<int> GetCountByCategoryAsync(string category)
        {

            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(i => i.ProductCategories.Any(a => a.Category.Url == category.ToLower()));
            }

            return await products.CountAsync();

        }

        public async Task<IEnumerable<Product>> GetFilteredProductsForPageAsync(string searchString,int page, int pageSize)
        {
            var products = context.Products.Where(i => i.Name.Contains(searchString));

            return  await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Product> GetProductDetailsAsync(string url)
        {

            return await context .Products.FirstOrDefaultAsync(i => i.Url == url.ToLower());

        }

        public async Task<IEnumerable<Product>>  GetProductsByCategoryAsync(string category, int page, int pageSize)
        {

            var products = context.Products.AsQueryable();

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(i => i.ProductCategories.Any(a => a.Category.Url == category.ToLower()));

            }

            return await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


        }

        public async Task Update(Product entity, int[] categoryIds)
        {

            var product = await context.Products.FirstOrDefaultAsync(i => i.Id == entity.Id);
            var unmatchedCategoryIds = new List<int>();
            if (product != null)
            {
                product.Name = entity.Name;
                product.Url = entity.Url;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.ImageUrl = entity.ImageUrl;
                product.IsApproved = entity.IsApproved;
                product.IsHome = entity.IsHome;


                product.ProductCategories.Clear();
                    product.ProductCategories = categoryIds.Select(categoryId => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = categoryId
                    }).ToList();
               
               
            }
            await context.SaveChangesAsync();



        }
    }
}
