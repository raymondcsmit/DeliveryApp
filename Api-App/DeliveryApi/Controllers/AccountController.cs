using Core;
using Delivery.Services.Implementation;
using Delivery.ViewModel;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost(nameof(Register))]
        public async Task<ResponseResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return ResponseFactory.CreateBadRequest(String.Join(", ", errors));
            }

            var user = new User { UserName = model.UserName, Email = model.Email, UserTypeId = 1 }; // You may need to adjust this based on your User model
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return ResponseFactory.CreateSuccess($"User registered successfully.", user);

            }

            return ResponseFactory.CreateBadRequest(String.Join(",", result.Errors.Select(c => c.Description)));
        }

        [HttpPost(nameof(SignIn))]
        public async Task<ResponseResult> SignIn(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return _tokenService.GetToken(user);
            }
            return ResponseFactory.CreateNotFound($"Invalid username or password.");
        }

        // Add forget password, recover password methods as needed
    }

}
