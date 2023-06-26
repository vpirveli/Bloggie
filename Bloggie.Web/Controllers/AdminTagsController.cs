using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {

        ITagRepository _repository;
        public AdminTagsController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);
            if (!ModelState.IsValid)
            {
                return View();
            }
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await _repository.AddAsync(tag);

            return RedirectToAction("List");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await _repository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _repository.GetAsync(id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }
            return View(null);
        }

        public async Task<IActionResult> Edit(EditTagRequest editTag)
        {
            var tag = new Tag
            {
                Id = editTag.Id,
                Name = editTag.Name,
                DisplayName = editTag.DisplayName
            };

            var existingTag = await _repository.UpdateAsync(tag);

            if (existingTag != null)
            {
                return RedirectToAction("List");
                //show success notification
            }
            else
            {
                //show error notification
            }
            return RedirectToAction("Edit", new { id = editTag.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTag)
        {
            return await DeleteById(editTag.Id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var existingTag = await _repository.DeleteAsync(id);

            if (existingTag != null)
            {
                return RedirectToAction("List");
                // Show a success notification
            }
            else
            {
                //Show an Error notification
            }
            return RedirectToAction("Edit", new { id });
        }

        private void ValidateAddTagRequest(AddTagRequest addTagRequest)
        {
            if(addTagRequest.Name != null && addTagRequest.DisplayName != null)
            {
                if(addTagRequest.Name == addTagRequest.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as Display Name");
                }
            }
        }
    }
}
