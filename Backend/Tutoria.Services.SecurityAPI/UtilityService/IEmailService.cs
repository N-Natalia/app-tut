using Tutoria.Services.SecurityAPI.Models;

namespace Tutoria.Services.SecurityAPI.UtilityService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
