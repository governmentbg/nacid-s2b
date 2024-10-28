using Microsoft.AspNetCore.Mvc;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Filters.Base;
using Sc.Repositories.Base;
using Sc.Services.Base.Nomenclatures;

namespace Server.Controllers.Base.Nomenclatures
{
    public abstract class NomenclatureSearchController<TEntity, TDto, TSearchDto, TFilterDto, TRepository, TService> : ControllerBase
        where TEntity : Nomenclature, new()
        where TDto : IEntity
        where TSearchDto : class
        where TFilterDto : IFilterDto<TEntity>
        where TRepository : IRepositoryBase<TEntity, TFilterDto>
        where TService : NomenclatureService<TEntity, TDto, TFilterDto, TRepository>
    {
        protected readonly TService service;

        public NomenclatureSearchController(
            TService service
            )
        {
            this.service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await service.GetDtoById(id, cancellationToken));
        }

        [HttpPost("search")]
        public virtual async Task<ActionResult<SearchResultDto<TSearchDto>>> GetSearchResultDto([FromBody] TFilterDto filter, CancellationToken cancellationToken)
        {
            return Ok(await service.GetSearchResultDto<TSearchDto>(filter, cancellationToken));
        }
    }
}
