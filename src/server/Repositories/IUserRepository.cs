using server.Models;

namespace server.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public void CreateUser(User user);
    }
}
