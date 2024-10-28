using Sc.Solr.Suppliers.Entities.Base;
using SolrNet.Attributes;

namespace Sc.Solr.SupplierOfferings.Entities
{
    public class SolrSupplierOffering : BaseSolrSupplier
    {
        [SolrField("supplierofferingid")]
        public int SupplierOfferingId { get; set; }
    }
}
