using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Repositories;
using Sc.Solr.Suppliers.Services.Base;
using SolrNet;

namespace Sc.Solr.SupplierOfferings.Services
{
    public class SupplierOfferingSolrIndexService : SolrIndexService<SolrSupplierOffering>, ISupplierOfferingSolrIndexService
    {

        public SupplierOfferingSolrIndexService(ISolrOperations<SolrSupplierOffering> solr)
            : base(solr)
        {
        }
    }
}
