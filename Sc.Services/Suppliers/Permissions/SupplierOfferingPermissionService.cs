using Infrastructure;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Suppliers;

namespace Sc.Services.Suppliers.Permissions
{
    public class SupplierOfferingPermissionService
    {
        private readonly UserContext userContext;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public SupplierOfferingPermissionService(
            UserContext userContext,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
        )
        {
            this.userContext = userContext;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        public void VerifyOfferingPermissionException(string permission, SupplierDto supplierDto, int institutionOfferingId, CancellationToken cancellationToken)
        {
            if (!VerifyOfferingPermission(permission, supplierDto, institutionOfferingId, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }
        }

        public bool VerifyOfferingPermission(string permission, SupplierDto supplierDto, int supplierOfferingId, CancellationToken cancellationToken)
        {
            if (supplierDto.Type == SupplierType.Institution
                && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) }))
            {
                return false;
            }
            else if (supplierDto.Type == SupplierType.Complex
                && !permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) }))
            {
                return false;
            }

            var representativeUserId = supplierDto.Representative.UserId;

            var offeringTeamsIds = supplierDto.SupplierOfferings
                .Single(e => e.Id == supplierOfferingId)
                .SupplierOfferingTeams
                .Select(s => s.SupplierTeam.UserId)
                .ToList();

            if (permissionService.VerifyUnitPermission(permission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) })
                || (userContext.UserId.HasValue
                    && (representativeUserId == userContext.UserId.Value || offeringTeamsIds.Contains(userContext.UserId.Value))))
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
