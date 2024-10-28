using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories.Base;

namespace Sc.Solr.Suppliers.Repositories
{
    public interface ISupplierEquipmentSolrSearchService : ISolrSearchService<SolrSupplierEquipment>
    {
        Task<List<int>> GetOfferingIdsExactMatch(string searchContent);
        Task<List<int>> GetOfferingIdsMatchAll(string searchContent);
        Task<List<int>> GetOfferingIdsMatchAny(string searchContent);
    }
}
