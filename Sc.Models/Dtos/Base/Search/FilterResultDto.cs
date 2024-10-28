namespace Sc.Models.Dtos.Base.Search
{
    public class FilterResultDto
    {
        public int Id { get; set; }

        public string Code { get; set; } 
        public string Name { get; set; }
        public string NameAlt { get; set; }
        public string ShortName { get; set; }
        public string ShortNameAlt { get; set; }

        public int Count { get; set; }

        public bool IsSelected { get; set; } = false;
    }
}
