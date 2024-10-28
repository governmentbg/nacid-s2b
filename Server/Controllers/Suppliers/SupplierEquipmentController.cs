using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers/{supplierId:int}/equipment")]
    public class SupplierEquipmentController : ControllerBase
    {
        private readonly SupplierEquipmentService supplierEquipmentService;
        private readonly ISupplierEquipmentRepository supplierEquipmentRepository;
        private readonly SupplierService supplierService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public SupplierEquipmentController(
            SupplierEquipmentService supplierEquipmentService,
            ISupplierEquipmentRepository supplierEquipmentRepository,
            SupplierService supplierService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.supplierEquipmentService = supplierEquipmentService;
            this.supplierEquipmentRepository = supplierEquipmentRepository;
            this.supplierService = supplierService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierEquipmentDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await supplierEquipmentService.GetDtoById(id, cancellationToken));
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierEquipmentDto>>> GetBySupplier([FromRoute] int supplierId, CancellationToken cancellationToken)
        {
            return Ok(await supplierEquipmentService.GetBySupplierDtos(supplierId, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SupplierEquipmentDto>> Create([FromRoute] int supplierId, [FromBody] SupplierEquipmentDto supplierEquipmentDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentCreatePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierEquipmentService.Create(supplierId, supplierEquipmentDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut]
        public async Task<ActionResult<SupplierEquipmentDto>> Update([FromRoute] int supplierId, [FromBody] SupplierEquipmentDto supplierEquipmentDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierEquipmentForUpdate = await supplierEquipmentRepository.GetById(supplierEquipmentDto.Id, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.All));

            if (supplierId != supplierEquipmentForUpdate.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Supplier_InvalidSupplierId);
            }

            if (supplierEquipmentForUpdate.Supplier.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentWritePermission, new List<(string, int?)> { (null, supplierEquipmentForUpdate.Supplier.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierEquipmentForUpdate.Supplier.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierEquipmentForUpdate.Supplier.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierEquipmentService.Update(supplierEquipmentForUpdate, supplierEquipmentDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int supplierId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierEquipmentForDelete = await supplierEquipmentRepository.GetById(id, cancellationToken, supplierEquipmentRepository.ConstructInclude(IncludeType.All));

            if (supplierId != supplierEquipmentForDelete.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierEquipmentErrorCode.SupplierEquipment_Supplier_InvalidSupplierId);
            }

            if (supplierEquipmentForDelete.Supplier.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentDeletePermission, new List<(string, int?)> { (null, supplierEquipmentForDelete.Supplier.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierEquipmentForDelete.Supplier.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierEquipmentDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierEquipmentForDelete.Supplier.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            await supplierEquipmentService.Delete(supplierEquipmentForDelete, cancellationToken);

            return Ok();
        }
    }
}
