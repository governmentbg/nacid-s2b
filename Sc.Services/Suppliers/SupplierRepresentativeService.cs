using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Repositories.Suppliers;
using Sc.Services.Base;

namespace Sc.Services.Suppliers
{
    public class SupplierRepresentativeService : ValidateService<SupplierRepresentative, SupplierRepresentativeDto>
    {
        private readonly IMapper mapper;
        private readonly ISupplierRepresentativeRepository supplierRepresentativeRepository;

        public SupplierRepresentativeService(
            IMapper mapper,
            ISupplierRepresentativeRepository supplierRepresentativeRepository,
            DomainValidatorService domainValidatorService)
            : base(domainValidatorService)
        {
            this.mapper = mapper;
            this.supplierRepresentativeRepository = supplierRepresentativeRepository;
        }

        public async Task<SupplierRepresentativeDto> GetDtoById(int supplierId, CancellationToken cancellationToken)
        {
            var supplierRepresentative = await supplierRepresentativeRepository.GetById(supplierId, cancellationToken, supplierRepresentativeRepository.ConstructInclude(IncludeType.Collections));

            return mapper.Map<SupplierRepresentativeDto>(supplierRepresentative);
        }

        public async Task<SupplierRepresentativeDto> Create(int supplierId, SupplierRepresentativeDto supplierRepresentativeDto, CancellationToken cancellationToken)
        {
            supplierRepresentativeDto.Id = supplierId;
            supplierRepresentativeDto.ValidateProperties(domainValidatorService);
            await CreateValidation(supplierRepresentativeDto, cancellationToken);

            var newSupplierRepresentative = mapper.Map<SupplierRepresentative>(supplierRepresentativeDto);

            await supplierRepresentativeRepository.Create(newSupplierRepresentative);

            return mapper.Map<SupplierRepresentativeDto>(await supplierRepresentativeRepository.GetById(newSupplierRepresentative.Id, cancellationToken));
        }

        public async Task<SupplierRepresentativeDto> Update(SupplierRepresentative supplierRepresentativeForUpdate, SupplierRepresentativeDto supplierRepresentativeDto, CancellationToken cancellationToken)
        {
            supplierRepresentativeDto.ValidateProperties(domainValidatorService);

            await supplierRepresentativeRepository.UpdateFromDto(supplierRepresentativeForUpdate, supplierRepresentativeDto);

            return await GetDtoById(supplierRepresentativeForUpdate.Id, cancellationToken);
        }

        public async Task<bool> Exists(int supplierId, CancellationToken cancellationToken)
        {
            return await supplierRepresentativeRepository.AnyEntity(e => e.Id == supplierId, cancellationToken);
        }

        #region IValidationService
        protected async override Task CreateValidation(SupplierRepresentativeDto dto, CancellationToken cancellationToken)
        {
            if (await Exists(dto.Id, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(SupplierRepresentativeErrorCode.SupplierRepresentative_Entity_Exists);
            }
        }
        #endregion
    }
}
