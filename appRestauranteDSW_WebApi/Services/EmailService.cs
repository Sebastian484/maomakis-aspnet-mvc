using System.Net.Mail;
using System.Net;

namespace appRestauranteDSW_WebApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationEmail(string to, string token)
        {
            var from = _config["Email:From"];
            var pass = _config["Email:Password"];
            var host = "smtp.gmail.com";
            var port = 587;

            var link = $"https://localhost:7296/api/auth/verify?token={token}";

            var message = new MailMessage(from, to)
            {
                Subject = "Verificación de cuenta",
                Body = $"Haz clic en este enlace para verificar tu cuenta: {link}",
                IsBodyHtml = true
            };

            using var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(from, pass),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
