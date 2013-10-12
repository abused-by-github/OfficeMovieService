namespace Svitla.MovieService.MailingApi
{
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public EmailAddress(string email, string name = null)
        {
            Name = name;
            Email = email;
        }
    }
}
