using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comm.DataAccess.Abstract
{
    public interface ICommentRepository:IRepository<Comment>
    {
        public bool FindMatching(Product product,ApplicationUser user);
    }
}
