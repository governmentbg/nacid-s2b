using AutoMapper;
using Infrastructure.DomainValidation;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Base;

namespace Sc.Services.Suppliers
{
    public class SupplierService : ValidateService<Supplier, SupplierDto>
    {
        private readonly IMapper mapper;
        private readonly ISupplierRepository supplierRepository;

        public SupplierService(
            IMapper mapper,
            ISupplierRepository supplierRepository,
            DomainValidatorService domainValidatorService)
            : base(domainValidatorService)
        {
            this.mapper = mapper;
            this.supplierRepository = supplierRepository;
        }

        public async Task<SupplierDto> GetById(int id, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetById(id, cancellationToken, supplierRepository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<SupplierDto>(supplier);
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(SupplierFilterDto filterDto, IncludeType includeType, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await supplierRepository.GetAll(filterDto, cancellationToken, supplierRepository.ConstructInclude(includeType), e => e.OrderBy(e => e.Institution.Name).ThenBy(e => e.Complex.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<SupplierDto> Create(SupplierDto supplierDto, CancellationToken cancellationToken)
        {
            supplierDto.ValidateProperties(domainValidatorService);

            var newSupplier = mapper.Map<Supplier>(supplierDto);

            await supplierRepository.Create(newSupplier);

            return mapper.Map<SupplierDto>(await supplierRepository.GetById(newSupplier.Id, cancellationToken));
        }

        public async Task Delete(Supplier supplierTeamForDelete)
        {
            await supplierRepository.Delete(supplierTeamForDelete);
        }
    }
}
