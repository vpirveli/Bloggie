using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync();
    }
}
