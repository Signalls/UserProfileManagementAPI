using Microsoft.AspNetCore.Mvc;
using Service.UserServices;
using UserProfileData.DTO;

namespace UserManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("create/user")]
        public async Task<IActionResult> CreateUserProfile(UserProfileDto userProfile)
        {
            if(ModelState.IsValid)
            {
                var createUser = await _userService.CreateUserProfile(userProfile);
                if(createUser.StatusCode == 200)
                {
                    return Ok(createUser);
                }
                return BadRequest(createUser);
            }
            return BadRequest(userProfile);
        }
        [HttpPost("authenticate/user")]

        public async Task<IActionResult> AuthenticateUser(LoginRequestDto userProfile)
        {
            if(ModelState.IsValid)
            {
                var createUser = await _userService.AuthenticateUser(userProfile);
                if(createUser.StatusCode == 200)
                {
                    return Ok(createUser);
                }
                return BadRequest(createUser);
            }
            return BadRequest(userProfile);
        }
        [HttpGet("user/profile")]
        public async Task<IActionResult> GetUserProfile(string token)
        {
            if(ModelState.IsValid)
            {

                var userProfile = await _userService.GetUserProfile(token);
                if(userProfile.StatusCode == 200)
                {
                    return Ok(userProfile);
                }
                return BadRequest(userProfile);
            }
            return BadRequest(token);
        }
        [HttpPut("update/user/profile")]
        public async Task<IActionResult> UpdateUserProfile(string token, UserProfileUpdateDto userProfileUpdateDto)
        {
            if(ModelState.IsValid)
            {
                var userProfile = await _userService.UpdateUserProfile(token, userProfileUpdateDto);
                if(userProfile.StatusCode == 200)
                {
                    return Ok(userProfile);
                }
                return BadRequest(userProfile);
            }
            return BadRequest(userProfileUpdateDto);
        }
    }
}
