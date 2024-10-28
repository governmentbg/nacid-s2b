using Sc.Models.Dtos.Suppliers.Search.SupplierGroup.Base;

namespace Sc.Models.Dtos.Suppliers.Search.SupplierGroup
{
    public class SupplierRootGroupDto : BaseSupplierGroupDto
    {
        public List<SupplierSubordinateGroupDto> SupplierSubordinateGroup { get; set; } = new List<SupplierSubordinateGroupDto>();
    }
}
