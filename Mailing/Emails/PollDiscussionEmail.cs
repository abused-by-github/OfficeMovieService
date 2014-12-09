using MovieService.Core.Entities;
using MovieService.Mailing.Core;
using MovieService.MailingApi;

namespace MovieService.Mailing.Emails
{
    public class PollDiscussionEmail : BaseEmail<Vote>, IPollDiscussionEmail
    {
        private readonly Templates.PollDiscussionEmail template;

        public PollDiscussionEmail(MailingContext context, IEmailClient client, EmailConfig emailConfig)
            : base(context, client, emailConfig)
        {
            template = new Templates.PollDiscussionEmail();
            template.Helper = new T4Helper(Context.Timing, emailConfig.WebAppUrl);
        }

        protected override string Subject
        {
            get
            {
                var date = template.Helper.FormatDateTime(template.Poll.DiscussionDate.Value);
                return string.Format("\"{0}\" discussion at {1}", template.Poll.Winner.Name, date);
            }
        }

        protected override string Body
        {
            get
            {
                return template.TransformText();
            }
        }

        public override void Bind(Vote model)
        {
            template.Poll = model.Poll;
            template.User = model.User;
            template.IsOwnProposition = model.Poll.Winner.User == model.User;
        }
    }
}
