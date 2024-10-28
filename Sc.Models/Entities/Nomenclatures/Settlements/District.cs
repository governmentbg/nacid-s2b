using Sc.Models.Attributes;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Settlements;

namespace Sc.Models.Entities.Nomenclatures.Settlements
{
    public class District : Nomenclature
    {
        public RegionType Region { get; set; }
        public string Code2 { get; set; }
        public string SecondLevelRegionCode { get; set; }
        public string MainSettlementCode { get; set; }

        [Skip]
        public List<Institution> Institutions { get; set; } = new List<Institution>();
        [Skip]
        public List<Complex> Complexes { get; set; } = new List<Complex>();
    }
}
