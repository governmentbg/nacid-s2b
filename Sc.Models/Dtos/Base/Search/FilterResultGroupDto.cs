namespace Sc.Models.Dtos.Base.Search
{
    public class FilterResultGroupDto<T>
        where T : class
    {
        public SearchResultDto<T> SearchResult { get; set; } = new SearchResultDto<T>();
        public Dictionary<string, List<FilterResultDto>> FilterResult { get; set; } = new Dictionary<string, List<FilterResultDto>>();
    }
}
