using Infrastructure.AppSettings;
using Infrastructure.Attributes;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Infrastructure.DomainValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;
using Sc.Services.ReceivedVouchers;
using Sc.Services.ReceivedVouchers.Permissions;

namespace Server.Controllers.ReceivedVouchers
{
    [ApiController]
    [Route("api/receivedVouchers/{receivedVoucherId:int}/certificates")]
    public class ReceivedVoucherCertificateController : ControllerBase
    {
        private readonly ReceivedVoucherCertificateService certificateService;
        private readonly IReceivedVoucherRepository receivedVoucherRepository;
        private readonly ReceivedVoucherPermissionService receivedVoucherPermissionService;
        private readonly DomainValidatorService domainValidatorService;

        public ReceivedVoucherCertificateController(
            ReceivedVoucherCertificateService certificateService,
            IReceivedVoucherRepository receivedVoucherRepository,
            ReceivedVoucherPermissionService receivedVoucherPermissionService,
            DomainValidatorService domainValidatorService
            )
        {
            this.certificateService = certificateService;
            this.receivedVoucherRepository = receivedVoucherRepository;
            this.receivedVoucherPermissionService = receivedVoucherPermissionService;
            this.domainValidatorService = domainValidatorService;
        }

        [Authorize, ScClient]
        [HttpPost]
        public async Task<ActionResult<ReceivedVoucherCertificateDto>> GenerateCertificate([FromBody] ReceivedVoucherCertificateDto receivedVoucherCertificateDto, CancellationToken cancellationToken)
        {
            if (!AppSettingsProvider.UseAllFunctionalities)
            {
                domainValidatorService.ThrowErrorMessage(SystemErrorCode.System_FunctionalityUnavailible);
            }

            var receivedVoucher = await receivedVoucherRepository.GetById(receivedVoucherCertificateDto.ReceivedVoucherId, cancellationToken, 
                e => e.Include(s => s.Certificates));

            await receivedVoucherPermissionService.VerifyCertificateGenerationPermissionException(receivedVoucherCertificateDto, cancellationToken);

            return Ok(await certificateService.GenerateCertificate(receivedVoucher, receivedVoucherCertificateDto, cancellationToken));
        }
    }
}
