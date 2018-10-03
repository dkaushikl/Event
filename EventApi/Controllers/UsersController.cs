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
        public async Task<IHttpActionResult> Register(AccountViewModel accountViewModel)
        {
            IHttpActionResult returnResult;
            if (this.Validation(accountViewModel, out returnResult))
            {
                return returnResult;
            }

            var checkEmailExist = await this.userRepository.CheckEmailExist(accountViewModel.Email);

            if (checkEmailExist)
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User already exist", null));
            }

            await this.userRepository.Register(accountViewModel);

            var userId = await this.userRepository.GetUserIdByEmail(accountViewModel.Email);

            var response = new
            {
                userId,
                email = accountViewModel.Email,
                token = JwtProvider.GenerateToken(accountViewModel.Email, Convert.ToString(userId))
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