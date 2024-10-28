using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.Notifications;

namespace Sc.Models.Entities.Notifications
{
    public class BaseNotification<TEntity> : EntityVersion
        where TEntity : EntityVersion
    {
        public int EntityId { get; set; }
        [Skip]
        public TEntity Entity { get; set; }

        public NotificationType Type { get; set; }

        public DateTime CreateDate { get; set; }

        public int FromUserId { get; set; }
        public string FromUsername { get; set; }
        public string FromFullname { get; set; }
        public string FromUserOrganization { get; set; }

        public int ToUserId { get; set; }

        public string Text { get; set; }
    }
}
