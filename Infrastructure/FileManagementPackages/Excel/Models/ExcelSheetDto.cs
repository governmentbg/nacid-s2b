using System.Linq.Expressions;

namespace Infrastructure.FileManagementPackages.Excel.Models
{
    public class ExcelSheetDto<TResult>
    {
        public string SheetName { get; set; }

        public string ReportName { get; set; }

        public List<ExcelSheetFilterDto> Filters { get; set; } = new List<ExcelSheetFilterDto>();
        public List<object> Items { get; set; }
        public List<Expression<Func<object, TResult>>> Expressions { get; set; }
    }
}
