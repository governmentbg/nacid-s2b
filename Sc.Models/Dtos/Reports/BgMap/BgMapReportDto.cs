namespace Sc.Models.Dtos.Reports.BgMap
{
    public class BgMapReportDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public int SuppliersCount { get; set; }

        public List<BgMapReportDto> Children { get; set; } = new List<BgMapReportDto>();
    }
}
