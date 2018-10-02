namespace EventApi.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Data;
    using Event.Repository.Interface;

    using EventApi.Utility;
    using Microsoft.IdentityModel.Tokens;

    public class UsersController : BaseController
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository) => this.userRepository = userRepository;

        [HttpPost]
        [Route("api/Users/CreateUser")]
        public async Task<Message> CreateUser(RegisterViewModel registerViewModel) =>
            await this.userRepository.Register(registerViewModel)
                ? new Message
                {
                    Description = "User register successfully",
                    Status = ResponseStatus.Ok.ToString(),
                    IsSuccess = true
                }
                : new Message
                {
                    Description = "Something went wrong!!",
                    Status = ResponseStatus.Error.ToString(),
                    IsSuccess = false
                };

        [HttpPost]
        [Route("api/Users/Login")]
        public IHttpActionResult Login(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
                return CreateErrorResponse("Please enter Email and Password to continue.");

            if (string.IsNullOrWhiteSpace(registerViewModel.Email))
                return CreateErrorResponse($"{nameof(registerViewModel.Email)} cannot be empty");

            if (string.IsNullOrWhiteSpace(registerViewModel.Password))
                return CreateErrorResponse($"{nameof(registerViewModel.Password)} cannot be empty");

            User objUser = new User()
            {
                Email = "dkaushikl@gmail.com",
                Password = "123",
                CreateDate = DateTime.Now,
            };
            var token = this.CreateNewToken(objUser);

            return new Message
            {
                Description = token,
                Status = ResponseStatus.Ok.ToString(),
                IsSuccess = true
            };
        }

        //private static string CreateToken(string userId)
        //{
        //    var issuedAt = DateTime.UtcNow;
        //    var expires = DateTime.UtcNow.AddDays(7);
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) });

        //    const string SecurityKey = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        //    var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(SecurityKey));

        //    var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

        //    var token = tokenHandler.CreateJwtSecurityToken(
        //        "http://localhost:50554",
        //        "http://localhost:50554",
        //        claimsIdentity,
        //        issuedAt,
        //        expires,
        //        signingCredentials: signingCredentials);

        //    var tokenString = tokenHandler.WriteToken(token);

        //    return tokenString;
        //}


        public string CreateNewToken(User objUser, int? tokenExpiryInMins)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, objUser.Email),
                new Claim("UserId", objUser.Id.ToString()),
            });

            const string SecurityKey = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(SecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(tokenExpiryInMins ?? 10);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(
                "http://localhost:50554",
                "http://localhost:50554",
                claimsIdentity,
                issuedAt,
                expires,
                null,
                signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}