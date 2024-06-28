namespace SISDEN.Models
{
    public interface IEmailValidacion
    {
        Task SendEmailAsync(string email, string subject, string message);

    }
}
