using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            this._userManager = userManager;
        }
        public async Task<IActionResult> List()
        {
            var users = await _userRepository.GetAllAsync();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email
            };

            var identityResult =
                await _userManager.CreateAsync(identityUser, userViewModel.Password);

            if (identityResult.Succeeded)
            {
                var roles = new List<string> { "User" };

                if (userViewModel.AdminRoleChecbox)
                {
                    roles.Add("Admin");
                }

                identityResult =
                    await _userManager.AddToRolesAsync(identityUser, roles);

                if (identityResult.Succeeded && identityResult != null)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (identityResult.Succeeded && identityResult != null)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            return View();
        }
    }
}
