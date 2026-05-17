using server.DTOs;
using server.Models;
using server.Repositories;
using BC = BCrypt.Net.BCrypt;


public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
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

        _unitOfWork.Users.CreateUser(user);
        await _unitOfWork.SaveChangesAsync();

        var verificationToken = new EmailVerificationToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        _unitOfWork.EmailVerificationTokens.Add(verificationToken);
        await _unitOfWork.SaveChangesAsync();

        var verificationLink = $"https://yourfrontend.com/verify-email?token={verificationToken.Token}";
        var emailBody = $"<p>Hi {user.FullName},</p><p>Please click the link below to verify your email address:</p><p><a href='{verificationLink}'>Verify Email</a></p><p>This link will expire in 24 hours.</p>";
        await _emailService.SendEmailAsync(user.Email, "Verify your email", emailBody);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetUserByEmailAsync(request.Email);
        if (user == null || !BC.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password");
        }

        return new LoginResponse
        {
            AccessToken = _jwtService.GenerateAccessToken(user),
            RefreshToken = await _jwtService.GenerateRefreshToken(user),
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

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        var tokenHash = _jwtService.HashToken(refreshToken);
        var existingToken = await _unitOfWork.RefreshTokens.GetByTokenHashAsync(tokenHash);
        if (existingToken == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }
        existingToken.isRevoked = true;
        _unitOfWork.RefreshTokens.Revoke(existingToken);

        var user = existingToken.User;
        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = await _jwtService.GenerateRefreshToken(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
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