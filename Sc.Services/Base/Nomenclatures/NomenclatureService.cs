using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Enums.Common;
using Sc.Models.Filters.Base;
using Sc.Repositories.Base;

namespace Sc.Services.Base.Nomenclatures
{
    public abstract class NomenclatureService<TEntity, TDto, TFilterDto, TRepository>
        where TEntity : Nomenclature, new()
        where TDto : IEntity
        where TFilterDto : IFilterDto<TEntity>
        where TRepository : IRepositoryBase<TEntity, TFilterDto>
    {
        protected readonly IMapper mapper;
        protected readonly TRepository repository;
        protected readonly DomainValidatorService domainValidatorService;

        public NomenclatureService(
            IMapper mapper,
            TRepository repository,
            DomainValidatorService domainValidatorService
            )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.domainValidatorService = domainValidatorService;
        }

        public virtual async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(TFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await repository.GetAll(filterDto, cancellationToken, repository.ConstructInclude(IncludeType.NavProperties), e => e.OrderBy(e => e.ViewOrder).ThenBy(e => e.Name));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public virtual async Task<TDto> GetDtoById(int id, CancellationToken cancellationToken)
        {
            var nomenlcature = await repository.GetById(id, cancellationToken, repository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<TDto>(nomenlcature);
        }

        public virtual async Task<List<TEntity>> GetList(TFilterDto filterDto, CancellationToken cancellationToken)
        {
            var result = await repository.GetList(filterDto, cancellationToken, repository.ConstructInclude(IncludeType.NavProperties), e => e.OrderBy(e => e.ViewOrder).ThenBy(e => e.Name));

            return result;
        }

        public virtual async Task<TDto> Create(TDto nomenclatureDto, CancellationToken cancellationToken)
        {
            var newNomenclature = mapper.Map<TEntity>(nomenclatureDto);
            await repository.Create(newNomenclature);

            return await GetDtoById(newNomenclature.Id, cancellationToken);
        }

        public virtual async Task<TDto> Update(TEntity entityForUpdate, TDto nomenclatureDto, CancellationToken cancellationToken)
        {
            await repository.UpdateFromDto(entityForUpdate, nomenclatureDto);

            return await GetDtoById(entityForUpdate.Id, cancellationToken);
        }

        public virtual async Task Delete(TEntity nomenclatureFroDelete)
        {
            try
            {
                await repository.Delete(nomenclatureFroDelete);
            }
            catch (Exception) 
            {
                domainValidatorService.ThrowErrorMessage(NomenclatureErrorCode.Nomenclature_Entity_HasReference);
            }
        }
    }
}
