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
            return await this.entities.Users.Where(x => x.Email.ToLower() == email.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<User> Login(string email, string password)
        {
            return await this.entities.Users.FirstOrDefaultAsync(
                       x => x.Email.ToLower() == email.ToLower() && x.Password == password);
        }

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            var objUser = new User
            {
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                Email = registerViewModel.Email,
                Password = EncryptDecrypt.Encrypt(registerViewModel.Password),
                Mobile = registerViewModel.Mobile,
                CreateDate = DateTime.Now
            };
            this.entities.Users.Add(objUser);
            await this.entities.SaveChangesAsync();
            return true;
        }
    }
}