using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.UserServices;
using UserManagementSystemAPI.Controllers;
using UserProfileData.DTO;

namespace UserProfileTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;
        private readonly UserProfileController _userProfileController;
        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
            _userProfileController = new UserProfileController(_userService.Object);
        }
        [Fact]
        public async Task CreateUserProfile_Returns_OkResult()
        {
            //Arrange
            var user = new UserProfileDto
            {
                UserName = "Test",
                Email = "sean@gmail.com",
                Password = "Pass123#",
            };
            var expectedResult = new ResponseDto<Object>
            {
                StatusCode = 200,
                Result = $"UserProfile with Username {user.UserName} created successful",
                DisplayMessage = "Success"
            };
            _userService.Setup(service => service.CreateUserProfile(user))
                .ReturnsAsync(new ResponseDto<Object> { StatusCode = 200 });

            //Act
            var result = await _userProfileController.CreateUserProfile(user);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task CreateUserProfile_With_WrongPasswordFormat_Returns_BadRequest()
        {
            //Arrange

            var user = new UserProfileDto
            {
                UserName = "Test",
                Email = "sean@gmail.com",
                Password = "remmmtis",
            };
            var expectedResult = new ResponseDto<Object>
            {
                StatusCode = 200,
                Result = $"UserProfile with Username {user.UserName} created successful",
                DisplayMessage = "Success"
            };
            _userService.Setup(service => service.CreateUserProfile(user))
                .ReturnsAsync(new ResponseDto<Object> { StatusCode = 400 });
            //Act
            var result = await _userProfileController.CreateUserProfile(user);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task AUthenticate_UserProfile_OkRequest()
        {
            //Arrange
            var user = new LoginRequestDto
            {
                UserName = "string",
                Password = "Pass123#",
            };
            var expectedResult = new APIResponse
            {
                StatusCode = 200,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwianRpIjoiZjEyMTA1ZTctZmZhOS00NTVjLWE3M2EtNDIyNWM5YTBlNmYzIiwiZXhwIjoxNjkxNjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ.g9nDmc4ZrjDL7uA4evMjG9XcKQyfZ8dduLaW1egpGc0",
            };
            _userService.Setup(service => service.AuthenticateUser(user))
                .ReturnsAsync(
                new APIResponse
                {
                    StatusCode = 200,
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwianRpIjoiZjEyMTA1ZTctZmZhOS00NTVjLWE3M2EtNDIyNWM5YTBlNmYzIiwiZXhwIjoxNjkxNjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ.g9nDmc4ZrjDL7uA4evMjG9XcKQyfZ8dduLaW1egpGc0"
                }
                );
            //Act
            var result = await _userProfileController.AuthenticateUser(user);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task AUthenticate_UserProfile_With_WrongPassword_Returns_BadRequest()
        {
            //Arrange
            var user = new LoginRequestDto
            {
                UserName = "string",
                Password = "Pass123567#",
            };
            var expectedResult = new APIResponse
            {
                StatusCode = 404,
                Message = "Invalid username or password."
            };
            _userService.Setup(service => service.AuthenticateUser(user))
                .ReturnsAsync(
                new APIResponse
                {
                    StatusCode = 404,
                    Message = "Invalid username or password."
                });
            //Act
            var result = await _userProfileController.AuthenticateUser(user);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task AUthenticate_UserProfile_With_WrongUserName_Returns_BadRequest()
        {
            //Arrange
            var user = new LoginRequestDto
            {
                UserName = "string1",
                Password = "Pass123#",
            };
            var expectedResult = new APIResponse
            {
                StatusCode = 404,
                Message = "Invalid username or password."
            };
            _userService.Setup(service => service.AuthenticateUser(user))
                .ReturnsAsync(
                new APIResponse
                {
                    StatusCode = 404,
                    Message = "Invalid username or password."
                });
            //Act
            var result = await _userProfileController.AuthenticateUser(user);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task Get_UserProfile_With_Token_Returns_OkRequest()
        {
            //Arrange
            var user = new UserProfileUpdateDto();

            string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwianRpIjoiZjEyMTA1ZTctZmZhOS00NTVjLWE3M2EtNDIyNWM5YTBlNmYzIiwiZXhwIjoxNjkxNjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ.g9nDmc4ZrjDL7uA4evMjG9XcKQyfZ8dduLaW1egpGc0";
            var expectedResult = new ResponseDto<UserProfileUpdateDto>()
            {
                StatusCode = 200,
                Result = user,
            };
            _userService.Setup(service => service.GetUserProfile(Token))
                .ReturnsAsync(
                new ResponseDto<UserProfileUpdateDto>
                {
                    StatusCode = 200,
                    Result = user,
                });
            //Act
            var result = await _userProfileController.GetUserProfile(Token);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Get_UserProfile_With_InValidToken_Returns_BadRequestRequest()
        {
            //Arrange
            var user = new UserProfileUpdateDto();

            string Token = "eyJhbGciOiJLzA1L2lk567890-=jnnbnbnmkl;'5nIiwianRpIjoiZjEyMTA1ZTctZmZhOS00NTVjLWE3M2EtNDIyNWM5YTBlNmYzIiwiZXhwIjoxNjkxNjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ.g9nDmc4ZrjDL7uA4evMjG9XcKQyfZ8dduLaW1egpGc0";
            var expectedResult = new ResponseDto<UserProfileUpdateDto>
            {
                StatusCode = 404,
                DisplayMessage = "Failed",
                Result = null,
            };
            _userService.Setup(service => service.GetUserProfile(Token))
                .ReturnsAsync(
                new ResponseDto<UserProfileUpdateDto>
                {
                    StatusCode = 404,
                    Result = null,
                    DisplayMessage = "Failed"
                });
            //Act
            var result = await _userProfileController.GetUserProfile(Token);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task Update_UserProfile_With_ValidToken_Returns_OkRequest()
        {
            //Arrange
            var user = new UserProfileUpdateDto
            {
                UserName = "sean",
                Email = "Sean20@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "08136582045",
                PhoneNumberConfirmed = true,
            };

            string Token = "eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwianRpIjoiZjEyMTA1ZTctZmZhOS00NTVjLWE3M2EtNDIyNWM5YTBlNmYzIiwiZXhwIjoxNjkxNjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ";
            var expectedResult = new ResponseDto<Object>
            {
                StatusCode = 200,
                DisplayMessage = "Sucessful",
                Result = user,
            };
            _userService.Setup(service => service.UpdateUserProfile(Token, user))
                .ReturnsAsync(
                new ResponseDto<Object>
                {
                    StatusCode = 200,
                    Result = user,
                    DisplayMessage = "Successful"
                });
            //Act
            var result = await _userProfileController.UpdateUserProfile(Token, user);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Update_UserProfile_With_InValidToken_Returns_BadRequest()
        {
            //Arrange
            var user = new UserProfileUpdateDto
            {
                UserName = "sean",
                Email = "Sean20@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "08136582045",
                PhoneNumberConfirmed = true,
            };

            string Token = "e567890iop9ikknIiwianRpIjoiZjEyMT4567890-NjIzNzEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDQzNzEifQ";
            var expectedResult = new ResponseDto<Object>
            {
                StatusCode = 404,
                DisplayMessage = "Failed",
                Result = user,
            };
            _userService.Setup(service => service.UpdateUserProfile(Token, user))
                .ReturnsAsync(
                new ResponseDto<Object>
                {
                    StatusCode = 404,
                    Result = user,
                    DisplayMessage = "Failed"
                });
            //Act
            var result = await _userProfileController.UpdateUserProfile(Token, user);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}