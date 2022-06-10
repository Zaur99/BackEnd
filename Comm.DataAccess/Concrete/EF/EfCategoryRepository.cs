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
    public class EfCategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        protected new readonly CommerceContext context;

        public EfCategoryRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<Category> GetByIdWithProductsAsync(int id)
        {

            return await context.Categories.FirstOrDefaultAsync(i => i.Id == id);

        }

        public void DeleteFromCategory(int categoryId, int productId)
        {

            var cmd = @"delete from ProductCategory where ProductId=@p0 AND CategoryId=@p1";
            context.Database.ExecuteSqlRaw(cmd, productId, categoryId);

        }

        //public async Task<List<Category>> GetAllWithSubCategoriesAsync(Expression<Func<Category, bool>> filter = null)
        //{
        //    var 
        //    return await (filter == null) ? context.Categories.ToListAsync() : context.Categories.Where(filter).ToListAsync();

        //}
    }
}
