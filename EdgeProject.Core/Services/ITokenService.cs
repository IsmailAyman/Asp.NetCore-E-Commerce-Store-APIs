using EdgeProject.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Services
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
    }
}
