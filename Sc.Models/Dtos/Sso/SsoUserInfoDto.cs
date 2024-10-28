using Sc.Models.Dtos.Nomenclatures.Settlements;

namespace Sc.Models.Dtos.Sso
{
    public class SsoUserInfoDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int? DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public int? MunicipalityId { get; set; }
        public MunicipalityDto Municipality { get; set; }
        public int? SettlementId { get; set; }
        public SettlementDto Settlement { get; set; }
        public string Address { get; set; }
    }
}
