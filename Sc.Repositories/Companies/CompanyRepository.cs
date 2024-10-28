using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Companies;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Companies;
using Sc.Repositories.Base;

namespace Sc.Repositories.Companies
{
    public class CompanyRepository : RepositoryBase<Company, CompanyFilterDto>, ICompanyRepository
    {
        public CompanyRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Company>, IQueryable<Company>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.District)
                    .Include(s => s.Municipality)
                    .Include(s => s.Settlement)
                    .Include(s => s.LawForm)
                    .Include(s => s.CompanyAdditional),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.District)
                    .Include(s => s.Municipality)
                    .Include(s => s.Settlement)
                    .Include(s => s.LawForm),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
