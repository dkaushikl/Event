namespace Event.Repository.Interface
{
    using System.Threading.Tasks;

    using Event.Core;
    using Event.Data;

    public interface IUserRepository
    {
        Task<bool> CheckEmailExist(string email);

        Task<long> GetUserIdByEmail(string email);

        Task<User> Login(string email, string password);

        Task<bool> Register(RegisterViewModel registerViewModel);
    }
}