using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.Nomenclatures.Institutions;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.Nomenclatures.Institutions;
using Sc.Repositories.Nomenclatures.Institutions;

namespace Sc.Services.Nomenclatures.Institutions
{
    public class InstitutionService
    {
        private readonly IMapper mapper;
        private readonly IInstitutionRepository institutionRepository;

        public InstitutionService(
            IMapper mapper,
            IInstitutionRepository institutionRepository
            )
        {
            this.mapper = mapper;
            this.institutionRepository = institutionRepository;
        }

        public async Task<InstitutionDto> GetById(int id, CancellationToken cancellationToken)
        {
            var institution = await institutionRepository.GetById(id, cancellationToken, institutionRepository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<InstitutionDto>(institution);
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(InstitutionFilterDto filterDto, IncludeType includeType, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await institutionRepository.GetAll(filterDto, cancellationToken, institutionRepository.ConstructInclude(includeType), e => e.OrderBy(e => e.ParentId.HasValue).ThenBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<List<InstitutionDto>> GetSubordinates(int parentId, CancellationToken cancellationToken)
        {
            var institutionFilterDto = new InstitutionFilterDto
            {
                GetAllData = true,
                ParentId = parentId
            };

            var subordinatesList = await institutionRepository.GetList(institutionFilterDto, cancellationToken, e => e.Include(s => s.Settlement), e => e.OrderBy(e => e.Name));

            return mapper.Map<List<InstitutionDto>>(subordinatesList);
        }
    }
}
