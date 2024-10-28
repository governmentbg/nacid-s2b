using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models.Dtos.Notifications;
using Sc.Models.Interfaces;

namespace Sc.Models.Dtos.VoucherRequests
{
    public class VoucherRequestCommunicationDto : BaseCommunicationDto, IValidate
    {
        public VoucherRequestDto Entity { get; set; }

        public void ValidateProperties(DomainValidatorService domainValidatorService)
        {
            if (Entity == null || Entity.SupplierOfferingId == 0 || Entity.RequestCompanyId == 0)
            {
                domainValidatorService.ThrowErrorMessage(VoucherRequestCommunicationErrorCode.VoucherRequestCommunication_SupplierOfferingCompany_Required);
            }
        }
    }
}
