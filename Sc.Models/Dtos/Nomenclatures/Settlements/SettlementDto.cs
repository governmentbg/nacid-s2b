using Sc.Models.Dtos.Base.Nomenclatures;

namespace Sc.Models.Dtos.Nomenclatures.Settlements
{
    public class SettlementDto : NomenclatureDto
    {
        public int DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public int MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
    }
}
