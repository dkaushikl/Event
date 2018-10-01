namespace Event.Repository.Interface
{
    using System.Threading.Tasks;

    using Event.Core;

    public interface IUserRepository
    {
        Task<bool> Register(RegisterViewModel registerViewModel);
    }
}