using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Reports.BgMap;
using Sc.Models.Filters.Nomenclatures.Settlements;
using Sc.Repositories.Helpers;
using Sc.Repositories.Nomenclatures.Settlements;

namespace Sc.Reports.BgMap
{
    public class BgMapReportService
    {
        private readonly IDistrictRepository districtRepository;

        public BgMapReportService(
            IDistrictRepository districtRepository
            )
        {
            this.districtRepository = districtRepository;
        }

        public async Task<List<BgMapReportDto>> GetBgMap(DistrictFilterDto filterDto, CancellationToken cancellationToken)
        {
            if (filterDto == null)
            {
                filterDto = new DistrictFilterDto();
            }

            filterDto.GetAllData = true;

            var districtsList = await districtRepository.GetList(filterDto, cancellationToken, e => 
                e.Include(s => s.Complexes)
                    .ThenInclude(m => m.Suppliers)
                        .ThenInclude(n => n.SupplierOfferings)
                            .ThenInclude(b => b.SmartSpecializations)
                 .Include(s => s.Institutions)
                    .ThenInclude(m => m.Suppliers)
                        .ThenInclude(n => n.SupplierOfferings)
                            .ThenInclude(b => b.SmartSpecializations));

            var result = districtsList
                .Where(e => e.Institutions.Any(s => s.Suppliers.Any(m => m.SupplierOfferings.Any(m =>  m.IsActive))) || e.Complexes.Any(s => s.Suppliers.Any(m => m.SupplierOfferings.Any(m => m.IsActive))))
                .GroupBy(e => e.Region)
                .Select(e => new BgMapReportDto
                {
                    Id = (int)e.Key,
                    ParentId = 999999999,
                    Title = EnumHelper.GetEnumDescription(e.Key),
                    SuppliersCount = e.SelectMany(m => m.Institutions.SelectMany(n => n.Suppliers))
                    .Where(z => filterDto.SmartSpecializationId.HasValue ? z.SupplierOfferings.Any(x => x.IsActive && x.SmartSpecializations.Any(c => c.SmartSpecializationId == filterDto.SmartSpecializationId)) : true)
                    .Count(b => b.SupplierOfferings.Any(k => k.IsActive)) 
                    + e.SelectMany(m => m.Complexes.SelectMany(n => n.Suppliers))
                    .Where(z => filterDto.SmartSpecializationId.HasValue ? z.SupplierOfferings.Any(x => x.IsActive && x.SmartSpecializations.Any(c => c.SmartSpecializationId == filterDto.SmartSpecializationId)) : true)
                    .Count(b => b.SupplierOfferings.Any(k => k.IsActive)),
                    Children = e
                        .Where(s => s.Institutions.Any(s => s.Suppliers.Any()) || s.Complexes.Any(s => s.Suppliers.Any()))
                        .GroupBy(s => s.Id)
                        .Select(s => new BgMapReportDto
                        {
                            Id = s.Key,
                            ParentId = (int)e.Key,
                            Title = s.First().Name,
                            SuppliersCount = s.SelectMany(f => f.Institutions.SelectMany(g => g.Suppliers))
                            .Where(z => filterDto.SmartSpecializationId.HasValue ? z.SupplierOfferings.Any(x => x.IsActive && x.SmartSpecializations.Any(c => c.SmartSpecializationId == filterDto.SmartSpecializationId)) : true)
                            .Count(b => b.SupplierOfferings.Any(k => k.IsActive)) 
                            + s.SelectMany(f => f.Complexes.SelectMany(g => g.Suppliers))
                            .Where(z => filterDto.SmartSpecializationId.HasValue ? z.SupplierOfferings.Any(x => x.IsActive && x.SmartSpecializations.Any(c => c.SmartSpecializationId == filterDto.SmartSpecializationId)) : true)
                            .Count(b => b.SupplierOfferings.Any(k => k.IsActive))
                        })
                        .OrderBy(s => s.Title)
                        .ToList()
                })
                .OrderBy(s => s.Title)
                .ToList();

            result.Insert(0, new BgMapReportDto
            {
                Id = 999999999,
                SuppliersCount = result.Sum(e => e.SuppliersCount),
                Title = "България"
            });

            return result;
        }
    }
}
