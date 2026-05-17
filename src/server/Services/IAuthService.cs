
public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    public Task<LoginResponse> RefreshTokenAsync(string refreshToken);
}