using System;
using System.Net.Mail;
using System.Net;

namespace Elide.Workbench.ExceptionHandling
{
    public sealed class DumpSender
    {
        private const string FROM = "serv.elalang@gmail.com";
        private const string TITLE = "Elide Report";
        private const string DEFAULT_HOST = "smtp.gmail.com";
        private const int DEFAULT_PORT = 587;
        
        public DumpSender()
        {
            From = FROM;
            Title = TITLE;
            Port = DEFAULT_PORT;
            SmtpHost = DEFAULT_HOST;
            EnableSsl = true;            
        }
        
        public bool SendDump(string dump)
        {
            var eml = new MailMessage(From, To, Title, dump);
            var client = new SmtpClient(SmtpHost, Port);
            client.EnableSsl = EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = Credentials;
          
            try
            {
                Error = null;
                client.Send(eml);
                return true;
            }
            catch (ArgumentException ex)
            {
                return ProcessException(ex);
            }
            catch (InvalidOperationException ex)
            {
                return ProcessException(ex);
            }
            catch (SmtpException ex)
            {
                return ProcessException(ex);
            }
        }

        private bool ProcessException(Exception ex)
        {
            Error = ex.Message;
            return false;
        }

        public string From { get; set; }

        public string To { get; set; }

        public string Title { get; set; }

        public string SmtpHost { get; set; }

        public NetworkCredential Credentials { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public string Error { get; private set; }
    }
}
