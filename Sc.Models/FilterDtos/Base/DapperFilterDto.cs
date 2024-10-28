using Dapper;

namespace Sc.Models.FilterDtos.Base
{
    public abstract class DapperFilterDto<TEntity> : IDapperFilterDto<TEntity>
        where TEntity : class
    {
        public int Limit { get; set; } = 30;
        public int Offset { get; set; }
        public bool GetAllData { get; set; } = true;
        public bool? IsActive { get; set; }

        public virtual void DefaultWhereBuilder(SqlBuilder sqlBuilder)
        {
        }

        public virtual void WhereBuilder(SqlBuilder sqlBuilder)
        {
        }
    }
}
