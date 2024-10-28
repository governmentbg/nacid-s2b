using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories;
using Sc.Solr.Suppliers.Services.Base;
using SolrNet;

namespace Sc.Solr.Suppliers.Services
{
    public class SupplierEquipmentSolrIndexService : SolrIndexService<SolrSupplierEquipment>, ISupplierEquipmentSolrIndexService
    {

        public SupplierEquipmentSolrIndexService(ISolrOperations<SolrSupplierEquipment> solr)
            : base(solr)
        {
        }
    }
}
