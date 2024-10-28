using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Enums.Suppliers;
using Sc.Models.Filters.Base;

namespace Sc.Models.FilterDtos.Suppliers
{
    public class SupplierTeamFilterDto : FilterDto<SupplierTeam>
    {
        public int? SupplierId { get; set; }
        public int? RootInstitutionId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<int> ExcludeIds { get; set; } = new List<int>();

        public override IQueryable<SupplierTeam> WhereBuilder(IQueryable<SupplierTeam> query)
        {
            if (SupplierId.HasValue)
            {
                query = query.Where(e => e.SupplierId == SupplierId);
            }

            if (RootInstitutionId.HasValue)
            {
                query = query.Where(e => e.Supplier.Institution.RootId == RootInstitutionId);
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var userName = UserName.Trim().ToLower();
                query = query.Where(e => e.UserName.Trim().ToLower() == userName);
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim().ToLower();
                query = query.Where(e => e.Name.Trim().ToLower().Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var email = Email.Trim().ToLower();
                query = query.Where(e => e.UserName.Trim().ToLower() == email);
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                var phoneNumber = PhoneNumber.Trim().ToLower();
                query = query.Where(e => e.PhoneNumber.Trim().ToLower() == phoneNumber);
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

            query = ConstructTextFilter(query);

            return query;
        }

        public virtual IQueryable<SupplierTeam> ConstructTextFilter(IQueryable<SupplierTeam> query)
        {
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                var textFilter = $"{TextFilter.Trim().ToLower()}";
                query = query.Where(e => (e.Name.Trim().ToLower() + " " + e.UserName.Trim().ToLower()).Contains(textFilter));
            }
            return query;
        }

        public void SetIsActive(PermissionService permissionService, SupplierDto supplierDto)
        {
            if ((supplierDto.Type == SupplierType.Institution
                    && permissionService.VerifyUnitPermission(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (null, supplierDto.InstitutionId), (OrganizationalUnitConstants.NacidAlias, null) }))
                 || (supplierDto.Type == SupplierType.Complex
                    && permissionService.VerifyUnitPermission(PermissionConstants.SupplierTeamWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.ComplexAlias, supplierDto.ComplexId), (OrganizationalUnitConstants.NacidAlias, null) })))
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
