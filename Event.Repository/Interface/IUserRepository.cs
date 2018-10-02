namespace Event.Repository.Interface
{
    using System.Threading.Tasks;

    using Event.Core;
    using Event.Data;

    public interface IUserRepository
    {
        Task<bool> Register(RegisterViewModel registerViewModel);

        Task<long> GetUserIdByEmail(string email);
    }
}