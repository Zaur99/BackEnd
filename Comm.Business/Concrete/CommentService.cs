using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Concrete
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            
        }
        public async Task Create(Comment entity)
        {
            await _commentRepository.Create(entity);

        }

        public async  Task Delete(Comment entity)
        {
           await  _commentRepository.Delete(entity);
        }

       

        public async Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return await _commentRepository.GetAllAsync(filter);

        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task Update(Comment entity)
        {

            await _commentRepository.Update(entity);
        }

        public bool FindMatching(Product product, ApplicationUser user)
        {
            if (product.Comments != null && user.Comments!=null)
            {
                return _commentRepository.FindMatching(product, user);

            }
            return false;
        }
    }
}
