using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Abstract
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAll(Expression<Func<Comment, bool>> filter = null);

        Comment GetById(int id);

        void Update(Comment entity);
        void Delete(Comment entity);
        void Create(Comment entity);

        public bool FindMatching(Product product,ApplicationUser user);

    }
}
