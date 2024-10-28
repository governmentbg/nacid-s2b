using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/supplierOfferings/group")]
    public class SupplierOfferingGroupController : ControllerBase
    {
        private readonly SupplierOfferingGroupService supplierOfferingGroupService;

        public SupplierOfferingGroupController(
            SupplierOfferingGroupService supplierOfferingGroupService
            )
        {
            this.supplierOfferingGroupService = supplierOfferingGroupService;
        }

        [HttpGet("smartSpecializations")]
        public async Task<ActionResult<SearchResultDto<SmartSpecializationRootGroupDto>>> GetSmartSpecializationGroupDto(CancellationToken cancellationToken)
        {
            return Ok(await supplierOfferingGroupService.GetSmartSpecializationRootGroupDto(cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<FilterResultGroupDto<SupplierOfferingSearchDto>>> GetSupplierOfferingGroupDto([FromBody] SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await supplierOfferingGroupService.GetSupplierOfferingGroupDto(filter, cancellationToken));
        }
    }
}
