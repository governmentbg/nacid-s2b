using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Services.VoucherRequests;
using Sc.Services.VoucherRequests.Permissions;

namespace Server.Controllers.VoucherRequests
{
    [ApiController]
    [Route("api/voucherRequestCommunications")]
    public class VoucherRequestCommunicationController : ControllerBase
    {
        private readonly VoucherRequestService voucherRequestService;
        private readonly VoucherRequestCommunicationService communicationService;
        private readonly VoucherRequestPermissionService voucherRequestPermissionService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly UserContext userContext;

        public VoucherRequestCommunicationController(
            VoucherRequestService voucherRequestService,
            VoucherRequestCommunicationService communicationService,
            VoucherRequestPermissionService voucherRequestPermissionService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService,
            UserContext userContext
            )
        {
            this.voucherRequestService = voucherRequestService;
            this.communicationService = communicationService;
            this.voucherRequestPermissionService = voucherRequestPermissionService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
            this.userContext = userContext;
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<List<VoucherRequestCommunicationDto>>> GetCommunications([FromBody] VoucherRequestCommunicationFilterDto filter, CancellationToken cancellationToken)
        {
            if (!filter.SupplierOfferingId.HasValue || !filter.RequestCompanyId.HasValue)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestCommunicationErrorCode.VoucherRequestCommunication_SupplierOfferingCompany_Required);
            }

            if ((!userContext.CompanyId.HasValue || userContext.CompanyId == 0)
                && !userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            var voucherRequestDto = await voucherRequestService.GetBySupplierOfferingCompany(filter.SupplierOfferingId.Value, filter.RequestCompanyId.Value, cancellationToken);

            if (voucherRequestDto != null)
            {
                await voucherRequestPermissionService.VerifyVoucherRequestPermissionException(PermissionConstants.VoucherRequestReadPermission, voucherRequestDto, cancellationToken);

                return Ok(await communicationService.GetCommunications(filter, cancellationToken));
            }
            else
            {
                return Ok();
            }
        }

        [Authorize, ScClient]
        [HttpPost("sendMessage")]
        public async Task<ActionResult<VoucherRequestCommunicationDto>> SendMessage([FromBody] VoucherRequestCommunicationDto currentCommunicationDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            if (currentCommunicationDto.Entity == null || currentCommunicationDto.Entity.SupplierOfferingId == 0 || currentCommunicationDto.Entity.RequestCompanyId == 0)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestCommunicationErrorCode.VoucherRequestCommunication_SupplierOfferingCompany_Required);
            }

            if ((!userContext.CompanyId.HasValue || userContext.CompanyId == 0)
                && !userContext.OrganizationalUnits.Any(e => e.SupplierId.HasValue && e.SupplierId > 0))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            var voucherRequestDto = await voucherRequestService.GetBySupplierOfferingCompany(currentCommunicationDto.Entity.SupplierOfferingId, currentCommunicationDto.Entity.RequestCompanyId, cancellationToken);

            if (voucherRequestDto == null)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.VoucherRequestCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.CompanyAlias, currentCommunicationDto.Entity.RequestCompanyId) });

                if (!currentCommunicationDto.Entity.SupplierOffering.IsActive)
                {
                    domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Entity_Inactive);
                }

                return Ok(await communicationService.SendInitialMessage(currentCommunicationDto, cancellationToken));
            }
            else
            {
                await voucherRequestPermissionService.VerifyVoucherRequestPermissionException(PermissionConstants.VoucherRequestWritePermission, voucherRequestDto, cancellationToken);

                return Ok(await communicationService.SendMessage(voucherRequestDto.Id, voucherRequestDto.State, currentCommunicationDto, cancellationToken));
            }
        }
    }
}
