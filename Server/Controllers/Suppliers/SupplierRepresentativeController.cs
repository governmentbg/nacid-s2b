using Infrastructure.DomainValidation;
using Infrastructure.Permissions;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Suppliers;
using Sc.Repositories.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers/{supplierId:int}/representative")]
    public class SupplierRepresentativeController : ControllerBase
    {
        private readonly SupplierRepresentativeService supplierRepresentativeService;
        private readonly ISupplierRepresentativeRepository supplierRepresentativeRepository;
        private readonly PermissionService permissionService;
        private readonly DomainValidatorService domainValidatorService;

        public SupplierRepresentativeController(
            SupplierRepresentativeService supplierRepresentativeService,
            ISupplierRepresentativeRepository supplierRepresentativeRepository,
            PermissionService permissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.supplierRepresentativeService = supplierRepresentativeService;
            this.supplierRepresentativeRepository = supplierRepresentativeRepository;
            this.permissionService = permissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [HttpGet]
        public async Task<ActionResult<SupplierRepresentativeDto>> GetById([FromRoute] int supplierId, CancellationToken cancellationToken)
        {
            return Ok(await supplierRepresentativeService.GetDtoById(supplierId, cancellationToken));
        }
    }
}
