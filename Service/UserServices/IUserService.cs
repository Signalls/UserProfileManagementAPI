using UserProfileData.DTO;

namespace Service.UserServices
{
    public interface IUserService
    {
        public Task<ResponseDto<Object>> CreateUserProfile(UserProfileDto userProfile);
        public Task<APIResponse> AuthenticateUser(LoginRequestDto userProfile);
        public Task<ResponseDto<UserProfileUpdateDto>> GetUserProfile(string token);
        public Task<ResponseDto<Object>> UpdateUserProfile(string token, UserProfileUpdateDto userProfileUpdateDto);
    }
}
