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
        [Route("api/CreateUser")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateUser(RegisterViewModel registerViewModel)
        {
            if (string.IsNullOrWhiteSpace(registerViewModel.Email))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address!!",
               null));

            var checkIsEmail = await registerViewModel.Email.IsEmail();

            if (!checkIsEmail)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email!!",
               null));

            if (string.IsNullOrWhiteSpace(registerViewModel.Password))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter password!!",
               null));

            var checkEmailExist = await this.userRepository.CheckEmailExist(registerViewModel.Email);

            if (checkEmailExist)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "User already exist",
               null));

            var objRegister = await this.userRepository.Register(registerViewModel);

            var userId = await this.userRepository.GetUserIdByEmail(registerViewModel.Email);

            var response = new
            {
                userId,
                email = registerViewModel.Email,
                token = JwtProvider.GenerateToken(registerViewModel.Email, Convert.ToString(userId))
            };

            return Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Register successfully!!",
              response));
        }

        [HttpPost]
        [Route("api/Login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(RegisterViewModel registerViewModel)
        {
            if (string.IsNullOrWhiteSpace(registerViewModel.Email))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter email address!!",
               null));

            var checkIsEmail = await registerViewModel.Email.IsEmail();

            if (!checkIsEmail)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email!!",
               null));

            if (string.IsNullOrWhiteSpace(registerViewModel.Password))
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter password!!",
               null));

            var password = EncryptDecrypt.Encrypt(registerViewModel.Password);
            var objUser = await this.userRepository.Login(registerViewModel.Email, password);

            if (objUser == null)
                return Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "Enter valid email and password!!",
              null));

            var response = new
            {
                userId = objUser.Id,
                email = objUser.Email,
                token = JwtProvider.GenerateToken(objUser.Email, Convert.ToString(objUser.Id))
            };

            return Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Login successfully!!",
              response));
        }
    }
}