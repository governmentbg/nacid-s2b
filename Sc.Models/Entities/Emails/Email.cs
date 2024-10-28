using Sc.Models.Entities.Base;
using Sc.Models.Enums.Emails;

namespace Sc.Models.Entities.Emails
{
    public class Email : EntityVersion
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public EmailStatus Status { get; set; }
        public DateTime? StartSendingDate { get; set; }
        public DateTime? SentDate { get; set; }

        public Email(string subject, string body, string to)
        {
            Subject = subject;
            Body = body;
            To = to;
            Status = EmailStatus.Unsent;
        }
    }
}
