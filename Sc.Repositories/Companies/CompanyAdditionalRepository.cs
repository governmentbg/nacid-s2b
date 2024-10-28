using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Companies;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Companies;
using Sc.Repositories.Base;

namespace Sc.Repositories.Companies
{
    public class CompanyAdditionalRepository : RepositoryBase<CompanyAdditional, CompanyAdditionalFilterDto>, ICompanyAdditionalRepository
    {
        public CompanyAdditionalRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<CompanyAdditional>, IQueryable<CompanyAdditional>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Company),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e,
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
