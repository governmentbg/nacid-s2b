using Infrastructure;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.VoucherRequests;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.VoucherRequests;
using Sc.Services.VoucherRequests;
using Sc.Services.VoucherRequests.Permissions;

namespace Server.Controllers.VoucherRequests
{
    [ApiController]
    [Route("api/voucherRequests")]
    public class VoucherRequestController : ControllerBase
    {
        private readonly VoucherRequestService voucherRequestService;
        private readonly IVoucherRequestRepository voucherRequestRepository;
        private readonly PermissionService permissionService;
        private readonly VoucherRequestPermissionService voucherRequestPermissionService;
        private readonly UserContext userContext;
        private readonly DomainValidatorService domainValidatorService;

        public VoucherRequestController(
           VoucherRequestService voucherRequestService,
           IVoucherRequestRepository voucherRequestRepository,
           PermissionService permissionService,
           VoucherRequestPermissionService voucherRequestPermissionService,
           UserContext userContext,
           DomainValidatorService domainValidatorService
           )
        {
            this.voucherRequestService = voucherRequestService;
            this.voucherRequestRepository = voucherRequestRepository;
            this.permissionService = permissionService;
            this.voucherRequestPermissionService = voucherRequestPermissionService;
            this.userContext = userContext;
            this.domainValidatorService = domainValidatorService;
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VoucherRequestDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var voucherRequestDto = await voucherRequestService.GetById(id, cancellationToken);

            await voucherRequestPermissionService.VerifyVoucherRequestPermissionException(PermissionConstants.VoucherRequestReadPermission, voucherRequestDto, cancellationToken);

            return Ok(voucherRequestDto);
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SearchResultDto<VoucherRequestDto>>> GetSearchResultDto([FromBody] VoucherRequestFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyPermissionException(PermissionConstants.VoucherRequestReadPermission);

            if (userContext.CompanyId.HasValue && userContext.CompanyId > 0)
            {
                filter.RequestCompanyId = userContext.CompanyId;
            } 
            else if (userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                filter.AuthorizedRepresentativeUserId = userContext.UserId;
            }
            else if (!userContext.OrganizationalUnits.Any(e => e.Alias == OrganizationalUnitConstants.NacidAlias || e.Alias == OrganizationalUnitConstants.PniiditAlias))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await voucherRequestService.GetSearchResultDto<VoucherRequestDto>(filter, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut("requestCode")]
        public async Task<ActionResult<ChangedStateDto>> RequestCode([FromBody] VoucherRequestStateDto voucherRequestStateDto, CancellationToken cancellationToken)
        {
            domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);

            var voucherRequest = await voucherRequestRepository.GetByProperties(e => e.SupplierOfferingId == voucherRequestStateDto.SupplierOfferingId && e.RequestCompanyId == voucherRequestStateDto.RequestCompanyId, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties));

            permissionService.VerifyUnitPermissionException(PermissionConstants.VoucherRequestWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, voucherRequest.RequestCompanyId) });

            if (voucherRequestStateDto.State != VoucherRequestState.Requested || voucherRequest.State != VoucherRequestState.Pending)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_State_Wrong);
            }

            return Ok(await voucherRequestService.ChangeState(voucherRequest, voucherRequestStateDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut("generateCode")]
        public async Task<ActionResult<ChangedStateDto>> GenerateCode([FromBody] VoucherRequestStateDto voucherRequestStateDto, CancellationToken cancellationToken)
        {
            domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);

            var voucherRequest = await voucherRequestRepository.GetByProperties(e => e.SupplierOfferingId == voucherRequestStateDto.SupplierOfferingId && e.RequestCompanyId == voucherRequestStateDto.RequestCompanyId, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties));

            await voucherRequestPermissionService.VerifyInstitutionVoucherRequestPermissionException(PermissionConstants.VoucherRequestWritePermission, voucherRequest, cancellationToken);

            if (voucherRequestStateDto.State != VoucherRequestState.Generated || voucherRequest.State != VoucherRequestState.Requested)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_State_Wrong);
            }

            return Ok(await voucherRequestService.ChangeState(voucherRequest, voucherRequestStateDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut("declineCode")]
        public async Task<ActionResult<ChangedStateDto>> DeclineCode([FromBody] VoucherRequestStateDto voucherRequestStateDto, CancellationToken cancellationToken)
        {
            domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);

            var voucherRequest = await voucherRequestRepository.GetByProperties(e => e.SupplierOfferingId == voucherRequestStateDto.SupplierOfferingId && e.RequestCompanyId == voucherRequestStateDto.RequestCompanyId, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties));

            await voucherRequestPermissionService.VerifyInstitutionVoucherRequestPermissionException(PermissionConstants.VoucherRequestWritePermission, voucherRequest, cancellationToken);

            if (voucherRequest.State == VoucherRequestState.Declined || voucherRequest.State == VoucherRequestState.Generated)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestErrorCode.VoucherRequest_State_Wrong);
            }

            return Ok(await voucherRequestService.ChangeState(voucherRequest, voucherRequestStateDto, cancellationToken));

        }
    }
}
