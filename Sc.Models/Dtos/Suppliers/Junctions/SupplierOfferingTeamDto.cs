using Sc.Models.Entities.Base;

namespace Sc.Models.Dtos.Suppliers.Junctions
{
    public class SupplierOfferingTeamDto : Entity
    {
        public int SupplierOfferingId { get; set; }
        public SupplierOfferingDto SupplierOffering { get; set; }

        public int SupplierTeamId { get; set; }
        public SupplierTeamDto SupplierTeam { get; set; }
    }
}
