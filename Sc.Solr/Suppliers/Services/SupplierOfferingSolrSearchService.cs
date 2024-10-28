using Sc.Solr.SupplierOfferings.Entities;
using Sc.Solr.Suppliers.Repositories;
using Sc.Solr.Suppliers.Services.Base;
using SolrNet;

namespace Sc.Solr.SupplierOfferings.Services
{
    public class SupplierOfferingSolrSearchService : SolrSearchService<SolrSupplierOffering>, ISupplierOfferingSolrSearchService
    {
        public SupplierOfferingSolrSearchService(
            ISolrOperations<SolrSupplierOffering> solr
            ) : base(solr)
        {
        }

        public async Task<List<int>> GetOfferingIdsExactMatch(string searchContent)
        {
            var result = await GetExactMatch(searchContent);

            return result.Select(e => e.SupplierOfferingId).ToList();
        }

        public async Task<List<int>> GetOfferingIdsMatchAll(string searchContent)
        {
            var result = await GetMatchAll(searchContent);

            return result.Select(e => e.SupplierOfferingId).ToList();
        }

        public async Task<List<int>> GetOfferingIdsMatchAny(string searchContent)
        {
            var result = await GetMatchAny(searchContent);

            return result.Select(e => e.SupplierOfferingId).ToList();
        }
    }
}
