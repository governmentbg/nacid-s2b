using Infrastructure.Helpers.Extensions;
using Sc.Solr.Suppliers.Entities.Base;
using Sc.Solr.Suppliers.Repositories.Base;
using SolrNet;

namespace Sc.Solr.Suppliers.Services.Base
{
    public class SolrSearchService<T> : ISolrSearchService<T>
        where T : BaseSolrSupplier
    {
        public readonly ISolrOperations<T> solr;

        public SolrSearchService(
            ISolrOperations<T> solr
            )
        {
            this.solr = solr;
        }

        public async Task<List<T>> GetExactMatch(string searchContent)
        {
            var solrQuery = new SolrQuery($"content:*{searchContent.ToLower().EscapeSpaces()}*");
            var result = await solr.QueryAsync(solrQuery);

            return result.ToList();
        }

        public async Task<List<T>> GetMatchAll(string searchContent)
        {
            AbstractSolrQuery solrQuery = new SolrQuery("");
            var allWords = searchContent.ToLower().Split(" ").ToList();

            foreach (var word in allWords)
            {
                solrQuery = solrQuery && new SolrQuery($"content:*{word}*");
            }

            var result = await solr.QueryAsync(solrQuery);

            return result.ToList();
        }

        public async Task<List<T>> GetMatchAny(string searchContent)
        {
            AbstractSolrQuery solrQuery = new SolrQuery("");
            var allWords = searchContent.ToLower().Split(" ").ToList();

            foreach (var word in allWords)
            {
                solrQuery = solrQuery || new SolrQuery($"content:*{word}*");
            }

            var result = await solr.QueryAsync(solrQuery);

            return result.ToList();
        }
    }
}
