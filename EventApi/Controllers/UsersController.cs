namespace EventApi.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

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

        [HttpGet]
        [Route("api/Login")]
        [AllowAnonymous]
        public Message Login(RegisterViewModel registerViewModel) =>
            new Message
                {
                    Description = JwtProvider.GenerateToken("dkaushikl@gmail.com", "1"),
                    Status = ResponseStatus.Ok.ToString(),
                    IsSuccess = true
                };
    }
}