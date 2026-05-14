using server.DTOs;
using server.Models;
using server.Repositories;
using BC = BCrypt.Net.BCrypt;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly  IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if(existingUser != null)
        {
            throw new ConflictException("Email is already registered");
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = BC.HashPassword(request.Password),
            SystemRole = "user"
        };

        await _userRepository.CreateUserAsync(user);
        return new LoginResponse
        {
            Token = _jwtService.GenerateToken(user),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                SystemRole = user.SystemRole
            }
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if(user == null || !BC.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        return new LoginResponse
        {
            Token = _jwtService.GenerateToken(user),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                SystemRole = user.SystemRole
            }
        };
    }
}