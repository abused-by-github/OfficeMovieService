namespace Svitla.MovieService.Mailing.Emails
{
    class T4Helper
    {
        private readonly string webAppUrl;

        public T4Helper(string webAppUrl)
        {
            this.webAppUrl = webAppUrl;
        }

        public string WebAppUrl(string relative)
        {
            return webAppUrl + "/" + relative;
        }
    }
}
