using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures;
using Sc.Models.Entities.Nomenclatures;
using Sc.Models.FilterDtos.Nomenclatures.LawForms;
using Sc.Repositories.Nomenclatures;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.LawForms
{
    public class LawFormService : NomenclatureService<LawForm, LawFormDto, LawFormFilterDto, ILawFormRepository>
    {
        public LawFormService(
            IMapper mapper,
            ILawFormRepository lawFormRepository,
            DomainValidatorService domainValidatorService
            ) :base(mapper, lawFormRepository, domainValidatorService)
        {
        }
    }
}
