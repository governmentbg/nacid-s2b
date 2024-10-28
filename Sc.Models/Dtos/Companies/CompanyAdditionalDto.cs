using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Entities.Base;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.Companies
{
    public class CompanyAdditionalDto : Entity, IValidate
    {
        public CompanyDto Company { get; set; }

        public uint StaffCount { get; set; }
        public decimal AnnualTurnover { get; set; }
        public string WebPage { get; set; }

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            WebPage = WebPage?.Trim();

            if (StaffCount < 1 || StaffCount % 1 != 0 || StaffCount > 9999)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.CompanyAdditional_StaffCount_Invalid);
            }

            if (AnnualTurnover < 1 || AnnualTurnover > 500000)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.CompanyAdditional_AnnualTurnover_Invalid);
            }

            if (AnnualTurnover <= 0)
            {
                domainValidatorService.ThrowErrorMessage(CompanyErrorCode.CompanyAdditional_AnnualTurnover_Invalid);
            }
        }
    }
}
