using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApiPyme.Service
{
    public class EmailService
    {
        private readonly string smtpHost = "smtp.gmail.com"; // Servidor SMTP, por ejemplo, Gmail
        private readonly int smtpPort = 587; // Puerto para SMTP con STARTTLS
        private readonly string smtpUser = "gaby.1994.qp@gmail.com"; // Tu dirección de correo
        private readonly string smtpPass = "qbgd mclv yoef zxbp"; // Contraseña o App Password

        public string sendMail(string to, string asunto, string body)
        {
            string msge = "Error al enviar este correo. Por favor verifique los datos o intente más tarde.";
            string from = "gaby.1994.qp@gmail.com";
            string displayName = "PYME";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add(to);

                mail.Subject = asunto;
                mail.Body = body;
                mail.IsBodyHtml = true;


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Aquí debes sustituir tu servidor SMTP y el puerto
                client.Credentials = new NetworkCredential(from, smtpPass);
                client.EnableSsl = true;//En caso de que tu servidor de correo no utilice cifrado SSL,poner en false


                client.Send(mail);
                msge = "¡Correo enviado exitosamente! Pronto te contactaremos.";

            }
            catch (Exception ex)
            {
                throw;
            }

            return msge;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = true; // Asegúrate de que SSL esté habilitado para seguridad

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(smtpUser, "PRUEBA CORREO"),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = true // Define si el cuerpo del correo soporta HTML
                    };
                    mensaje.To.Add(destinatario);

                    await client.SendMailAsync(mensaje);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                throw; // Opcional: manejar la excepción según sea necesario
            }
        }
    }
}
