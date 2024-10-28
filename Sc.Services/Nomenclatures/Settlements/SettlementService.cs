using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Nomenclatures.Settlements;
using Sc.Models.Entities.Nomenclatures.Settlements;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Nomenclatures.Settlements;
using Sc.Services.Base.Nomenclatures;

namespace Sc.Services.Nomenclatures.Settlements
{
    public class SettlementService : NomenclatureService<Settlement, SettlementDto, SettlementFilterDto, ISettlementRepository>
    {
        public SettlementService(
            IMapper mapper,
            ISettlementRepository settlementRepository,
            DomainValidatorService domainValidatorService
            ) : base(mapper, settlementRepository, domainValidatorService)
        {
        }
    }
}
