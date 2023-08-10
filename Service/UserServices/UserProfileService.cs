using Service.UserServices;
using UserProfileData.Domain;
using UserProfileData.DTO;
using UserProfileData.Repository;

namespace Service.UserService
{
    public class UserProfileService : IUserService
    {
        private readonly IUserProfileRepo _userProfileRepo;
        public UserProfileService(IUserProfileRepo userProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
        }
        public async Task<ResponseDto<Object>> CreateUserProfile(UserProfileDto userProfile)
        {
            try
            {
                var response = new ResponseDto<Object>();

                var user = new UserProfile
                {
                    Email = userProfile.Email,
                    UserName = userProfile.UserName,
                };
                var createUser = await _userProfileRepo.CreateUserProfile(user, userProfile.Password);
                if(createUser.StatusCode == 200)
                {
                    response.StatusCode = 200;
                    response.DisplayMessage = "Success";
                    response.Result = $"UserProfile with Username {userProfile.UserName} created successfully";
                    return response;
                }
                response.StatusCode = createUser.StatusCode;
                response.DisplayMessage = createUser.DisplayMessage;
                response.Result = createUser.Result;
                return response;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<APIResponse> AuthenticateUser(LoginRequestDto userProfile)
        {
            try
            {
                var createUser = await _userProfileRepo.AuthenticateUser(userProfile);
                if(createUser.IsSuccess)
                {
                    return createUser;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDto<UserProfileUpdateDto>> GetUserProfile(string token)
        {
            try
            {
                var userProfile = await _userProfileRepo.GetLoggedInUserByToken(token);
                if(userProfile != null)
                {
                    return userProfile;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseDto<Object>> UpdateUserProfile(string token, UserProfileUpdateDto userProfileUpdateDto)
        {
            try
            {
                var userProfile = await _userProfileRepo.UpdateUser(token, userProfileUpdateDto);
                if(userProfile != null)
                {
                    return userProfile;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
