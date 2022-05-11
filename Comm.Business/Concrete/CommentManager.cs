using Comm.Business.Abstract;
using Comm.DataAccess.Abstract;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Comm.Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private ICommentRepository _commentRepository;
        public CommentManager(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            
        }
        public void Create(Comment entity)
        {
            _commentRepository.Create(entity);

        }

        public void Delete(Comment entity)
        {
            _commentRepository.Delete(entity);
        }

       

        public IEnumerable<Comment> GetAll(Expression<Func<Comment, bool>> filter = null)
        {
            return _commentRepository.GetAll(filter);

        }

        public Comment GetById(int id)
        {
            return _commentRepository.GetById(id);
        }

        public void Update(Comment entity)
        {

            _commentRepository.Update(entity);
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
