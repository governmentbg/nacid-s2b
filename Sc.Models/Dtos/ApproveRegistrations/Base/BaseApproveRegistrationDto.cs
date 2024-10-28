using Sc.Models.Dtos.Auth;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Base;
using Sc.Models.Enums.State;

namespace Sc.Models.Dtos.ApproveRegistrations.Base
{
    public abstract class BaseApproveRegistrationDto : Entity
    {
        public DateTime CreateDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int AdministratedUserId { get; set; }

        public string AdministratedUsername { get; set; }

        public ApproveRegistrationState State { get; set; }

        public string DeclinedNote { get; set; }

        public SignUpDto SignUpDto { get; set; }

        public int? SupplierId { get; set; }
        public SupplierDto Supplier { get; set; }
    }
}
