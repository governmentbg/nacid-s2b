using Sc.Models.Entities.Base;

namespace Sc.Models.Entities.Notifications
{
    public class BaseCommunication : EntityVersion
    {
        public int EntityId { get; set; }

        public DateTime CreateDate { get; set; }

        public int FromUserId { get; set; }
        public string FromUsername { get; set; }
        public string FromFullname { get; set; }

        public string Text { get; set; }
    }
}
