using Comm.DataAccess.Abstract;
using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comm.DataAccess.Concrete.EF
{
    public class EfCommentRepository : EfRepository<Comment>, ICommentRepository
    {
        protected new readonly CommerceContext context;

        public EfCommentRepository(CommerceContext context) : base(context)
        {
            this.context = context;
        }

        public bool FindMatching(Product product, ApplicationUser user)
        {
             //int id = user.Comments.Where(i => (product.Comments.Select(x => x.Id == i.Id).FirstOrDefault())).Select(x => x.Id).FirstOrDefault();
            var comment = product.Comments.Where(i => i.UserId == user.Id).FirstOrDefault();

            //int id = (from comment in user.Comments
            //          where
            //            comment.Id == ((from prodComment in product.Comments
            //            where comment.Id == prodComment.Id
            //            select prodComment.Id).FirstOrDefault())
            //            select comment.Id).FirstOrDefault();
            return comment != null ? true : false;

        }
    }
}
