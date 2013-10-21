using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Mailing.Core;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Emails
{
    public class InviteEmail : BaseEmail<User>, IInviteEmail
    {
        private readonly Templates.InviteEmail template;

        public InviteEmail(MailingContext context, IEmailClient client, EmailConfig config)
            : base(context, client, config)
        {
            template = new Templates.InviteEmail();
            template.Helper = new T4Helper(Context.Timing, EmailConfig.WebAppUrl);
        }

        public override void Bind(User model)
        {
            template.User = model;
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
