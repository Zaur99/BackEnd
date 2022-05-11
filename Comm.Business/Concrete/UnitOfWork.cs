using Comm.DataAccess.Abstract;
using Comm.DataAccess.Concrete.EF;
using Comm.DataAccess.IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Business.Concrete
{
    public class UnitOfWork :IUnitOfWork
    {
        private CommerceContext _context;
        public UnitOfWork(CommerceContext context)
        {
            _context = context;
            Carts = new EfCartRepository(_context);
            Products = new EfProductRepository(_context);
            Categories = new EfCategoryRepository(_context);
            Orders = new EfOrderRepository(_context);
            Comments = new EfCommentRepository(_context);
        }

        public ICartRepository Carts { get; private set; }

        public IProductRepository Products { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public ICommentRepository Comments { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
             _context.SaveChanges();
        }
    }
}
