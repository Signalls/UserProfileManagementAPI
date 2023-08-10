using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserProfileData.Domain;
using UserProfileData.DTO;

namespace UserProfileData.Repository
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserProfileRepo(UserManager<UserProfile> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResponseDto<Object>> CreateUserProfile(UserProfile user, string Password)
        {
            var response = new ResponseDto<Object>();
            var email = await _userManager.FindByEmailAsync(user.Email);
            if(email != null)
            {
                response.Result = "Try using another email";
                response.StatusCode = 400;
                response.DisplayMessage = "email already exist";
                return response;
            }
            var UserToCreate = await _userManager.CreateAsync(user, Password);
            var error = UserToCreate.Errors;
            if(UserToCreate.Succeeded)
            {
                response.Result = UserToCreate;
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                return response;
            }
            response.Result = error;
            response.StatusCode = 404;
            response.DisplayMessage = "failed";
            return response;
        }
        public async Task<APIResponse> AuthenticateUser(LoginRequestDto loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if(user == null)
            {
                return new APIResponse
                {
                    StatusCode = 404,
                    Message = "Invalid username or password."
                };

            }
            var verifiedUser = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if(verifiedUser)
            {

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                var jwtToken = GetToken(authClaims);

                return new APIResponse
                {
                    StatusCode = 200,
                    Message = "UserProfile Authenticated!",
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    Expiration = jwtToken.ValidTo
                };
            }

            return new APIResponse
            {
                StatusCode = 404,
                Message = "Invalid username or password."
            };
        }
        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        public async Task<ResponseDto<UserProfileUpdateDto>> GetLoggedInUserByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var response = new ResponseDto<UserProfileUpdateDto>();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = authSigningKey,
                    ValidateIssuerSigningKey = true
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var claims = principal.Claims;

                var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                var user = await _userManager.FindByNameAsync(userName);
                if(user == null || userName == null)
                {
                    response.StatusCode = 404;
                    response.DisplayMessage = "Invalid token or UserName not found";
                    response.Result = null;
                    return response;
                }
                var userResponse = _mapper.Map<UserProfileUpdateDto>(user);

                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result = userResponse;
                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = 400;
                response.DisplayMessage = ex.Message;
                response.Result = null;
                return response;
            }
        }
        public async Task<ResponseDto<Object>> UpdateUser(string token, UserProfileUpdateDto updateUserDto)
        {
            var response = new ResponseDto<Object>();
            try
            {
                var user = await GetLoggedInUserByToken(token);
                var userDetails = await _userManager.FindByEmailAsync(user.Result.Email);
                if(user == null)
                {
                    response.StatusCode = 402;
                    response.DisplayMessage = "User not found for the provided user ID.";
                    response.Result = null;
                    return response;
                }
                userDetails.Address = updateUserDto.Address;
                userDetails.PhoneNumber = updateUserDto.PhoneNumber;
                userDetails.PhoneNumberConfirmed = updateUserDto.PhoneNumberConfirmed;
                userDetails.EmailConfirmed = updateUserDto.EmailConfirmed;
                userDetails.UserName = updateUserDto.UserName;
                userDetails.Age = updateUserDto.Age;
                userDetails.City = updateUserDto.City;
                userDetails.Email = updateUserDto.Email;
                userDetails.maritalStatus = updateUserDto.maritalStatus;
                var updateResult = await _userManager.UpdateAsync(userDetails);
                var errors = updateResult.Errors;
                if(!updateResult.Succeeded)
                {
                    response.StatusCode = 404;
                    response.Result = errors;
                    response.DisplayMessage = "failed";
                }
                response.StatusCode = 200;
                response.DisplayMessage = $"{updateUserDto.UserName} information updated successfully.";
                response.Result = updateResult;
                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.DisplayMessage = ex.Message;
                response.Result = null;
                return response;
            }
        }
    }
}
