using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Enums.Common;

namespace Sc.Models.FilterDtos.Base.Nomenclatures
{
    public class NomenclatureHierarchyFilterDto<TEntity> : NomenclatureFilterDto<TEntity>
        where TEntity : NomenclatureHierarchy
    {
        public Level? Level { get; set; }
        public bool ExcludeLevel { get; set; } = false;
        public int? RootId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsRoot { get; set; }

        public override IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query)
        {
            if (Level.HasValue)
            {
                query = query.Where(e => ExcludeLevel ? e.Level != Level : e.Level == Level);
            }

            if (ParentId.HasValue)
            {
                query = query.Where(e => e.ParentId == ParentId);
            }

            if (RootId.HasValue)
            {
                query = query.Where(e => e.RootId == RootId);
            }

            if (IsRoot.HasValue)
            {
                if (IsRoot.Value)
                {
                    query = query.Where(e => !e.ParentId.HasValue);
                }
                else
                {
                    query = query.Where(e => e.ParentId.HasValue);
                }
            }

            return base.WhereBuilder(query);
        }
    }
}
