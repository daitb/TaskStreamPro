using server.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IEmailVerificationTokenRepository EmailVerificationTokens { get; }
    Task<int> SaveChangesAsync();
}