using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Services.Suppliers;
using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Repositories;

namespace Server.SolrControllers
{
    [ApiController]
    [Route("api/so/solr")]
    public class SupplierOfferingSolrController : ControllerBase
    {
        private readonly PermissionService permissionService;
        private readonly ISupplierOfferingSolrIndexService soSolrIndexService;
        private readonly SupplierOfferingService supplierOfferingService;

        public SupplierOfferingSolrController(
            PermissionService permissionService,
            ISupplierOfferingSolrIndexService soSolrIndexService,
            SupplierOfferingService supplierOfferingService
        )
        {
            this.permissionService = permissionService;
            this.soSolrIndexService = soSolrIndexService;
            this.supplierOfferingService = supplierOfferingService;
        }

        [Authorize, ScClient]
        [HttpDelete("all")]
        public async Task<ActionResult> All()
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            await soSolrIndexService.DeleteAll();

            return Ok();
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var supplierOfferingDto = await supplierOfferingService.GetDtoById(id, cancellationToken);

            foreach (var fileForDelete in supplierOfferingDto.Files)
            {
                if (await soSolrIndexService.Exists(fileForDelete.Id))
                {
                    await soSolrIndexService.Delete(new SolrSupplierOffering { Id = fileForDelete.Id });
                }
            }

            return Ok();
        }

        [Authorize, ScClient]
        [HttpGet("{id:int}/forceIndex")]
        public async Task<ActionResult<List<SolrSupplierOffering>>> Index([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.SolrWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var solrSupplierOffering = await supplierOfferingService.ForceIndex(id, cancellationToken);

            if (solrSupplierOffering != null)
            {
                return Ok(solrSupplierOffering);
            }
            else
            {
                return BadRequest("Supplier offering not found or does not contain file.");
            }
        }
    }
}
