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
    public class EfCategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        protected new readonly CommerceContext context;

        public EfCategoryRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }
        public Category GetByIdWithProducts(int id)
        {

            return context.Categories.Where(i => i.Id == id).Include(i => i.ProductCategories).ThenInclude(i => i.Product).FirstOrDefault();

        }

        public void DeleteFromCategory(int categoryId, int productId)
        {

            var cmd = @"delete from ProductCategory where ProductId=@p0 AND CategoryId=@p1";
            context.Database.ExecuteSqlRaw(cmd, productId, categoryId);

        }

        public List<Category> GetAllWithSubCategories(Expression<Func<Category, bool>> filter = null)
        {

            return filter == null ? context.Categories.Include(i => i.Children).ToList() : context.Categories.Where(filter).Include(i => i.Children).ToList();

        }
    }
}
