using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures.Complexes;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Models.FilterDtos.Nomenclatures.Complexes;
using Sc.Repositories.Nomenclatures.Complexes;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.Complexes
{
    public class ComplexService : NomenclatureService<Complex, ComplexDto, ComplexFilterDto, IComplexRepository>
    {
        public ComplexService(
            IMapper mapper,
            IComplexRepository complexRepository,
            DomainValidatorService domainValidatorService
            ) : base(mapper, complexRepository, domainValidatorService)
        {
        }
    }
}
