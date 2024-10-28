using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Services.Suppliers;
using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories;

namespace Server.SolrControllers
{
    [ApiController]
    [Route("api/se/solr")]
    public class SupplierEquipmentSolrController : ControllerBase
    {
        private readonly PermissionService permissionService;
        private readonly ISupplierEquipmentSolrIndexService seSolrIndexService;
        private readonly SupplierEquipmentService supplierEquipmentService;

        public SupplierEquipmentSolrController(
            PermissionService permissionService,
            ISupplierEquipmentSolrIndexService seSolrIndexService,
            SupplierEquipmentService supplierEquipmentService
        )
        {
            this.permissionService = permissionService;
            this.seSolrIndexService = seSolrIndexService;
            this.supplierEquipmentService = supplierEquipmentService;
        }

        [Authorize, ScClient]
        [HttpDelete("all")]
        public async Task<ActionResult> All()
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            await seSolrIndexService.DeleteAll();

            return Ok();
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            await seSolrIndexService.Delete(new SolrSupplierEquipment { Id = id });

            return Ok();
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}/forceIndex")]
        public async Task<ActionResult<List<SolrSupplierOffering>>> Index([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var solrSupplierEquipment = await supplierEquipmentService.ForceIndex(id, cancellationToken);

            if (solrSupplierEquipment != null)
            {
                return Ok(solrSupplierEquipment);
            }
            else
            {
                return BadRequest("Supplier equipment not found or does not contain file.");
            }
        }
    }
}
