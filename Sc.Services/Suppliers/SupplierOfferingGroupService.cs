using AutoMapper;
using Infrastructure.Permissions.Constants;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Suppliers;
using Sc.Models.FilterDtos.Suppliers.Junctions;
using Sc.Repositories.Suppliers;
using Sc.Repositories.Suppliers.Junctions;
using Sc.Solr.Suppliers.Repositories;

namespace Sc.Services.Suppliers
{
    public class SupplierOfferingGroupService
    {
        private readonly IMapper mapper;
        private readonly ISoSmartSpecializationRepository soSmartSpecializationRepository;
        private readonly ISupplierOfferingRepository supplierOfferingRepository;
        private readonly ISupplierOfferingSolrSearchService soSolrSearchService;
        private readonly ISupplierEquipmentSolrSearchService seSolrSearchService;

        public SupplierOfferingGroupService(
            IMapper mapper,
            ISoSmartSpecializationRepository soSmartSpecializationRepository,
            ISupplierOfferingRepository supplierOfferingRepository,
            ISupplierOfferingSolrSearchService soSolrSearchService,
            ISupplierEquipmentSolrSearchService seSolrSearchService
            )
        {
            this.mapper = mapper;
            this.soSmartSpecializationRepository = soSmartSpecializationRepository;
            this.supplierOfferingRepository = supplierOfferingRepository;
            this.soSolrSearchService = soSolrSearchService;
            this.seSolrSearchService = seSolrSearchService;
        }

        public async Task<SearchResultDto<SmartSpecializationRootGroupDto>> GetSmartSpecializationRootGroupDto(CancellationToken cancellationToken)
        {
            var filterDto = new SupplierOfferingSmartSpecializationFilterDto
            {
                GetAllData = true,
                IsActive = true
            };

            var soSmartSpecializationList = await soSmartSpecializationRepository.GetList(filterDto, cancellationToken, soSmartSpecializationRepository.ConstructInclude(IncludeType.All));

            var list = soSmartSpecializationList
                .Where(e => e.SmartSpecialization.RootId.HasValue)
                .GroupBy(e => e.SmartSpecialization.RootId)
                .Select(s => new SmartSpecializationRootGroupDto
                {
                    Id = s.Key.Value,
                    Code = s.First().SmartSpecialization.Root.Code,
                    Name = s.First().SmartSpecialization.Root.Name,
                    NameAlt = s.First().SmartSpecialization.Root.NameAlt,
                    ViewOrder = s.First().SmartSpecialization.Root.ViewOrder,
                    SmartSpecializations = s
                        .GroupBy(s => s.SmartSpecializationId)
                        .Select(m => new SmartSpecializationGroupDto
                        {
                            Id = m.First().SmartSpecialization.Id,
                            Code = m.First().SmartSpecialization.Code,
                            Name = m.First().SmartSpecialization.Name,
                            NameAlt = m.First().SmartSpecialization.NameAlt,
                            ViewOrder = m.First().SmartSpecialization.ViewOrder,
                            Offerings = m.Select(f => new SmartSpecializationOfferingGroupDto
                            {
                                Id = f.SupplierOffering.Id,
                                Name = f.SupplierOffering.Name,
                                SupplierId = f.SupplierOffering.SupplierId,
                                SupplierName = f.SupplierOffering.Supplier?.Institution.Name ?? f.SupplierOffering.Supplier?.Complex.Name,
                                SupplierNameAlt = f.SupplierOffering.Supplier?.Institution.NameAlt ?? f.SupplierOffering.Supplier?.Complex.NameAlt,
                                InstitutionId = f.SupplierOffering.Supplier?.InstitutionId,
                                RootInstitutionId = f.SupplierOffering.Supplier?.Institution?.RootId,
                                RootInstitutionShortName = f.SupplierOffering.Supplier?.Institution?.Root?.ShortName,
                                RootInstitutionShortNameAlt = f.SupplierOffering.Supplier?.Institution?.Root?.ShortNameAlt
                            })
                            .OrderBy(e => e.Name)
                            .ToList()
                        })
                        .OrderBy(f => f.ViewOrder)
                        .ThenBy(f => f.Name)
                        .ToList()
                })
                .OrderBy(f => f.ViewOrder)
                .ThenBy(f => f.Name)
                .ToList();

            var searchResult = new SearchResultDto<SmartSpecializationRootGroupDto>
            {
                Result = list,
                TotalCount = list.Count()
            };

            return searchResult;
        }

        public async Task<FilterResultGroupDto<SupplierOfferingSearchDto>> GetSupplierOfferingGroupDto(SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            filter.GetAllData = true;
            filter.IsActive = true;

            if (!string.IsNullOrWhiteSpace(filter.Keywords))
            {
                filter.OfferingIds = await GetOfferingIdsKeywords(filter.Keywords, filter.KeywordsSearchType);
                if (!filter.OfferingIds.Any())
                {
                    return new FilterResultGroupDto<SupplierOfferingSearchDto>();
                }
            }

            var (result, count) = await supplierOfferingRepository.GetAll(filter, cancellationToken, supplierOfferingRepository.ConstructInclude(IncludeType.All), e => e.OrderBy(e => e.Name));

            var searchResult = new SearchResultDto<SupplierOfferingSearchDto>
            {
                Result = mapper.Map<List<SupplierOfferingSearchDto>>(result),
                TotalCount = count
            };

            var filterResultGroup = new FilterResultGroupDto<SupplierOfferingSearchDto>
            {
                SearchResult = searchResult,
                FilterResult = await ConstructFilterResult(filter, cancellationToken)
            };

            return filterResultGroup;
        }

