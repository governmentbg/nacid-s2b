using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using LinqKit;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Suppliers
{
    public class SupplierOfferingFilterDto : FilterDto<SupplierOffering>
    {
        public int? SupplierId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public KeywordsSearchType KeywordsSearchType { get; set; } = KeywordsSearchType.ExactMatch;
        public string Keywords { get; set; }

        public List<int> SupplierIds { get; set; } = new List<int>();
        public List<int> SmartSpecializationRootIds { get; set; } = new List<int>();
        public List<int> OfferingIds { get; set; } = new List<int>();
        public List<int> ExcludeIds { get; set; } = new List<int>();

        public override IQueryable<SupplierOffering> WhereBuilder(IQueryable<SupplierOffering> query)
        {
            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.SupplierId == SupplierId);
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                query = query.Where(e => e.Code.Trim() == Code.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                var keywords = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var predicate = PredicateBuilder.New<SupplierOffering>(true);

                if (KeywordsSearchType == KeywordsSearchType.ExactMatch)
                {
                    query = query.Where(e => e.Name.Trim().ToLower().Contains(name));
                }
                else if (KeywordsSearchType == KeywordsSearchType.MatchAll)
                {
                    foreach (var keyword in keywords)
                    {
                        predicate = predicate.And(e => e.Name.Trim().ToLower().Contains(keyword));
                    }

                    query = query.Where(predicate); 
                }
                else if (KeywordsSearchType == KeywordsSearchType.MatchAny)
                {
                    foreach (var keyword in keywords)
                    {
                        predicate = predicate.Or(x => x.Name.Trim().ToLower().Contains(keyword));
                    }

                    query = query.Where(predicate);
                }
                else
                {
                    query = query.Where(e => e.Name.Trim().ToLower().Contains(name));
                }
            }

            if (OfferingIds.Any())
            {
                query = query.Where(e => OfferingIds.Contains(e.Id));
            }

            if (SupplierIds.Any())
            {
                query = query.Where(e => SupplierIds.Contains(e.SupplierId));
            }

            if (SmartSpecializationRootIds.Any())
            {
                query = query.Where(e => e.SmartSpecializations.Any(s => SmartSpecializationRootIds.Contains(s.SmartSpecialization.RootId.Value)));
            }

            if (ExcludeIds.Any())
            {
                query = query.Where(e => !ExcludeIds.Contains(e.Id));
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

            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => (e.Code.Trim().ToLower() + " - " + e.Name.Trim().ToLower()).Contains(textFilter));
            }

            return query;
        }

        public void SetIsActive(PermissionService permissionService, SupplierDto supplierDto)
        {
            if ((supplierDto.Type == SupplierType.Institution 
                    && permissionService.VerifyUnitPermission(PermissionConstants.SupplierOfferingWritePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) }))
                 || (supplierDto.Type == SupplierType.Complex
                    && permissionService.VerifyUnitPermission(PermissionConstants.SupplierOfferingWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) })))
            {
                IsActive = null;
            }
            else
            {
                IsActive = true;
            }
        }
    }
}
