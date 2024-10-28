using Infrastructure.Attributes;
using Infrastructure.Permissions;
using Infrastructure.Permissions.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sc.Models.Entities.Base;
using Sc.Models.Entities.Base.Nomenclatures;
using Sc.Models.Enums.Common;
using Sc.Models.Filters.Base;
using Sc.Repositories.Base;
using Sc.Services.Base.Nomenclatures;

namespace Server.Controllers.Base.Nomenclatures
{
    public abstract class NomenclatureController<TEntity, TDto, TSearchDto, TFilterDto, TRepository, TService> : NomenclatureSearchController<TEntity, TDto, TSearchDto, TFilterDto, TRepository, TService>
        where TEntity : Nomenclature, new()
        where TDto : IEntity
        where TSearchDto : class
        where TFilterDto : IFilterDto<TEntity>
        where TRepository : IRepositoryBase<TEntity, TFilterDto>
        where TService : NomenclatureService<TEntity, TDto, TFilterDto, TRepository>
    {
        private readonly PermissionService permissionService;
        private readonly TRepository repository;

        public NomenclatureController(
            TService service,
            PermissionService permissionService,
            TRepository repository
            ) : base(service)
        {
            this.permissionService = permissionService;
            this.repository = repository;
        }

        [Authorize, ScClient]
        [HttpPost]
        public virtual async Task<ActionResult<TDto>> Create([FromBody] TDto dto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.NomenclaturesCreatePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            return Ok(await service.Create(dto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpPut]
        public virtual async Task<ActionResult<TDto>> Update([FromBody] TDto dto, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.NomenclaturesWritePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var nomenclatureForUpdate = await repository.GetById(dto.Id, cancellationToken, repository.ConstructInclude(IncludeType.Collections));

            return Ok(await service.Update(nomenclatureForUpdate, dto, cancellationToken));
        }

        [Authorize, ScClient]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            permissionService.VerifyUnitPermissionException(PermissionConstants.NomenclaturesDeletePermission, new List<(string, int?)> { (OrganizationalUnitConstants.NacidAlias, null) });

            var nomenclatureForDelete = await repository.GetById(id, cancellationToken, repository.ConstructInclude(IncludeType.All));
            await service.Delete(nomenclatureForDelete);

            return Ok();
        }
    }
}
