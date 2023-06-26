using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository.Implementation
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext _bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await _bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<BlogPostLike?> GetLikeForBlogForUser(Guid blogPostId, Guid userId)
        {
            return await _bloggieDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId && x.UserId == userId).FirstOrDefaultAsync();

        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _bloggieDbContext.BlogPostLike
                .CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
