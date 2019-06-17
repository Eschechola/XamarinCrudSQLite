using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace App8.Email
{
    class ServicoEmail
    {
        //Classe para enviar um email para o usuario que se cadastra pela primeira vez
        public bool EnviarEmail(string _mail)
        {
            StringBuilder construtorMail = new StringBuilder();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("myfilm.contato@gmail.com");
                mail.To.Add(_mail);
                mail.Subject = "MyFilm";
                mail.IsBodyHtml = true;
                construtorMail.AppendLine("<strong>Seja bem - vindo ao MyFilm!</strong><br>");
                construtorMail.AppendLine("Leia nossos termos de contrato para utilizar nosso aplicativo!");
                mail.Body = construtorMail.ToString();
                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("myfilm.contato@gmail.com", "esc123321");
                object a = new object();
                SmtpServer.SendAsync(mail, a);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

