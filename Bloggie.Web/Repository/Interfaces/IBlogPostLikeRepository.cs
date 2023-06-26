using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repository.Interfaces
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
        Task<BlogPostLike?> GetLikeForBlogForUser(Guid blogPostId, Guid userId);
    }
}
