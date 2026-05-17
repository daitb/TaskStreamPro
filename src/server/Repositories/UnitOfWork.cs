using server.Data;
using server.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IUserRepository Users {get; }
    public IRefreshTokenRepository RefreshTokens {get; }
    public IEmailVerificationTokenRepository EmailVerificationTokens { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        RefreshTokens = new RefreshTokenRepository(context);
        EmailVerificationTokens = new EmailVerificationTokenRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}