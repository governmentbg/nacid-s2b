using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Integrations.SsoIntegration;
using Microsoft.AspNetCore.Http;
using Sc.Models;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Auth;
using Sc.Services.Base;

namespace Sc.Services.Suppliers
{
    public class SupplierTeamService : ValidateService<SupplierTeam, SupplierTeamDto>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ScDbContext context;
        private readonly IMapper mapper;
        private readonly SsoIntegrationService ssoIntegrationService;
        private readonly AuthService authService;
        private readonly ISupplierTeamRepository supplierTeamRepository;
        private readonly ISupplierRepresentativeRepository supplierRepresentativeRepository;
        private readonly PermissionService permissionService;

        public SupplierTeamService(
            IHttpContextAccessor httpContextAccessor,
            ScDbContext context,
            IMapper mapper,
            SsoIntegrationService ssoIntegrationService,
            AuthService authService,
            ISupplierTeamRepository supplierTeamRepository,
            ISupplierRepresentativeRepository supplierRepresentativeRepository,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService)
            : base(domainValidatorService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.mapper = mapper;
            this.ssoIntegrationService = ssoIntegrationService;
            this.authService = authService;
            this.supplierTeamRepository = supplierTeamRepository;
            this.supplierRepresentativeRepository = supplierRepresentativeRepository;
            this.permissionService = permissionService;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(SupplierTeamFilterDto filterDto, IncludeType includeType, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await supplierTeamRepository.GetAll(filterDto, cancellationToken, supplierTeamRepository.ConstructInclude(includeType), e => e.OrderBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<SupplierTeamDto> GetDtoById(int id, CancellationToken cancellationToken)
        {
            var supplierTeam = await supplierTeamRepository.GetById(id, cancellationToken, supplierTeamRepository.ConstructInclude(IncludeType.Collections));

            return mapper.Map<SupplierTeamDto>(supplierTeam);
        }

        public async Task<List<SupplierTeamDto>> GetBySupplierDtos(int supplierId, SupplierDto supplierDto, CancellationToken cancellationToken)
        {
            var supplierTeamFilterDto = new SupplierTeamFilterDto
            {
                GetAllData = true,
                SupplierId = supplierId
            };

            supplierTeamFilterDto.SetIsActive(permissionService, supplierDto);

            var teamList = await supplierTeamRepository.GetList(supplierTeamFilterDto, cancellationToken, supplierTeamRepository.ConstructInclude(IncludeType.Collections), e => e.OrderBy(e => e.Name));

            return mapper.Map<List<SupplierTeamDto>>(teamList);
        }

        public async Task<SupplierTeamDto> Create(SupplierDto supplierDto, SupplierTeamDto supplierTeamDto, CancellationToken cancellationToken)
        {
            using var transaction = context.BeginTransaction();

            SetRasPortalUrl(supplierTeamDto);
            supplierTeamDto.SupplierId = supplierDto.Id;
            supplierTeamDto.ValidateProperties(domainValidatorService);

            if (supplierTeamDto.UserName != supplierTeamDto.UserNameAgain)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_UserName_DoesNotMatch);
            }

            await CreateValidation(supplierTeamDto, cancellationToken);

            var ssoSignUpDto = authService.ConstructSsoSignUpTeamDto(supplierTeamDto, supplierDto, cancellationToken);
            var ssoUserDto = await ssoIntegrationService.SignUp(ssoSignUpDto, httpContextAccessor.HttpContext);

            var newSupplierTeam = mapper.Map<SupplierTeam>(supplierTeamDto);
            newSupplierTeam.IsActive = true;
            newSupplierTeam.UserId = ssoUserDto.Id;

            await supplierTeamRepository.Create(newSupplierTeam);

            await transaction.CommitAsync();

            return mapper.Map<SupplierTeamDto>(await supplierTeamRepository.GetById(newSupplierTeam.Id, cancellationToken));
        }

        public async Task<SupplierTeamDto> Update(SupplierTeam supplierTeamForUpdate, SupplierTeamDto supplierTeamDto, CancellationToken cancellationToken)
        {
            SetRasPortalUrl(supplierTeamDto);

            supplierTeamDto.ValidateProperties(domainValidatorService);
            await UpdateValidation(supplierTeamForUpdate, supplierTeamDto, cancellationToken);

            await supplierTeamRepository.UpdateFromDto(supplierTeamForUpdate, supplierTeamDto);

            return await GetDtoById(supplierTeamForUpdate.Id, cancellationToken);
        }

        public async Task<bool> ChangeIsActive(SupplierTeam supplierTeamForUpdate, IsActiveDto isActiveDto)
        {
            await supplierTeamRepository.UpdateFromDto(supplierTeamForUpdate, isActiveDto);

            return supplierTeamForUpdate.IsActive;
        }

        public async Task Delete(SupplierTeam supplierTeamForDelete)
        {
            await supplierTeamRepository.Delete(supplierTeamForDelete);
        }

        private void SetRasPortalUrl(SupplierTeamDto supplierTeamDto)
        {
            if (supplierTeamDto.RasLotId.HasValue)
            {
                supplierTeamDto.RasPortalUrl = $"{AppSettingsProvider.RasIntegration.RasPortalUri}dissertation-preview/{supplierTeamDto.RasLotId}";
            }
            else
            {
                supplierTeamDto.RasPortalUrl = null;
            }
        }

        #region IValidationService
        protected async override Task CreateValidation(SupplierTeamDto dto, CancellationToken cancellationToken)
        {
            if (await supplierTeamRepository.AnyEntity(e => e.UserName == dto.UserName && e.SupplierId == dto.SupplierId, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Entity_Exists);
            }

            if (await supplierRepresentativeRepository.AnyEntity(e => e.UserName == dto.UserName && e.Id == dto.SupplierId, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Representative_CannotBeInTeam);
            }
        }

        protected async override Task UpdateValidation(SupplierTeam supplierTeam, SupplierTeamDto dto, CancellationToken cancellationToken)
        {
            if (await supplierTeamRepository.AnyEntity(e => e.UserName == dto.UserName
                && e.SupplierId == dto.SupplierId
                && e.Id != dto.Id, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Entity_Exists);
            }

            if (await supplierRepresentativeRepository.AnyEntity(e => e.UserName == dto.UserName
                && e.Id == dto.SupplierId, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Representative_CannotBeInTeam);
            }

            if (supplierTeam.SupplierId != dto.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Entity_CannotChangeSupplierId);
            }

        }
        #endregion
    }
}
