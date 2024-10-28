using AutoMapper;
using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Base;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Suppliers;
using Sc.Services.Suppliers.Permissions;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers/{supplierId:int}/offerings")]
    public class SupplierOfferingController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly SupplierOfferingService supplierOfferingService;
        private readonly ISupplierOfferingRepository supplierOfferingRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly SupplierService supplierService;
        private readonly SupplierOfferingPermissionService supplierOfferingPermissionService;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public SupplierOfferingController(
            IMapper mapper,
            SupplierOfferingService supplierOfferingService,
            ISupplierOfferingRepository supplierOfferingRepository,
            ISupplierRepository supplierRepository,
            SupplierService supplierService,
            SupplierOfferingPermissionService supplierOfferingPermissionService,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.mapper = mapper;
            this.supplierOfferingService = supplierOfferingService;
            this.supplierOfferingRepository = supplierOfferingRepository;
            this.supplierRepository = supplierRepository;
            this.supplierService = supplierService;
            this.supplierOfferingPermissionService = supplierOfferingPermissionService;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierOfferingDto>> GetById([FromRoute] int supplierId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await supplierOfferingService.GetDtoByIdAndSupplierId(id, supplierId, cancellationToken));
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierOfferingDto>>> GetBySupplier([FromRoute] int supplierId, CancellationToken cancellationToken)
        {
            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            return Ok(await supplierOfferingService.GetDtosBySupplier(supplierId, supplierDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<SupplierOfferingDto>> Create([FromRoute] int supplierId, [FromBody] SupplierOfferingDto supplierOfferingDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierDto = await supplierService.GetById(supplierId, cancellationToken);

            if (supplierDto.Type == SupplierType.Institution)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierOfferingCreatePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else if (supplierDto.Type == SupplierType.Complex)
            {
                permissionService.VerifyUnitPermissionException(PermissionConstants.SupplierOfferingCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) });
            }
            else
            {
                domainValidatorService.ThrowErrorMessage(PermissionErrorCode.Permission_NotEnoughPermissions);
            }

            return Ok(await supplierOfferingService.Create(supplierDto, supplierOfferingDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut]
        public async Task<ActionResult<SupplierOfferingDto>> Update([FromRoute] int supplierId, [FromBody] SupplierOfferingDto supplierOfferingDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierOfferingForUpdate = await supplierOfferingRepository.GetById(supplierOfferingDto.Id, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.Collections));

            if (supplierId != supplierOfferingForUpdate.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Supplier_InvalidSupplierId);
            }


            var supplierDto = mapper.Map<SupplierDto>(await supplierRepository.GetById(supplierId, cancellationToken,
                    e => e
                        .Include(s => s.Representative)
                        .Include(s => s.SupplierOfferings)
                            .ThenInclude(m => m.SupplierOfferingTeams)
                                .ThenInclude(n => n.SupplierTeam)));

            supplierOfferingPermissionService.VerifyOfferingPermissionException(PermissionConstants.SupplierOfferingWritePermission, supplierDto, supplierOfferingForUpdate.Id, cancellationToken);

            return Ok(await supplierOfferingService.Update(supplierOfferingForUpdate, supplierOfferingDto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int supplierId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierOfferingForDelete = await supplierOfferingRepository.GetById(id, cancellationToken,
                  е => е.Include(s => s.Files)
                        .Include(s => s.SmartSpecializations)
                        .Include(s => s.SupplierOfferingTeams)
                        .Include(s => s.SupplierOfferingEquipment));

            if (supplierId != supplierOfferingForDelete.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Supplier_InvalidSupplierId);
            }

            var supplierDto = mapper.Map<SupplierDto>(await supplierRepository.GetById(supplierId, cancellationToken,
                     e => e
                         .Include(s => s.Representative)
                         .Include(s => s.SupplierOfferings)
                             .ThenInclude(m => m.SupplierOfferingTeams)
                                 .ThenInclude(n => n.SupplierTeam)));

            supplierOfferingPermissionService.VerifyOfferingPermissionException(PermissionConstants.SupplierOfferingDeletePermission, supplierDto, supplierOfferingForDelete.Id, cancellationToken);

            try
            {
                await supplierOfferingService.Delete(supplierOfferingForDelete, cancellationToken);
            }
            catch (Exception)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Delete_ReferenceExisting);
            }

            return Ok();
        }

        [Authorize, ScClient]
        [HttpPut("isActive")]
        public async Task<ActionResult<bool>> ChangeIsActive([FromRoute] int supplierId, [FromBody] IsActiveDto isActiveDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var supplierOfferingForUpdate = await supplierOfferingRepository.GetById(isActiveDto.Id, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.None));

            if (supplierId != supplierOfferingForUpdate.SupplierId)
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Supplier_InvalidSupplierId);
            }

            var supplierDto = mapper.Map<SupplierDto>(await supplierRepository.GetById(supplierId, cancellationToken,
                    e => e
                        .Include(s => s.Representative)
                        .Include(s => s.SupplierOfferings)
                            .ThenInclude(m => m.SupplierOfferingTeams)
                                .ThenInclude(n => n.SupplierTeam)));

            supplierOfferingPermissionService.VerifyOfferingPermissionException(PermissionConstants.SupplierOfferingWritePermission, supplierDto, supplierOfferingForUpdate.Id, cancellationToken);

            return Ok(await supplierOfferingService.ChangeIsActive(supplierOfferingForUpdate, isActiveDto));
        }
    }
}
