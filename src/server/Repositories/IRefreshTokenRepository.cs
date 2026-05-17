public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    void Add(RefreshToken refreshToken);
    void Revoke(RefreshToken refreshToken);
}