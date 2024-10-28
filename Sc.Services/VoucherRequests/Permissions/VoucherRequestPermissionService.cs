using Infrastructure;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.Suppliers;

namespace Sc.Services.VoucherRequests.Permissions
{
    public class VoucherRequestPermissionService
    {
        private readonly UserContext userContext;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ISupplierTeamRepository supplierTeamRepository;

        public VoucherRequestPermissionService(
            UserContext userContext,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService,
            ISupplierTeamRepository supplierTeamRepository
        )
        {
            this.userContext = userContext;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
            this.supplierTeamRepository = supplierTeamRepository;
        }

        public async Task VerifyVoucherRequestPermissionException(string permission, VoucherRequestDto voucherRequestDto, CancellationToken cancellationToken)
        {
            if (!await VerifyVoucherRequestPermission(permission, voucherRequestDto, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }
        }

        public async Task VerifyInstitutionVoucherRequestPermissionException(string permission, VoucherRequest voucherRequest, CancellationToken cancellationToken)
        {
            if (!await VerifyInstitutionVoucherRequestPermission(permission, voucherRequest, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }
        }

        public async Task<bool> VerifyVoucherRequestPermission(string permission, VoucherRequestDto voucherRequestDto, CancellationToken cancellationToken)
        {
            if (permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { 
                (OrganizationalUnitConstants.NacidAlias, null),
                (OrganizationalUnitConstants.PniiditAlias, null),
                (OrganizationalUnitConstants.CompanyAlias, voucherRequestDto.RequestCompanyId)
            }))
            {
                return true;
            }
            else
            {
                if (voucherRequestDto.SupplierOffering.Supplier.Type == SupplierType.Institution
                    && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (null, voucherRequestDto.SupplierOffering.Supplier.InstitutionId) }))
                {
                    return false;
                }
                else if (voucherRequestDto.SupplierOffering.Supplier.Type == SupplierType.Complex
                    && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, voucherRequestDto.SupplierOffering.Supplier.ComplexId) }))
                {
                    return false;
                }

                var representativeUserId = voucherRequestDto.SupplierOffering.Supplier.Representative.UserId;

                var offeringTeams = await supplierTeamRepository
                    .GetListByProperties(e => e.SupplierId == voucherRequestDto.SupplierOffering.SupplierId
                        && e.SupplierOfferingTeams.Any(s => s.SupplierOfferingId == voucherRequestDto.SupplierOfferingId)
                        && e.IsActive == true, cancellationToken);

                var offeringTeamUserIds = offeringTeams.Select(e => e.UserId).ToList();

                if (userContext.UserId.HasValue
                    && (representativeUserId == userContext.UserId.Value || offeringTeamUserIds.Contains(userContext.UserId.Value)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<bool> VerifyInstitutionVoucherRequestPermission(string permission, VoucherRequest voucherRequest, CancellationToken cancellationToken)
        {
            if (voucherRequest.SupplierOffering.Supplier.Type == SupplierType.Institution
                && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (null, voucherRequest.SupplierOffering.Supplier.InstitutionId) }))
            {
                return false;
            }
            else if (voucherRequest.SupplierOffering.Supplier.Type == SupplierType.Complex
                && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, voucherRequest.SupplierOffering.Supplier.ComplexId) }))
            {
                return false;
            }

            var representativeUserId = voucherRequest.SupplierOffering.Supplier.Representative.UserId;

            var offeringTeams = await supplierTeamRepository
                .GetListByProperties(e => e.SupplierId == voucherRequest.SupplierOffering.SupplierId
                    && e.SupplierOfferingTeams.Any(s => s.SupplierOfferingId == voucherRequest.SupplierOfferingId)
                    && e.IsActive == true, cancellationToken);

            var offeringTeamUserIds = offeringTeams.Select(e => e.UserId).ToList();

            if (userContext.UserId.HasValue
                && (representativeUserId == userContext.UserId.Value || offeringTeamUserIds.Contains(userContext.UserId.Value)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
