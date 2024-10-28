using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Nomenclatures.Institutions;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Institutions
{
    public class InstitutionRepository : RepositoryBase<Institution, InstitutionFilterDto>, IInstitutionRepository
    {
        public InstitutionRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Institution>, IQueryable<Institution>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Root)
                    .Include(s => s.Parent)
                    .Include(s => s.District)
                    .Include(s => s.Municipality)
                    .Include(s => s.Settlement)
                    .Include(s => s.Children),
                IncludeType.Collections => e => e,
                IncludeType.NavProperties => e => e
                    .Include(s => s.Root)
                    .Include(s => s.Parent)
                    .Include(s => s.District)
                    .Include(s => s.Municipality)
                    .Include(s => s.Settlement)
                    .Include(s => s.Children),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
