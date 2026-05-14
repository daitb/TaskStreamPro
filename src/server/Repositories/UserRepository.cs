using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public System.Threading.Tasks.Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        return _context.SaveChangesAsync();
    }
}