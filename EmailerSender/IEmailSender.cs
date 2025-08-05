namespace E_PRESCRIBING_SYSTEM.EmailerSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = true);
    }
}
