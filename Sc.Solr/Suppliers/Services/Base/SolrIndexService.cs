using Sc.Solr.Suppliers.Entities.Base;
using Sc.Solr.Suppliers.Repositories.Base;
using SolrNet;

namespace Sc.Solr.Suppliers.Services.Base
{
    public class SolrIndexService<T> : ISolrIndexService<T>
        where T : BaseSolrSupplier
    {
        private readonly ISolrOperations<T> solr;

        public SolrIndexService(
            ISolrOperations<T> solr
            )
        {
            this.solr = solr;
        }

        public async Task<bool> Exists(int id)
        {
            var solrEntity = await solr.QueryAsync(new SolrQueryByField("id", id.ToString()));

            return solrEntity != null;
        }

        public async Task Index(T solrEntity)
        {
            solrEntity.Content = solrEntity.Content.ToLower();
            await solr.AddAsync(solrEntity);
            await solr.CommitAsync();
        }

        public async Task Delete(T solrEntity)
        {
            await solr.DeleteAsync(solrEntity);
            await solr.CommitAsync();
        }

        public async Task DeleteAll()
        {
            await solr.DeleteAsync(await solr.QueryAsync(SolrQuery.All));
            await solr.CommitAsync();
        }
    }
}
