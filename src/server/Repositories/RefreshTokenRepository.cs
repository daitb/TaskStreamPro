using Microsoft.EntityFrameworkCore;
using server.Data;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash
                && rt.ExpiresAt > DateTime.UtcNow
                && !rt.isRevoked);
    }

    public void Add(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
    }

    public void Revoke(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
    }
}