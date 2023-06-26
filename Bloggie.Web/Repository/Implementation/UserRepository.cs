using Bloggie.Web.Data;
using Bloggie.Web.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bloggie.Web.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly string superEmail = "superadmin@bloggie.com";
        private readonly AuthDbContext _authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this._authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return await _authDbContext.Users.Where(x => x.Email != superEmail).ToListAsync();
        }
    }
}
