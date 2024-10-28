using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers.Search.SupplierGroup;
using Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Suppliers;
using Sc.Models.FilterDtos.Suppliers.Junctions;
using Sc.Repositories.Suppliers.Junctions;

namespace Sc.Services.Suppliers
{
    public class SupplierSearchGroupService
    {
        private readonly ISoSmartSpecializationRepository soSmartSpecializationRepository;

        public SupplierSearchGroupService(ISoSmartSpecializationRepository soSmartSpecializationRepository)
        {
            this.soSmartSpecializationRepository = soSmartSpecializationRepository;
        }

        public async Task<SearchResultDto<SupplierRootGroupDto>> GetSupplierRootGroupDto(SupplierOfferingSmartSpecializationFilterDto filterDto, CancellationToken cancellationToken)
        {
            filterDto.GetAllData = true;
            filterDto.IsActive = true;

            var soSmartSpecializationList = await soSmartSpecializationRepository.GetList(filterDto, cancellationToken, soSmartSpecializationRepository.ConstructInclude(IncludeType.All));

            var institutionList = soSmartSpecializationList
                .Where(e => e.SupplierOffering.Supplier.Type == SupplierType.Institution)
                .OrderBy(e => e.SupplierOffering.Supplier.Institution.Id == e.SupplierOffering.Supplier.Institution.RootId)
                .GroupBy(e => e.SupplierOffering.Supplier.Institution.RootId)
                .Select(s => new SupplierRootGroupDto
                {
                    Id = s.First().SupplierOffering.Supplier.Institution.Id == s.First().SupplierOffering.Supplier.Institution.RootId ? s.First().SupplierOffering.SupplierId : null,
                    Code = s.First().SupplierOffering.Supplier.Institution.Root.Code,
                    Name = s.First().SupplierOffering.Supplier.Institution.Root.Name,
                    NameAlt = s.First().SupplierOffering.Supplier.Institution.Root.NameAlt,
                    Uic = s.First().SupplierOffering.Supplier.Institution.Root.Uic,
                    ViewOrder = s.First().SupplierOffering.Supplier.Institution.Root.ViewOrder,
                    SupplierSubordinateGroup = s
                        .Where(f => f.SupplierOffering.Supplier.Institution.Id != f.SupplierOffering.Supplier.Institution.RootId)
                        .GroupBy(f => f.SupplierOffering.Supplier.Institution.Id)
                        .Select(g => new SupplierSubordinateGroupDto
                        {
                            Id = g.First().SupplierOffering.SupplierId,
                            Code = g.First().SupplierOffering.Supplier.Institution.Code,
                            Name = g.First().SupplierOffering.Supplier.Institution.Name,
                            NameAlt = g.First().SupplierOffering.Supplier.Institution.NameAlt,
                            ViewOrder = g.First().SupplierOffering.Supplier.Institution.ViewOrder,
                            SmartSpecializations = g
                                .Where(f => f.SupplierOffering.SupplierId == g.First().SupplierOffering.SupplierId)
                                .GroupBy(f => f.SmartSpecialization.RootId)
                                .Select(v => new SmartSpecializationGroupDto
                                {
                                    Id = v.Key.Value,
                                    Code = v.First().SmartSpecialization.Root.Code,
                                    Name = v.First().SmartSpecialization.Root.Name,
                                    NameAlt = v.First().SmartSpecialization.Root.NameAlt,
                                    ViewOrder = v.First().SmartSpecialization.Root.ViewOrder,
                                    Offerings = v
                                    .GroupBy(o => o.SupplierOfferingId)
                                    .Select(o => new SmartSpecializationOfferingGroupDto
                                    {
                                        Id = o.First().SupplierOffering.Id,
                                        ShortDescription = o.First().SupplierOffering.ShortDescription,
                                        OfferingType = o.First().SupplierOffering.OfferingType,
                                        Name = o.First().SupplierOffering.Name
                                    })
                                    .Distinct()
                                    .OrderBy(e => e.Name)
                                    .ToList()
                                })
                                .OrderBy(e => e.ViewOrder)
                                .ThenBy(e => e.Id)
                                .ToList()
                        })
                        .OrderBy(e => e.ViewOrder)
                        .ThenBy(e => e.Name)
                        .ToList(),
                    SmartSpecializations = s
                        .Where(f => f.SupplierOffering.SupplierId == s.First().SupplierOffering.SupplierId
                            && f.SupplierOffering.Supplier.InstitutionId == f.SupplierOffering.Supplier.Institution.RootId)
                        .GroupBy(f => f.SmartSpecialization.RootId)
                        .Select(g => new SmartSpecializationGroupDto
                        {
                            Id = g.Key.Value,
                            Code = g.First().SmartSpecialization.Root.Code,
                            Name = g.First().SmartSpecialization.Root.Name,
                            NameAlt = g.First().SmartSpecialization.Root.Name,
                            ViewOrder = g.First().SmartSpecialization.Root.ViewOrder,
                            Offerings = g
                            .GroupBy(o => o.SupplierOfferingId)
                            .Select(o => new SmartSpecializationOfferingGroupDto
                            {
                                Id = o.First().SupplierOffering.Id,
                                ShortDescription = o.First().SupplierOffering.ShortDescription,
                                OfferingType = o.First().SupplierOffering.OfferingType,
                                Name = o.First().SupplierOffering.Name
                            })
                            .Distinct()
                            .OrderBy(e => e.Name)
                            .ToList()
                        })
                        .OrderBy(e => e.ViewOrder)
                        .ThenBy(e => e.Id)
                        .ToList()
                })
                .OrderBy(e => e.ViewOrder)
                .ThenBy(e => e.Name)
                .ToList();

            var complexList = soSmartSpecializationList
                .Where(e => e.SupplierOffering.Supplier.Type == SupplierType.Complex)
                .GroupBy(e => e.SupplierOffering.Supplier.ComplexId)
                .Select(s => new SupplierRootGroupDto
                {
                    Id = s.First().SupplierOffering.SupplierId,
                    Code = s.First().SupplierOffering.Supplier.Complex.Code,
                    Name = s.First().SupplierOffering.Supplier.Complex.Name,
                    NameAlt = s.First().SupplierOffering.Supplier.Complex.NameAlt,
                    Uic = s.First().SupplierOffering.Supplier.Complex.Code,
                    ViewOrder = s.First().SupplierOffering.Supplier.Complex.ViewOrder,
                    SmartSpecializations = s
                        .Where(f => f.SupplierOffering.SupplierId == s.First().SupplierOffering.SupplierId)
                        .GroupBy(f => f.SmartSpecialization.RootId)
                        .Select(g => new SmartSpecializationGroupDto
                        {
                            Id = g.Key.Value,
                            Code = g.First().SmartSpecialization.Root.Code,
                            Name = g.First().SmartSpecialization.Root.Name,
                            NameAlt = g.First().SmartSpecialization.Root.Name,
                            ViewOrder = g.First().SmartSpecialization.Root.ViewOrder,
                            Offerings = g
                            .GroupBy(o => o.SupplierOfferingId)
                            .Select(o => new SmartSpecializationOfferingGroupDto
                            {
                                Id = o.First().SupplierOffering.Id,
                                ShortDescription = o.First().SupplierOffering.ShortDescription,
                                OfferingType = o.First().SupplierOffering.OfferingType,
                                Name = o.First().SupplierOffering.Name
                            })
                            .Distinct()
                            .OrderBy(e => e.Name)
                            .ToList()
                        })
                        .OrderBy(e => e.ViewOrder)
                        .ThenBy(e => e.Id)
                        .ToList()
                })
                .OrderBy(e => e.ViewOrder)
                .ThenBy(e => e.Name)
                .ToList();

            var result = institutionList.Concat(complexList).ToList();

            var searchResult = new SearchResultDto<SupplierRootGroupDto>
            {
                Result = result,
                TotalCount = result.Count
            };

            return searchResult;
        }
    }
}
