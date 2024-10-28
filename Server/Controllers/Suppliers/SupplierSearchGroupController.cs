using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers.Search.SupplierGroup;
using Sc.Models.FilterDtos.Suppliers.Junctions;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/supplierGroup/search")]
    public class SupplierSearchGroupController : ControllerBase
    {
        private readonly SupplierSearchGroupService supplierSearchGroupService;

        public SupplierSearchGroupController(SupplierSearchGroupService supplierSearchGroupService)
        {
            this.supplierSearchGroupService = supplierSearchGroupService;
        }

        [HttpPost]
        public async Task<ActionResult<SearchResultDto<SupplierRootGroupDto>>> GetSupplierRootGroupDto([FromBody] SupplierOfferingSmartSpecializationFilterDto filterDto, CancellationToken cancellationToken)
        {
            return Ok(await supplierSearchGroupService.GetSupplierRootGroupDto(filterDto, cancellationToken));
        }
    }
}
