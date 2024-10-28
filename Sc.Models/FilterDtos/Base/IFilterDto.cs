namespace Sc.Models.Filters.Base
{
    public interface IFilterDto<TEntity>
    {
        int Limit { get; set; }
        int Offset { get; set; }
        bool GetAllData { get; set; }
        string TextFilter { get; set; }

        IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query);
    }
}
