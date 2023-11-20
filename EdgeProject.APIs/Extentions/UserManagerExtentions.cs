using EdgeProject.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EdgeProject.APIs.Extentions
{
    public static class UserManagerExtentions
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager,ClaimsPrincipal Currentuser)
        {
            var email = Currentuser.FindFirstValue(ClaimTypes.Email);

            var User= await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Email == email);

            return User;
        }
    }
}
