using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Website.Models.BlogComment;

namespace Website.Repository
{
    interface IBlogCommentRepository
    {
        public Task<BlogComment> UpsertAsync(BlogCommentCreate blogCommentCreate, int applicationUserId);

        public Task<List<BlogComment>> GetAllAsync(int blogId);

        public Task<BlogComment> GetAsync(int blogCommentId);

        public Task<int> DeleteAsync(int blogCommentId);
    }
}
