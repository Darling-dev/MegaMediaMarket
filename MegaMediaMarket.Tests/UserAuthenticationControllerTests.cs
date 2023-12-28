using Xunit;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MegaMediaMarket.Tests
{
    public class UserAuthenticationControllerTests
    {
        private readonly UserAuthenticationController controller;
        private readonly Mock<IUserAuthenticationService> mockAuthService;

        public UserAuthenticationControllerTests()
        {
            mockAuthService = new Mock<IUserAuthenticationService>();
            controller = new UserAuthenticationController(mockAuthService.Object);
        }
        [Fact]
        public async Task RegistrationPost_ValidModel_RedirectsAfterRegistration()
        {
            
            var registrationModel = new RegistrationModel();
            mockAuthService.Setup(s => s.RegisterAsync(registrationModel))
                           .ReturnsAsync(new UserAuthenticationResult { StatusCode = 1 });

            
            var result = await controller.Registration(registrationModel);

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(UserAuthenticationController.Registration), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Logout_CallsServiceAndRedirectsToLogin()
        {
            
            var result = await controller.Logout();

            
            mockAuthService.Verify(s => s.LogoutAsync(), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(UserAuthenticationController.Login), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task ChangePasswordPost_InvalidModel_ReturnsViewWithModel()
        {
            
            var changePasswordModel = new ChangePasswordModel();
            controller.ModelState.AddModelError("Error", "Some error");

            
            var result = await controller.ChangePassword(changePasswordModel);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(changePasswordModel, viewResult.Model);
        }

        [Fact]
        public void Login_ReturnsViewResult()
        {
            
            var result = controller.Login();

            
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task LoginPost_InvalidModel_ReturnsViewWithModel()
        {
            
            controller.ModelState.AddModelError("Error", "Some error");
            var loginModel = new LoginModel();

            
            var result = await controller.Login(loginModel);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(loginModel, viewResult.Model);
        }

        [Fact]
        public async Task LoginPost_ValidModel_RedirectsOnChangePassword()
        {
            
            var loginModel = new LoginModel();
            mockAuthService.Setup(s => s.LoginAsync(loginModel))
                           .ReturnsAsync(new UserAuthenticationResult { StatusCode = 1 });

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, "username"),
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            
            var result = await controller.Login(loginModel);

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ChangePassword", redirectToActionResult.ActionName);
            Assert.Equal("UserAuthentication", redirectToActionResult.ControllerName);
        }


        [Fact]
        public void Registration_ReturnsViewResult()
        {
            
            var result = controller.Registration();

            
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task RegistrationPost_InvalidModel_ReturnsViewWithModel()
        {
            
            var registrationModel = new RegistrationModel();
            controller.ModelState.AddModelError("Error", "Some error");

            
            var result = await controller.Registration(registrationModel);

            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(registrationModel, viewResult.Model);
        }

        [Fact]
        public async Task Logout_RedirectsToLogin()
        {
            
            var result = await controller.Logout();

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectToActionResult.ActionName);
        }

        [Fact]
        public void ChangePassword_ReturnsViewResult()
        {
            
            var result = controller.ChangePassword();

            
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ChangePasswordPost_ValidModel_RedirectsToChangePassword()
        {
            
            var changePasswordModel = new ChangePasswordModel();
            mockAuthService.Setup(s => s.ChangePasswordAsync(changePasswordModel, It.IsAny<string>()))
                           .ReturnsAsync(new UserAuthenticationResult { StatusCode = 1 });

            
            var result = await controller.ChangePassword(changePasswordModel);

            
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ChangePassword", redirectToActionResult.ActionName);
        }
    }
}
