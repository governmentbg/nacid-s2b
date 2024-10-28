using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Base.Nomenclatures
{
    public class NomenclatureFilterDto<TEntity> : FilterDto<TEntity>
        where TEntity : Nomenclature
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> Aliases { get; set; } = new List<string>();
        public bool ExcludeAliases { get; set; } = true;

        public List<int> ExcludeIds { get; set; } = new List<int>();

        public override IQueryable<TEntity> WhereBuilder(IQueryable<TEntity> query)
        {

            if (!string.IsNullOrWhiteSpace(Code))
            {
                var code = Code.Trim().ToLower();
                query = query.Where(e => e.Code.Trim().ToLower().Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Name.Trim().ToLower().Contains(name)
                    || e.NameAlt.Trim().ToLower().Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                var description = Description.Trim().ToLower();
                query = query.Where(e => e.Description.Trim().ToLower().Contains(description)
                    || e.DescriptionAlt.Trim().ToLower().Contains(description));
            }

            if (Aliases.Any())
            {
                if (ExcludeAliases)
                {
                    query = query.Where(e => !Aliases.Contains(e.Alias));
                }
                else
                {
                    query = query.Where(e => Aliases.Contains(e.Alias));
                }
            }

            if (IsActive.HasValue)
            {
                if (IsActive.Value)
                {
                    query = query.Where(e => e.IsActive);
                }
                else
                {
                    query = query.Where(e => !e.IsActive);
                }
            }

            if (ExcludeIds.Any())
            {
                query = query.Where(e => !ExcludeIds.Contains(e.Id));
            }

            query = ConstructTextFilter(query);

            return query;
        }

        public virtual IQueryable<TEntity> ConstructTextFilter(IQueryable<TEntity> query)
        {
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => (e.Code.Trim().ToLower() + " " + e.Name.Trim().ToLower()).Contains(textFilter));
            }

            return query;
        }
    }
}
