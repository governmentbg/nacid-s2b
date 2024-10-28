using Sc.Models;
using Sc.Models.Entities.Nomenclatures;
using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures
{
    public class LawFormRepository : RepositoryBase<LawForm, LawFormFilterDto>, ILawFormRepository
    {
        public LawFormRepository(ScDbContext context)
            : base(context)
        {
        }
    }
}
