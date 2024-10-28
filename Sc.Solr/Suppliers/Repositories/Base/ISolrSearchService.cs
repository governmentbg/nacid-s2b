using Sc.Solr.Suppliers.Entities.Base;

namespace Sc.Solr.Suppliers.Repositories.Base
{
    public interface ISolrSearchService<T>
        where T : BaseSolrSupplier
    {
        Task<List<T>> GetExactMatch(string searchContent);
        Task<List<T>> GetMatchAll(string searchContent);
        Task<List<T>> GetMatchAny(string searchContent);
    }
}
