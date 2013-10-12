using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Mailing.Core;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Emails
{
    public class InviteEmail : BaseEmail<User>, IInviteEmail
    {
        private readonly Templates.InviteEmail template;

        public InviteEmail(IEmailClient client, EmailConfig config) : base(client, config)
        {
            template = new Templates.InviteEmail();
        }

        public override void Bind(User model)
        {
            template.User = model;
            template.Helper = new T4Helper(EmailConfig.WebAppUrl);
        }

        protected override string Body
        {
            get { return template.TransformText(); }
        }

        protected override string Subject
        {
            get { return "You are invited to SvitlaMovie club!"; }
        }
    }
}
