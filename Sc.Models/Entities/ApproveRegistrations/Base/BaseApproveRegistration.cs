using Sc.Models.Attributes;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.State;

namespace Sc.Models.Entities.ApproveRegistrations.Base
{
    public abstract class BaseApproveRegistration : EntityVersion
    {
        public DateTime CreateDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public int? AdministratedUserId { get; set; }

        public string AdministratedUsername { get; set; }

        public string JsonSignUpDto { get; set; }

        public ApproveRegistrationState State { get; set; }

        public string DeclinedNote { get; set; }

        public int? SupplierId { get; set; }

        [Skip]
        public Supplier Supplier { get; set; }
    }
}
