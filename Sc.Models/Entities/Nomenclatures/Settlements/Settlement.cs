using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;

namespace Sc.Models.Entities.Nomenclatures.Settlements
{
    public class Settlement : Nomenclature
    {
        public int MunicipalityId { get; set; }
        [Skip]
        public Municipality Municipality { get; set; }
        public int DistrictId { get; set; }
        [Skip]
        public District District { get; set; }
        public string MunicipalityCode { get; set; }
        public string DistrictCode { get; set; }
        public string MunicipalityCode2 { get; set; }
        public string DistrictCode2 { get; set; }
        public string TypeName { get; set; }
        public string SettlementName { get; set; }
        public string TypeCode { get; set; }
        public string MayoraltyCode { get; set; }
        public string Category { get; set; }
        public string Altitude { get; set; }
        public bool IsDistrict { get; set; }
    }
}
