using System.Collections.Generic;
using System.Linq;

namespace Svitla.MovieService.Core.Entities.EmailQueue
{
    public class Email : Entity
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public virtual ICollection<Recipient> Recipients { get; set; }

        public Recipient From
        {
            get
            {
                return Recipients.FirstOrDefault(r => r.Role == RecipientRole.From);
            }
        }

        public IEnumerable<Recipient> To
        {
            get
            {
                return Recipients.Where(r => r.Role == RecipientRole.To);
            }
        }

        public IEnumerable<Recipient> Cc
        {
            get
            {
                return Recipients.Where(r => r.Role == RecipientRole.Cc);
            }
        }

        public IEnumerable<Recipient> Bcc
        {
            get
            {
                return Recipients.Where(r => r.Role == RecipientRole.Bcc);
            }
        }

        public void SetFrom(string name, string email)
        {
            Recipients.Remove(From);
            Recipients.Add(new Recipient { Email = email, Name = name });
        }

        public void AddTo(string name, string email)
        {
            Recipients.Add(new Recipient { Email = email, Name = name, Role = RecipientRole.To });
        }

        public void AddCc(string name, string email)
        {
            Recipients.Add(new Recipient { Email = email, Name = name, Role = RecipientRole.Cc });
        }

        public void AddBcc(string name, string email)
        {
            Recipients.Add(new Recipient { Email = email, Name = name, Role = RecipientRole.Bcc });
        }
    }
}
