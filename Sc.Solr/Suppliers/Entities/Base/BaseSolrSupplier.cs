using SolrNet.Attributes;

namespace Sc.Solr.Suppliers.Entities.Base
{
    public class BaseSolrSupplier
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }
        [SolrField("supplierid")]
        public int SupplierId { get; set; }
        [SolrField("content")]
        public string Content { get; set; }
    }
}
