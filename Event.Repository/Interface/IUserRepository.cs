namespace Event.Repository.Interface
{
    using System.Threading.Tasks;

    using Event.Core;
    using Event.Data;

    public interface IUserRepository
    {
        Task<bool> CheckEmailExist(string email);

        Task<User> GetUserDetail(int userId);

        Task<long> GetUserIdByEmail(string email);

        Task<User> Login(string email, string password);

        Task<bool> Register(RegisterViewModel registerViewModel);

        Task<bool> ResetPassword(ResetPasswordViewModel objResetPassword);

        Task<bool> UpdateForgotPasswordCode(string forgotCode, string email);
    }
}