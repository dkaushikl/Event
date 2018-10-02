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

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            var objUser = new User
            {
                Email = registerViewModel.Email,
                Password = EncryptDecrypt.Encrypt(registerViewModel.Password),
                CreateDate = DateTime.Now
            };
            this.entities.Users.Add(objUser);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<long> GetUserIdByEmail(string email)
        {
            return await this.entities.Users.Where(x => x.Email == email.ToLower()).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}