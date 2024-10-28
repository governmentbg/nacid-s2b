using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;

namespace Sc.Models.Entities.Nomenclatures.Settlements
{
    public class Municipality : Nomenclature
    {
        public int DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public string Code2 { get; set; }
        public string MainSettlementCode { get; set; }
        public string Category { get; set; }
    }
}
