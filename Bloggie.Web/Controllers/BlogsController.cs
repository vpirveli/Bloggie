using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository,
                                    SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
                                    IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostLikeRepository = blogPostLikeRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            this._blogPostCommentRepository = blogPostCommentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogdetailsViewModel = new BloggDetailsViewModel();

            if (blogPost != null)
            {
                //Getting likes

                if (_signInManager.IsSignedIn(User) && Guid.TryParse(_userManager.GetUserId(User), out var userId))
                {
                    var likeFromUser = await _blogPostLikeRepository.GetLikeForBlogForUser(blogPost.Id, userId);
                    liked = likeFromUser != null;
                }


                // Getting Comments
                var blogCommentDomainModel = await _blogPostCommentRepository.GetCommentsByBlogId(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach(var blogComment in blogCommentDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        UserName = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                blogdetailsViewModel = new BloggDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id),
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }

            return View(blogdetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BloggDetailsViewModel blogDetailsViewModel)
        {
            if (_signInManager.IsSignedIn(User) && Guid.TryParse(_userManager.GetUserId(User), out var userId))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = userId,
                    DateAdded = DateTime.Now
                };

                await _blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs",
                    new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
        }
    }
}
