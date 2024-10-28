namespace Sc.Models.Dtos.Base.Search
{
    public class SearchResultDto<T> where T : class
    {
        public int TotalCount { get; set; }
        public List<T> Result { get; set; }

        public SearchResultDto()
        {
            Result = new List<T>();
        }
    }
}
