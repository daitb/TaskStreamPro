using server.Models;

namespace server.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public System.Threading.Tasks.Task CreateUserAsync(User user);
    }
}
