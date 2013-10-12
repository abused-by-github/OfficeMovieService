namespace Svitla.MovieService.Mailing.Core.Client
{
    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}
