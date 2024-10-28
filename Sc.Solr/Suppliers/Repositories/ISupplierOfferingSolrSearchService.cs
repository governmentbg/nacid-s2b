using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Repositories.Base;

namespace Sc.Solr.Suppliers.Repositories
{
    public interface ISupplierOfferingSolrSearchService : ISolrSearchService<SolrSupplierOffering>
    {
        Task<List<int>> GetOfferingIdsExactMatch(string searchContent);
        Task<List<int>> GetOfferingIdsMatchAll(string searchContent);
        Task<List<int>> GetOfferingIdsMatchAny(string searchContent);
    }
}
