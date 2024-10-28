using Infrastructure.DomainValidation;
using Sc.Models.Entities.Base;

namespace Sc.Services.Base
{
    public abstract class ValidateService<TEntity, TDto>
        where TEntity : IEntityVersion
        where TDto : IEntity
    {
        protected readonly DomainValidatorService domainValidatorService;

        public ValidateService(
            DomainValidatorService domainValidatorService
            )
        {
            this.domainValidatorService = domainValidatorService;
        }

        protected virtual async Task CreateValidation(TDto dto, CancellationToken cancellationToken) { await Task.CompletedTask; }
        protected virtual async Task UpdateValidation(TEntity entity, TDto dto, CancellationToken cancellationToken) { await Task.CompletedTask; }
    }
}
