using Sc.Models.Entities.Nomenclatures;
using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures
{
    public interface ILawFormRepository : IRepositoryBase<LawForm, LawFormFilterDto>
    {
    }
}
