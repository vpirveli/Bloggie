using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository.Implementation;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Diagnostics.Eventing.Reader;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();

            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuit = Guid.Parse(selectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuit);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            var result = await _blogPostRepository.AddAsync(blogPost);

            if (result != null)
            {
                //success
            }
            else
            {
                //error
            }
            return RedirectToAction("Add");
        }

        public async Task<IActionResult> List()
        {
            var blogpost = await _blogPostRepository.GetAllAsync();
            return View(blogpost);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogPostRepository.GetAsync(id);
            var tagsDomainModel = await _tagRepository.GetAllAsync();
            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest blogPostRequest)
        {
            var blogPostModel = new BlogPost
            {
                Id = blogPostRequest.Id,
                Heading = blogPostRequest.Heading,
                PageTitle = blogPostRequest.PageTitle,
                Content = blogPostRequest.Content,
                ShortDescription = blogPostRequest.ShortDescription,
                FeaturedImageUrl = blogPostRequest.FeaturedImageUrl,
                UrlHandle = blogPostRequest.UrlHandle,
                PublishedDate = blogPostRequest.PublishedDate,
                Author = blogPostRequest.Author,
                Visible = blogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();

            foreach (var selectedTag in blogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostModel.Tags = selectedTags;

            var result = await _blogPostRepository.UpdateAsync(blogPostModel);

            if (result != null)
            {

                return RedirectToAction("Edit");
                //success
            }
            else
            {
                //error
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BlogPost blogPost)
        {
            return await DeleteById(blogPost.Id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id });
        }
    }
}
