using Infrastructure;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.Suppliers;

namespace Sc.Services.ReceivedVouchers.Permissions
{
    public class ReceivedVoucherPermissionService
    {
        private readonly UserContext userContext;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ISupplierTeamRepository supplierTeamRepository;

        public ReceivedVoucherPermissionService(
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

        public async Task VerifyReceivedVoucherPermissionException(string permission, ReceivedVoucherDto receivedVoucherDto, CancellationToken cancellationToken)
        {
            if (!await VerifyReceivedVoucherPermission(permission, receivedVoucherDto, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }
        }

        public async Task VerifyCertificateGenerationPermissionException(ReceivedVoucherCertificateDto certificateDto, CancellationToken cancellationToken)
        {
            if (!await VerifyCertificateGenerationPermission(certificateDto, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }
        }

        public async Task<bool> VerifyReceivedVoucherPermission(string permission, ReceivedVoucherDto receivedVoucherDto, CancellationToken cancellationToken)
        {
            if (permissionService.VerifyUnitPermission(permission, new List<(string, int?)> {
                (OrganizationalUnitConstants.NacidAlias, null),
                (OrganizationalUnitConstants.PniiditAlias, null),
                (OrganizationalUnitConstants.CompanyAlias, receivedVoucherDto.CompanyId)
            }))
            {
                return true;
            }
            else if (receivedVoucherDto.Supplier != null)
            {
                if ((receivedVoucherDto.Supplier.Type == SupplierType.Institution && permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (null, receivedVoucherDto.Supplier.InstitutionId) }))
                    || (receivedVoucherDto.Supplier.Type == SupplierType.Complex && permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, receivedVoucherDto.Supplier.ComplexId) }))
                    || (receivedVoucherDto.SecondSupplier != null && receivedVoucherDto.SecondSupplier.Type == SupplierType.Institution && permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (null, receivedVoucherDto.SecondSupplier.InstitutionId) }))
                    || (receivedVoucherDto.SecondSupplier != null && receivedVoucherDto.SecondSupplier.Type == SupplierType.Complex && permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, receivedVoucherDto.SecondSupplier.ComplexId) }))
                    )
                {
                    var representativeUserId = receivedVoucherDto.Supplier.Representative.UserId;
                    var secondRepresentativeUserId = receivedVoucherDto.SecondSupplier?.Representative?.UserId;

                    var offeringTeams = await supplierTeamRepository
                    .GetListByProperties(e => (e.SupplierId == receivedVoucherDto.SupplierId && e.SupplierOfferingTeams.Any(s => s.SupplierOfferingId == receivedVoucherDto.OfferingId)
                            || (receivedVoucherDto.SecondSupplierId.HasValue ? e.SupplierId == receivedVoucherDto.SecondSupplierId && e.SupplierOfferingTeams.Any(s => s.SupplierOfferingId == receivedVoucherDto.SecondOfferingId) : false))
                        && e.IsActive == true, cancellationToken);

                    var offeringTeamUserIds = offeringTeams.Select(e => e.UserId).ToList();

                    if (userContext.UserId.HasValue
                        && (representativeUserId == userContext.UserId.Value
                            || (secondRepresentativeUserId.HasValue && secondRepresentativeUserId == userContext.UserId.Value)
                            || offeringTeamUserIds.Contains(userContext.UserId.Value)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> VerifyCertificateGenerationPermission(ReceivedVoucherCertificateDto certificateDto, CancellationToken cancellationToken)
        {
            if ((certificateDto.Supplier.Type == SupplierType.Institution && permissionService.VerifyUnitPermission(PermissionConstants.ReceivedVoucherWritePermission, new List<(string, int?)> { (null, certificateDto.Supplier.InstitutionId) }))
                    || (certificateDto.Supplier.Type == SupplierType.Complex && permissionService.VerifyUnitPermission(PermissionConstants.ReceivedVoucherWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, certificateDto.Supplier.ComplexId) })))
            {
                var representativeUserId = certificateDto.Supplier.Representative.UserId;

                var offeringTeams = await supplierTeamRepository
                    .GetListByProperties(e => e.SupplierId == certificateDto.SupplierId && e.SupplierOfferingTeams.Any(s => s.SupplierOfferingId == certificateDto.OfferingId)
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
            else
            {
                return false;
            }
        }
    }
}
