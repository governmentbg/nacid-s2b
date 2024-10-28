using Sc.Solr.Suppliers.Entities;
using Sc.Solr.Suppliers.Repositories;
using Sc.Solr.Suppliers.Services.Base;
using SolrNet;

namespace Sc.Solr.Suppliers.Services
{
    public class SupplierEquipmentSolrSearchService : SolrSearchService<SolrSupplierEquipment>, ISupplierEquipmentSolrSearchService
    {
        public SupplierEquipmentSolrSearchService(
            ISolrOperations<SolrSupplierEquipment> solr
            ) : base(solr)
        {
        }

        public async Task<List<int>> GetOfferingIdsExactMatch(string searchContent)
        {
            var result = await GetExactMatch(searchContent);

            return result.SelectMany(e => e.SupplierOfferingIds).ToList();
        }

        public async Task<List<int>> GetOfferingIdsMatchAll(string searchContent)
        {
            var result = await GetMatchAll(searchContent);

            return result.SelectMany(e => e.SupplierOfferingIds).ToList();
        }

        public async Task<List<int>> GetOfferingIdsMatchAny(string searchContent)
        {
            var result = await GetMatchAny(searchContent);

            return result.SelectMany(e => e.SupplierOfferingIds).ToList();
        }
    }
}
