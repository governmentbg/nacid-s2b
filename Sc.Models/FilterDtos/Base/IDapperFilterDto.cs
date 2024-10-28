using Dapper;

namespace Sc.Models.FilterDtos.Base
{
    public interface IDapperFilterDto<TEntity>
    {
        int Limit { get; set; }
        int Offset { get; set; }
        bool GetAllData { get; set; }
        bool? IsActive { get; set; }

        void WhereBuilder(SqlBuilder sqlBuilder);
    }
}
