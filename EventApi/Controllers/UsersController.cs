namespace EventApi.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Event.Core;
    using Event.Repository.Interface;

    using EventApi.Utility;

    public class UsersController : ApiController
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
        public Message Login(RegisterViewModel registerViewModel)
        {
            var token = CreateToken("1");
            return new Message
            {
                Description = token,
                Status = ResponseStatus.Ok.ToString(),
                IsSuccess = true
            };
        }

        private static string CreateToken(string userId)
        {
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) });

            const string SecurityKey = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(SecurityKey));

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var token = tokenHandler.CreateJwtSecurityToken(
                "http://localhost:50554",
                "http://localhost:50554",
                claimsIdentity,
                issuedAt,
                expires,
                signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}