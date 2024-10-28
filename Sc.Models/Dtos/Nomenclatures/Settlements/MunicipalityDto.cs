using Sc.Models.Dtos.Base.Nomenclatures;

namespace Sc.Models.Dtos.Nomenclatures.Settlements
{
    public class MunicipalityDto : NomenclatureDto
    {
        public string SettlementName { get; set; }
        public int DistrictId { get; set; }
        public DistrictDto District { get; set; }
    }
}
