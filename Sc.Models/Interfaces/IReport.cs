using Dapper;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Entities.Base;
using Sc.Models.FilterDtos.Base;

namespace Sc.Models.Interfaces
{
    public interface IReport<TDto, TEntity, TFilter>
        where TDto : class
        where TEntity : EntityVersion
        where TFilter : DapperFilterDto<TEntity>, new()
    {
        Task<SearchResultDto<TDto>> GetReport(TFilter filter, CancellationToken cancellationToken);

        void SelectBuilder(SqlBuilder sqlBuilder);
        void WhereBuilder(SqlBuilder sqlBuilder, TFilter filter);
        void GroupByBuilder(SqlBuilder sqlBuilder);
        void OrderByBuilder(SqlBuilder sqlBuilder);
    }
}
