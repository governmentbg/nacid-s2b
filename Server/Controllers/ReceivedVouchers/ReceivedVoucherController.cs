using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;
using Sc.Services.ReceivedVouchers;
using Sc.Services.ReceivedVouchers.Permissions;

namespace Server.Controllers.ReceivedVouchers
{
    [ApiController]
    [Route("api/receivedVouchers")]
    public class ReceivedVoucherController : ControllerBase
    {
        private readonly IReceivedVoucherRepository receivedVoucherRepository;
        private readonly ReceivedVoucherService receivedVoucherService;
        private readonly PermissionService permissionService;
        private readonly ReceivedVoucherPermissionService receivedVoucherPermissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly UserContext userContext;

        public ReceivedVoucherController(
            IReceivedVoucherRepository receivedVoucherRepository,
            ReceivedVoucherService receivedVoucherService,
            PermissionService permissionService,
            ReceivedVoucherPermissionService receivedVoucherPermissionService,
            DomainValidatorService domainValidatorService,
            UserContext userContext
           )
        {
            this.receivedVoucherRepository = receivedVoucherRepository;
            this.receivedVoucherService = receivedVoucherService;
            this.permissionService = permissionService;
            this.receivedVoucherPermissionService = receivedVoucherPermissionService;
            this.domainValidatorService = domainValidatorService;
            this.userContext = userContext;
        }

        [Authorize, ScClient]
        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<ReceivedVoucherDto>>> GetSearchResultDto([FromBody] ReceivedVoucherFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.ReceivedVoucherReadPermission);

            if (userContext.CompanyId.HasValue && userContext.CompanyId > 0)
            {
                filter.CompanyId = userContext.CompanyId;
            }
            else if (userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                filter.AuthorizedRepresentativeUserId = userContext.UserId;
            }
            else if (!userContext.OrganizationalUnits.Any(e => e.Alias == OrganizationalUnitConstants.NacidAlias || e.Alias == OrganizationalUnitConstants.PniiditAlias))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await receivedVoucherService.GetSearchResultDto<ReceivedVoucherDto>(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReceivedVoucherDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var receivedVoucherDto = await receivedVoucherService.GetById(id, IncludeType.All, cancellationToken);

            await receivedVoucherPermissionService.VerifyReceivedVoucherPermissionException(PermissionConstants.ReceivedVoucherReadPermission, receivedVoucherDto, cancellationToken);

            return Ok(receivedVoucherDto);
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ReceivedVoucherDto newReceivedVoucherDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            if (!userContext.UserId.HasValue || !userContext.CompanyId.HasValue || userContext.CompanyId < 1)
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            newReceivedVoucherDto.CompanyId = userContext.CompanyId.Value;
            newReceivedVoucherDto.CompanyUserId = userContext.UserId.Value;

            permissionService.VerifyUnitPermissionException(PermissionConstants.ReceivedVoucherCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, newReceivedVoucherDto.CompanyId) });

            await receivedVoucherService.Create(newReceivedVoucherDto, cancellationToken);

            return Ok();
        }

        [Authorize, ScClient]
        [HttpPut]
        public async Task<ActionResult<ReceivedVoucherDto>> Update([FromBody] ReceivedVoucherDto receivedVoucherDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var receivedVoucherForUpdate = await receivedVoucherRepository.GetById(receivedVoucherDto.Id, cancellationToken, receivedVoucherRepository.ConstructInclude(IncludeType.NavProperties));

            permissionService.VerifyUnitPermissionException(PermissionConstants.ReceivedVoucherWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, receivedVoucherForUpdate.CompanyId) });

            return Ok(await receivedVoucherService.Update(receivedVoucherForUpdate, receivedVoucherDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpGet("{id}/terminate")]
        public async Task<ActionResult<ReceivedVoucherDto>> TerminateVoucher([FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var receivedVoucherDto = await receivedVoucherService.GetById(id, IncludeType.All, cancellationToken);

            receivedVoucherDto.ValidateProperties(domainValidatorService);

            permissionService.VerifyUnitPermissionException(PermissionConstants.ReceivedVoucherDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, receivedVoucherDto.CompanyId), (OrganizationalUnitConstants.NacidAlias, null), (OrganizationalUnitConstants.PniiditAlias, null) });

            return Ok(await receivedVoucherService.TerminateVoucher(id, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetReceivedVoucherCount(CancellationToken cancellationToken)
        {
            if (!userContext.UserId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            var count = await receivedVoucherService.GetReceivedVoucherCountAsync(userContext.UserId.Value, cancellationToken);
            return Ok(count);
        }

    }
}
