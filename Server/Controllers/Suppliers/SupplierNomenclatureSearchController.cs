using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers/offerings")]
    public class SupplierOfferingSearchController : ControllerBase
    {
        private readonly SupplierOfferingService supplierOfferingService;

        public SupplierOfferingSearchController(
            SupplierOfferingService supplierOfferingService
            )
        {
            this.supplierOfferingService = supplierOfferingService;
        }

        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<SupplierOfferingDto>>> GetSearchResultDto([FromBody] SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await supplierOfferingService.GetSearchResultDto<SupplierOfferingDto>(filter, cancellationToken));
        }
    }

    [ApiController]
    [Route("api/suppliers/teams")]
    public class SupplierTeamSearchController : ControllerBase
    {
        private readonly SupplierTeamService supplierTeamService;

        public SupplierTeamSearchController(
            SupplierTeamService supplierTeamService
            )
        {
            this.supplierTeamService = supplierTeamService;
        }

        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<SupplierTeamDto>>> GetSearchResultDto([FromBody] SupplierTeamFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await supplierTeamService.GetSearchResultDto<SupplierTeamDto>(filter, IncludeType.None, cancellationToken));
        }
    }

    [ApiController]
    [Route("api/suppliers/equipment")]
    public class SupplierEquipmentSearchController : ControllerBase
    {
        private readonly SupplierEquipmentService supplierEquipmentService;

        public SupplierEquipmentSearchController(
            SupplierEquipmentService supplierEquipmentService
            )
        {
            this.supplierEquipmentService = supplierEquipmentService;
        }

        [HttpPost("search")]
        public async Task<ActionResult<SearchResultDto<SupplierEquipmentDto>>> GetSearchResultDto([FromBody] SupplierEquipmentFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await supplierEquipmentService.GetSearchResultDto<SupplierEquipmentDto>(filter, IncludeType.None, cancellationToken));
        }
    }
}
