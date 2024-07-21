namespace IdentityAppPractice.Interfaces
{
    public interface ISendGridEmail
    {
        Task SendEmailAsync(string toEmail,string subject,string message);
    }
}
