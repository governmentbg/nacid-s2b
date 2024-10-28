using Sc.Models.Entities.Companies;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Companies
{
    public class CompanyRepresentativeFilterDto : FilterDto<CompanyRepresentative>
    {
        public int? CompanyId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public override IQueryable<CompanyRepresentative> WhereBuilder(IQueryable<CompanyRepresentative> query)
        {
            if (CompanyId.HasValue)
            {
                query = query.Where(e => e.Id == CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var userName = UserName.Trim().ToLower();
                query = query.Where(e => e.UserName.Trim().ToLower() == userName);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Name.Trim().ToLower().Contains(name) || e.NameAlt.Trim().ToLower().Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                var phoneNumber = PhoneNumber.Trim().ToLower();
                query = query.Where(e => e.PhoneNumber.Trim().ToLower() == phoneNumber);
            }

            query = ConstructTextFilter(query);

            return query;
        }

        public virtual IQueryable<CompanyRepresentative> ConstructTextFilter(IQueryable<CompanyRepresentative> query)
        {
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => (e.Name.Trim().ToLower() + " " + e.UserName.Trim().ToLower()).Contains(textFilter)
                    || (e.NameAlt.Trim().ToLower() + " " + e.UserName.Trim().ToLower()).Contains(textFilter));
            }
            return query;
        }
    }
}
