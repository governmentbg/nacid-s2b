using Infrastructure.DomainValidation;

namespace Sc.Models.Interfaces
{
    public interface IValidate
    {
        void ValidateProperties(DomainValidatorService domainValidatorService);
    }
}
