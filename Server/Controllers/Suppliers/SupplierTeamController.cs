using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers/{supplierId:int}/teams")]
    public class SupplierTeamController : ControllerBase
    {
        private readonly SupplierTeamService supplierTeamService;
        private readonly ISupplierTeamRepository supplierTeamRepository;
        private readonly SupplierService supplierService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public SupplierTeamController(
            SupplierTeamService supplierTeamService,
            ISupplierTeamRepository supplierTeamRepository,
            SupplierService supplierService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.supplierTeamService = supplierTeamService;
            this.supplierTeamRepository = supplierTeamRepository;
            this.supplierService = supplierService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierTeamDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await supplierTeamService.GetDtoById(id, cancellationToken));
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierTeamDto>>> GetBySupplier([FromRoute] int supplierId, CancellationToken cancellationToken)
        {
            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            return Ok(await supplierTeamService.GetBySupplierDtos(supplierId, supplierDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SupplierTeamDto>> Create([FromRoute] int supplierId, [FromBody] SupplierTeamDto supplierTeamDto, CancellationToken cancellationToken)
        {
            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamCreatePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierTeamService.Create(supplierDto, supplierTeamDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut]
        public async Task<ActionResult<SupplierTeamDto>> Update([FromRoute] int supplierId, [FromBody] SupplierTeamDto supplierTeamDto, CancellationToken cancellationToken)
        {
            var supplierTeamForUpdate = await supplierTeamRepository.GetById(supplierTeamDto.Id, cancellationToken, supplierTeamRepository.ConstructInclude(IncludeType.Collections));

            if (supplierId != supplierTeamForUpdate.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Supplier_InvalidSupplierId);
            }

            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierTeamService.Update(supplierTeamForUpdate, supplierTeamDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut("isActive")]
        public async Task<ActionResult<bool>> ChangeIsActive([FromRoute] int supplierId, [FromBody] IsActiveDto isActiveDto, CancellationToken cancellationToken)
        {
            var supplierTeamForUpdate = await supplierTeamRepository.GetById(isActiveDto.Id, cancellationToken, supplierTeamRepository.ConstructInclude(IncludeType.None));

            if (supplierId != supplierTeamForUpdate.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Supplier_InvalidSupplierId);
            }

            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierTeamService.ChangeIsActive(supplierTeamForUpdate, isActiveDto));
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int supplierId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);

            var supplierTeamForDelete = await supplierTeamRepository.GetById(id, cancellationToken, supplierTeamRepository.ConstructInclude(IncludeType.All));

            if (supplierId != supplierTeamForDelete.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierTeamErrorCode.SupplierTeam_Supplier_InvalidSupplierId);
            }

            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamDeletePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierTeamDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            await supplierTeamService.Delete(supplierTeamForDelete);

            return Ok();
        }
    }
}
