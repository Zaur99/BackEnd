using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Abstract
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>> filter = null);

        Task<Comment> GetByIdAsync(int id);

        Task Update(Comment entity);
        Task Delete(Comment entity);
        Task Create(Comment entity);

        bool FindMatching(Product product,ApplicationUser user);

    }
}
