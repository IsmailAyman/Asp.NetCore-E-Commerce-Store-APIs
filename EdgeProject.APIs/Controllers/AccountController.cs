using AutoMapper;
using EdgeProject.APIs.Dtos;
using EdgeProject.APIs.Errors;
using EdgeProject.APIs.Extentions;
using EdgeProject.Core.Entities.Identity;
using EdgeProject.Core.Services;
using EdgeProject.Repository.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdgeProject.APIs.Controllers
{

    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("Login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }

        [HttpPost("Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExsist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email already exsist" } });
           
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }

        [Authorize]
        [HttpGet("currentuser")]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });

        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
           
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            var address = mapper.Map<Address, AddressDto>(user.Address);

            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddres)
        {
            var address = mapper.Map<AddressDto, Address>(updatedAddres);
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));
            
            
            return Ok(updatedAddres);

        }

        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckEmailExsist(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }

    }
}
