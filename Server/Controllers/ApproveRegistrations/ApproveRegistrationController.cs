using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.ApproveRegistrations;
using Sc.Models.Dtos.ApproveRegistrations.Search;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.State;
using Sc.Models.FilterDtos.ApproveRegistrations;
using Sc.Services.ApproveRegistrations;

namespace Server.Controllers.ApproveRegistrations
{
    [ApiController]
    [Route("api/approveRegistrations")]
    public class ApproveRegistrationController : ControllerBase
    {
        private readonly ApproveRegistrationService approveRegistrationService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public ApproveRegistrationController(
            ApproveRegistrationService approveRegistrationService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.approveRegistrationService = approveRegistrationService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpPost]
        [Authorize, ScClient]
        public async Task<ActionResult<SearchResultDto<ApproveRegistrationSearchDto>>> GetSearchResultDto([FromBody] ApproveRegistrationFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await approveRegistrationService.GetSearchResultDto(filter, IncludeType.All, cancellationToken));
        }

        [HttpPost]
        [Route("count")]
        [Authorize, ScClient]
        public async Task<ActionResult<SupplierOfferingCountDto>> GetSupplierOfferingCount(ApproveRegistrationFilterDto filter, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var counts = await approveRegistrationService.GetSupplierOfferingCount(filter, cancellationToken);

            return Ok(counts);
        }

        [HttpGet("{id:int}")]
        [Authorize, ScClient]
        public async Task<ActionResult> GetSearchDtoById([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationReadPermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var result = await approveRegistrationService.GetSearchDtoById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPut]
        [Route("decline")]
        [Authorize, ScClient]
        public async Task<ActionResult> DeclineRegistration(DeclineRegistrationDto declineDto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var result = await approveRegistrationService.DeclineRegistration(declineDto, cancellationToken);

            return Ok(result);
        }

        [HttpPut]
        [Route("approve")]
        [Authorize, ScClient]
        public async Task<ActionResult> ApproveRegistration([FromBody] ApproveRegistrationDto approveDto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var result = await approveRegistrationService.ApproveRegistration(approveDto, cancellationToken);

            return Ok(result);
        }

        [HttpPut("signUpSupplier")]
        [Authorize, ScClient]
        public async Task<ActionResult> SignUpSupplier([FromBody] ApproveRegistrationSearchDto approveRegistrationSearchDto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            if (approveRegistrationSearchDto.State != ApproveRegistrationState.Approved)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_WrongState);
            }

            await approveRegistrationService.SignUpSupplier(approveRegistrationSearchDto, cancellationToken);

            return Ok();
        }

        [HttpPut("updateRepresentativeInfo")]
        [Authorize, ScClient]
        public async Task<ActionResult> UpdateRepresentativeInfo([FromBody] ApproveRegistrationSearchDto approveRegistrationSearchDto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.ApprovalRegistrationWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            if (approveRegistrationSearchDto.State != ApproveRegistrationState.Approved)
            {
                domainValidatorService.ThrowErrorMessage(ApproveRegistrationErrorCode.ApproveRegistration_WrongState);
            }

            await approveRegistrationService.UpdateRepresentativeInfo(approveRegistrationSearchDto, cancellationToken);

            return Ok();
        }
    }
}


