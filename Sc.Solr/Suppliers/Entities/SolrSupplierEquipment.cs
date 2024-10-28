using Sc.Solr.Suppliers.Entities.Base;
using SolrNet.Attributes;

namespace Sc.Solr.Suppliers.Entities
{
    public class SolrSupplierEquipment : BaseSolrSupplier
    {
        [SolrField("supplierofferingids")]
        public List<int> SupplierOfferingIds { get; set; } = new List<int>();
    }
}
