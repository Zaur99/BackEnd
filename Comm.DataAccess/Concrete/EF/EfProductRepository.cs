using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfProductRepository : EfRepository<Product>, IProductRepository
    {
        protected new readonly CommerceContext context;

        public EfProductRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<Product> GetApprovedProductsForPage(int page, int pageSize)
        {
            var products = context.Products.Where(i=>i.IsApproved);

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        }

        public Product GetByIdWithCategories(int id)
        {
            

            return context.Products.Where(i => i.Id == id).FirstOrDefault();

        }

        public int GetCountByCategory(string category)
        {

            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(i => i.ProductCategories.Any(a => a.Category.Url == category.ToLower()));
            }

            return products.Count();

        }

        public IEnumerable<Product> GetFilteredProductsForPage(string searchString,int page, int pageSize)
        {
            var products = context.Products.Where(i => i.Name.Contains(searchString));

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Product GetProductDetails(string url)
        {

            return context.Products.Where(i => i.Url == url.ToLower()).FirstOrDefault();

        }

        public IEnumerable<Product> GetProductsByCategory(string category, int page, int pageSize)
        {

            var products = context.Products.AsQueryable();

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(i => i.ProductCategories.Any(a => a.Category.Url == category.ToLower()));

            }

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();


        }

        public void Update(Product entity, int[] categoryIds)
        {

            var product = context.Products.FirstOrDefault(i => i.Id == entity.Id);
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


                //int[] existingCategoryIds = product.ProductCategories.Select(i => i.CategoryId).ToArray();
                //for (int i = 0; i < categoryIds.Length; i++)
                //{

                //    if (!existingCategoryIds.Contains(categoryIds[i]))
                //    {
                //        unmatchedCategoryIds[i] = categoryIds[i];
                //    }
                //}


                //if (unmatchedCategoryIds != null)
                //{
                product.ProductCategories.Clear();
                    product.ProductCategories = categoryIds.Select(categoryId => new ProductCategory()
                    {
                        ProductId = entity.Id,
                        CategoryId = categoryId
                    }).ToList();
                //}
               
            }
            context.SaveChanges();



        }
    }
}
