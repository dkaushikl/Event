namespace EventApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Common;
    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;
    using EventApi.Utility.JwtToken;

    public class UsersController : ApiController
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository) => this.userRepository = userRepository;

        [HttpPost]
        [Route("api/Users/Register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterViewModel objRegisterViewModel)
        {
            IHttpActionResult returnResult;
            if (this.RegisterValidation(objRegisterViewModel, out returnResult))
            {
                return returnResult;
            }

            if (this.Validation(new AccountViewModel { Email = objRegisterViewModel.Email, Password = objRegisterViewModel.Password }, out returnResult))
            {
                return returnResult;
            }

            var checkEmailExist = await this.userRepository.CheckEmailExist(objRegisterViewModel.Email);

            if (checkEmailExist)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User already exist", null));
            }

            await this.userRepository.Register(objRegisterViewModel);

            var userId = await this.userRepository.GetUserIdByEmail(objRegisterViewModel.Email);

            var response = new
            {
                userId,
                email = objRegisterViewModel.Email,
                token = JwtProvider.GenerateToken(objRegisterViewModel.Email, Convert.ToString(userId))
            };

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Register successfully!!", response));
        }

        [HttpPost]
        [Route("api/Users/Login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(AccountViewModel accountViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(accountViewModel, out returnResult))
            {
                return returnResult;
            }

            var password = EncryptDecrypt.Encrypt(accountViewModel.Password);
            var objUser = await this.userRepository.Login(accountViewModel.Email, password);

            if (objUser == null)
            {
                return this.Ok(
                    ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email and password!!", null));
            }

            var response = new
            {
                userId = objUser.Id,
                email = objUser.Email,
                token = JwtProvider.GenerateToken(objUser.Email, Convert.ToString(objUser.Id))
            };

            return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Login successfully!!", response));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Users/ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel objForgotPassword)
        {
            IHttpActionResult returnResult;
            if (this.ForgotPasswordValidation(objForgotPassword, out returnResult))
            {
                return returnResult;
            }

            var code = Utility.GetCode();

            var result = await this.userRepository.UpdateForgotPasswordCode(code, objForgotPassword.Email);

            if (result)
            {
                Utility.SendMail(objForgotPassword.Email, "Reset Password With Event manager", Utility.PopulateForgotPasswordBody(objForgotPassword.Email, code));

                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok,
                "New Password Confirmation Link sent. Please Check Your Mail Box.", null));
            }
            else
            {
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found.", null));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Users/ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel objResetPassword)
        {
            IHttpActionResult returnResult;
            if (this.Validation(new AccountViewModel { Email = objResetPassword.Email, Password = objResetPassword.Password }, out returnResult))
            {
                return returnResult;
            }

            if (this.ResetPasswordValidation(objResetPassword, out returnResult))
            {
                return returnResult;
            }

            var checkEmailExist = await this.userRepository.CheckEmailExist(objResetPassword.Email);

            if (!checkEmailExist)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User not found.", null));
            }

            var result = await this.userRepository.ResetPassword(objResetPassword);

            if (result)
            {
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok,
                "Reset Password Successfully.", null));
            }
            else
            {
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Your code is wrong.", null));
            }
        }

        private bool ForgotPasswordValidation(ForgotPasswordViewModel objForgotPassword, out IHttpActionResult returnResult)
        {
            if (objForgotPassword.Email.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address", null));
                return true;
            }

            if (!objForgotPassword.Email.IsEmail())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email", null));
                return true;
            }

            returnResult = null;
            return false;
        }

        private bool ResetPasswordValidation(ResetPasswordViewModel objResetPassword, out IHttpActionResult returnResult)
        {
            if (objResetPassword.Code.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Code is required.", null));
                return true;
            }

            returnResult = null;
            return false;
        }

        private bool RegisterValidation(RegisterViewModel objRegisterViewModel, out IHttpActionResult returnResult)
        {
            if (objRegisterViewModel.Firstname.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter firstname", null));
                return true;
            }

            if (objRegisterViewModel.Lastname.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter lastname", null));
                return true;
            }

            if (objRegisterViewModel.Mobile.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter mobile no", null));
                return true;
            }

            if (!objRegisterViewModel.Mobile.IsMobileNo())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid mobile no", null));
                return true;
            }

            returnResult = null;
            return false;
        }

        private bool Validation(AccountViewModel objAccountViewModel, out IHttpActionResult returnResult)
        {
            if (objAccountViewModel.Email.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address", null));
                return true;
            }

            if (!objAccountViewModel.Email.IsEmail())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email", null));
                return true;
            }

            if (objAccountViewModel.Password.IsEmpty())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter password", null));
                return true;
            }

            if (!objAccountViewModel.Password.IsPassword())
            {
                returnResult = this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Password length should be 8 to 15", null));
                return true;
            }

            returnResult = null;
            return false;
        }
    }
}