        private async Task<List<int>> GetOfferingIdsKeywords(string content, KeywordsSearchType keywordsSearchType)
        {
            List<int> offerings = new List<int>();
            List<int> equipment = new List<int>();

            switch (keywordsSearchType)
            {
                case KeywordsSearchType.ExactMatch:
                    offerings = await soSolrSearchService.GetOfferingIdsExactMatch(content);
                    equipment = await seSolrSearchService.GetOfferingIdsExactMatch(content);
                    break;
                case KeywordsSearchType.MatchAll:
                    offerings = await soSolrSearchService.GetOfferingIdsMatchAll(content);
                    equipment = await seSolrSearchService.GetOfferingIdsMatchAll(content);
                    break;
                case KeywordsSearchType.MatchAny:
                    offerings = await soSolrSearchService.GetOfferingIdsMatchAny(content);
                    equipment = await seSolrSearchService.GetOfferingIdsMatchAny(content);
                    break;
                default:
                    offerings = await soSolrSearchService.GetOfferingIdsExactMatch(content);
                    equipment = await seSolrSearchService.GetOfferingIdsExactMatch(content);
                    break;
            }

            return offerings.Union(equipment).ToList();
        }

        private async Task<Dictionary<string, List<FilterResultDto>>> ConstructFilterResult(SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            var filterResult = new Dictionary<string, List<FilterResultDto>>
            {
                { SupplierConstants.Suppliers, await ConstructSupplierFilterResult(filter, cancellationToken) },
                { SupplierConstants.SmartSpecializations, await ConstructSmartSpecializationFilterResult(filter, cancellationToken) }
            };

            return filterResult;
        }

        private async Task<List<FilterResultDto>> ConstructSupplierFilterResult(SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            var tempSupplierIds = filter.SupplierIds;
            filter.SupplierIds = new List<int>();
            var supplierOfferings = await supplierOfferingRepository.GetList(filter, cancellationToken, e => e.Include(s => s.Supplier.Institution.Root).Include(s => s.Supplier.Complex));
            filter.SupplierIds = tempSupplierIds;

            var list = supplierOfferings
                .GroupBy(e => e.SupplierId)
                .Select(s => new FilterResultDto
                {
                    Id = s.First().SupplierId,
                    Name = s.First().Supplier?.Institution != null 
                        ? $"{s.First().Supplier?.Institution?.Name} ({s.First().Supplier?.Institution?.Root?.ShortName})"
                        : s.First().Supplier?.Complex?.Name,
                    NameAlt = s.First().Supplier?.Institution != null
                        ? $"{s.First().Supplier?.Institution?.NameAlt} ({s.First().Supplier?.Institution?.Root?.ShortNameAlt})"
                        : s.First().Supplier?.Complex?.NameAlt,
                    ShortName = s.First().Supplier?.Institution != null
                        ? $"{s.First().Supplier?.Institution?.ShortName} ({s.First().Supplier?.Institution?.Root?.ShortName})"
                        : s.First().Supplier?.Complex?.ShortName,
                    ShortNameAlt = s.First().Supplier?.Institution != null
                        ? $"{s.First().Supplier?.Institution?.ShortNameAlt} ({s.First().Supplier?.Institution?.Root?.ShortNameAlt})"
                        : s.First().Supplier?.Complex?.ShortNameAlt,
                    Count = s.Count(),
                    IsSelected = filter.SupplierIds.Contains(s.First().SupplierId)
                }).OrderByDescending(e => e.Count).ThenBy(e => e.Name).ToList();

            return list;
        }

        private async Task<List<FilterResultDto>> ConstructSmartSpecializationFilterResult(SupplierOfferingFilterDto filter, CancellationToken cancellationToken)
        {
            var tempSmartSpecializationIds = filter.SmartSpecializationRootIds;
            filter.SmartSpecializationRootIds = new List<int>();
            var supplierOfferings = await supplierOfferingRepository.GetList(filter, cancellationToken, e => e.Include(s => s.SmartSpecializations.OrderBy(e => e.SmartSpecialization.ViewOrder).ThenBy(e => e.SmartSpecialization.Name)).ThenInclude(m => m.SmartSpecialization.Root));
            filter.SmartSpecializationRootIds = tempSmartSpecializationIds;

            var list = supplierOfferings
                .SelectMany(e => e.SmartSpecializations.Select(s => s.SmartSpecialization.Root), (io, ss) => new { io, ss })
                .GroupBy(e => e.ss.Id)
                .Select(e => new FilterResultDto
                {
                    Id = e.First().ss.Id,
                    Code = e.First().ss.Code,
                    Name = e.First().ss.Name,
                    NameAlt = e.First().ss.NameAlt,
                    Count = e.Select(f => f.io.Id).Distinct().Count(),
                    IsSelected = filter.SmartSpecializationRootIds.Contains(e.First().ss.Id)
                }).OrderByDescending(e => e.Count).ThenBy(e => e.Code).ToList();


            return list;
        }
    }
}
