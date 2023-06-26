using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository.Implementation
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext _bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _bloggieDbContext.BlogPostComments.AddAsync(blogPostComment);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogId(Guid blogPostId)
        {
            var taskss = await _bloggieDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
            return taskss;
        }
    }
}
