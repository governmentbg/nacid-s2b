using Sc.Solr.Suppliers.Entities.Base;

namespace Sc.Solr.Suppliers.Repositories.Base
{
    public interface ISolrIndexService<T>
        where T : BaseSolrSupplier
    {
        Task<bool> Exists(int id);
        Task Index(T solrEntity);
        Task Delete(T solrEntity);
        Task DeleteAll();
    }
}
