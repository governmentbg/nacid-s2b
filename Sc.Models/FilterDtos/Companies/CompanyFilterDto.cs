using Sc.Models.Entities.Companies;
using Sc.Models.Enums.Companies;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Companies
{
    public class CompanyFilterDto : FilterDto<Company>
    {
        public CompanyType? Type { get; set; }
        public string Uic { get; set; }
        public int? LawFormId { get; set; }
        public string Name { get; set; }
        public int? DistrictId { get; set; }
        public int? MunicipalityId { get; set; }
        public int? SettlementId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public override IQueryable<Company> WhereBuilder(IQueryable<Company> query)
        {
            if (Type.HasValue)
            {
                query = query.Where(e => e.Type == Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(Uic))
            {
                var uic = Uic.Trim().ToLower();
                query = query.Where(e => e.Uic.Trim().ToLower() == uic);
            }

            if (LawFormId.HasValue)
            {
                query = query.Where(e => e.LawFormId == LawFormId);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Name.Trim().ToLower().Contains(name) || e.NameAlt.Trim().ToLower().Contains(name));
            }

            if (DistrictId.HasValue)
            {
                query = query.Where(e => e.DistrictId == DistrictId);
            }

            if (MunicipalityId.HasValue)
            {
                query = query.Where(e => e.MunicipalityId == MunicipalityId);
            }

            if (SettlementId.HasValue)
            {
                query = query.Where(e => e.SettlementId == SettlementId);
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var email = Email.Trim().ToLower();
                query = query.Where(e => e.Email.Trim().ToLower().Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                var phoneNumber = PhoneNumber.Trim().ToLower();
                query = query.Where(e => e.PhoneNumber.Trim().ToLower().Contains(phoneNumber));
            }

            query = ConstructTextFilter(query);

            return query;
        }

        public virtual IQueryable<Company> ConstructTextFilter(IQueryable<Company> query)
        {
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => e.Name.Trim().ToLower().Contains(textFilter));
            }

            return query;
        }
    }
}
