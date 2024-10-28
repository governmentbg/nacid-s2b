using Infrastructure.FileManagementPackages.Excel.Models;
using Sc.Models.Entities.Base;
using Sc.Models.FilterDtos.Base;

namespace Sc.Models.Interfaces
{
    public interface IExport<TEntity, TFilter>
        where TEntity : EntityVersion
        where TFilter : DapperFilterDto<TEntity>, new()
    {
        List<ExcelSheetFilterDto> ConstructSheetFilter(TFilter filter);

        Task<MemoryStream> ExportExcel(TFilter filter, CancellationToken cancellationToken);
        Task<MemoryStream> ExportJson(TFilter filter, CancellationToken cancellationToken);
        Task<MemoryStream> ExportCsv(TFilter filter, CancellationToken cancellationToken);
    }
}
