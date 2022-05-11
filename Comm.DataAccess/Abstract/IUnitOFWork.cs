using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository Carts { get; }
        
        IProductRepository Products{ get;}
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        
        ICommentRepository Comments { get; }
        void Save();
    }
}
