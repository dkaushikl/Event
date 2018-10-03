namespace Event.Repository
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Event.Common;
    using Event.Core;
    using Event.Data;
    using Event.Repository.Interface;

    public class UserRepository : IUserRepository
    {
        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> CheckEmailExist(string email)
        {
            return await this.entities.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        public async Task<long> GetUserIdByEmail(string email)
        {
            return await this.entities.Users.Where(x => string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<User> Login(string email, string password)
        {
            return await this.entities.Users.FirstOrDefaultAsync(
                       x => string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase) && x.Password == password);
        }

        public async Task<bool> Register(AccountViewModel accountViewModel)
        {
            var objUser = new User
            {
                Email = accountViewModel.Email,
                Password = EncryptDecrypt.Encrypt(accountViewModel.Password),
                CreateDate = DateTime.Now
            };
            this.entities.Users.Add(objUser);
            await this.entities.SaveChangesAsync();
            return true;
        }
    }
}