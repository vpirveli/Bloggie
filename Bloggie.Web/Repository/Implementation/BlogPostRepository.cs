using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public readonly BloggieDbContext _dbContext;

        public BlogPostRepository(BloggieDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<BlogPost?> AddAsync(BlogPost blogPost)
        {
            await _dbContext.BlogPosts.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingBlogPost != null)
            {
                await Task.Run(() => _dbContext.BlogPosts.Remove(existingBlogPost));
                await _dbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _dbContext.BlogPosts.Include(i => i.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbContext.BlogPosts
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {

            var existingBlogPost = await GetAsync(blogPost.Id);

            if (existingBlogPost != null)
            {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Tags = blogPost.Tags;

                await _dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }
    }
}
