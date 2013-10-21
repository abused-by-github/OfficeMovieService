using System.Linq;
using Svitla.MovieService.Mailing.Core;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.MailingApi;
using Svitla.MovieService.MailingApi.DataObjects;

namespace Svitla.MovieService.Mailing.Emails
{
    public class PollResultEmail : BaseEmail<PollResultEmailModel>, IPollResultEmail
    {
        private readonly Templates.PollResultEmail template;

        public PollResultEmail(MailingContext context, IEmailClient client, EmailConfig emailConfig)
            : base(context, client, emailConfig)
        {
            template = new Templates.PollResultEmail();
            template.Helper = new T4Helper(Context.Timing, emailConfig.WebAppUrl);
        }

        private string subject;
        protected override string Subject
        {
            get
            {
                return subject;
            }
        }

        protected override string Body
        {
            get
            {
                return template.TransformText();
            }
        }

        public override void Bind(PollResultEmailModel model)
        {
            template.Poll = model.Poll;
            var userVotes = model.Target.Votes.Where(v => v.Poll == model.Poll).Select(v => v.Movie).ToList();
            if (!userVotes.Contains(model.Poll.Winner))
            {
                template.UserProposition = userVotes;
            }
            subject = "We watch movie at " + template.Helper.FormatDateTime(template.Poll.ViewDate.Value);
        }
    }
}
