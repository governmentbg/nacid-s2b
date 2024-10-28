using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Services.Suppliers;

namespace Server.Controllers.Suppliers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService supplierService;

        public SupplierController(
            SupplierService supplierService
            )
        {
            this.supplierService = supplierService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await supplierService.GetById(id, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<SearchResultDto<SupplierDto>>> GetSearchResultDto([FromBody] SupplierFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await supplierService.GetSearchResultDto<SupplierDto>(filter, IncludeType.NavProperties, cancellationToken));
        }
    }
}
