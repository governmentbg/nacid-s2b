using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.FilterDtos.Nomenclatures.Complexes;
using Sc.Repositories.Base;

namespace Sc.Repositories.Nomenclatures.Complexes
{
    public interface IComplexRepository : IRepositoryBase<Complex, ComplexFilterDto>
    {
    }
}
