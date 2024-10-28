using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Nomenclatures.Complexes;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Complexes
{
    public class ComplexRepository : RepositoryBase<Complex, ComplexFilterDto>, IComplexRepository
    {
        public ComplexRepository(ScDbContext context)
            : base(context)
        {
        }

        public override Func<IQueryable<Complex>, IQueryable<Complex>> ConstructInclude(IncludeType includeType = IncludeType.None)
        {
            return includeType switch
            {
                IncludeType.All => e => e
                    .Include(s => s.Settlement)
                    .Include(s => s.District)
                    .Include(s => s.Municipality)
                    .Include(s => s.ComplexOrganizations),
                IncludeType.Collections => e => e
                    .Include(s => s.ComplexOrganizations),
                IncludeType.NavProperties => e => e
                    .Include(s => s.Settlement)
                    .Include(s => s.District)
                    .Include(s => s.Municipality),
                IncludeType.None => e => e,
                _ => e => e,
            };
        }
    }
}
