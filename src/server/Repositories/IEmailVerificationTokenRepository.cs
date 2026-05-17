public interface IEmailVerificationTokenRepository
{
    Task<EmailVerificationToken?> GetByTokenAsync(string token);
    void Add(EmailVerificationToken token);
    void Remove(EmailVerificationToken token);
}