using AutoMapper;
using Infrastructure;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherCertificateService
    {
        private readonly ScDbContext context;
        private readonly UserContext userContext;
        private readonly IMapper mapper;
        private readonly IReceivedVoucherRepository receivedVoucherRepository;
        private readonly IReceivedVoucherCertificateRepository certificateRepository;
        private readonly ReceivedVoucherCertificateFileService certificateFileService;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ReceivedVoucherNotificationService notificationService;

        public ReceivedVoucherCertificateService(
            ScDbContext context,
            UserContext userContext,
            IMapper mapper,
            IReceivedVoucherRepository receivedVoucherRepository,
            IReceivedVoucherCertificateRepository certificateRepository,
            ReceivedVoucherCertificateFileService certificateFileService,
            DomainValidatorService domainValidatorService,
            ReceivedVoucherNotificationService notificationService
            )
        {
            this.context = context;
            this.userContext = userContext;
            this.mapper = mapper;
            this.receivedVoucherRepository = receivedVoucherRepository;
            this.certificateRepository = certificateRepository;
            this.certificateFileService = certificateFileService;
            this.domainValidatorService = domainValidatorService;
            this.notificationService = notificationService;
        }

        public async Task<ReceivedVoucherCertificateDto> GetById(int id, CancellationToken cancellationToken)
        {
            var receivedVoucherCertificate = await certificateRepository.GetById(id, cancellationToken, certificateRepository.ConstructInclude(IncludeType.All));
            return mapper.Map<ReceivedVoucherCertificateDto>(receivedVoucherCertificate);
        }

        public async Task<ReceivedVoucherCertificateDto> GenerateCertificate(ReceivedVoucher receivedVoucher, ReceivedVoucherCertificateDto receivedVoucherCertificateDto, CancellationToken cancellationToken)
        {
            CreateValidation(receivedVoucher, receivedVoucherCertificateDto);

            using var transaction = context.BeginTransaction();

            receivedVoucherCertificateDto.UserId = userContext.UserId.Value;
            receivedVoucherCertificateDto.Username = userContext.UserName;
            receivedVoucherCertificateDto.UserFullname = userContext.FullName;
            receivedVoucherCertificateDto.File = await certificateFileService.GenerateCertificateFile(receivedVoucher, receivedVoucherCertificateDto);

            var newReceivedVoucherCertificate = mapper.Map<ReceivedVoucherCertificate>(receivedVoucherCertificateDto);
            await certificateRepository.Create(newReceivedVoucherCertificate);

            if (receivedVoucher.State == ReceivedVoucherState.Completed)
            {
                receivedVoucher.State = ReceivedVoucherState.GeneratedCertificate;

                await receivedVoucherRepository.SaveEntityChanges(receivedVoucher);
            }

            await notificationService.CreateNotifications(receivedVoucher.Id, DateTime.Now, NotificationType.ChangedState, ReceivedVoucherState.GeneratedCertificate, $"Генериран е сертификат {receivedVoucherCertificateDto.File.Name}", cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return await GetById(newReceivedVoucherCertificate.Id, cancellationToken);
        }

        private void CreateValidation(ReceivedVoucher receivedVoucher, ReceivedVoucherCertificateDto receivedVoucherCertificateDto)
        {
            if (receivedVoucher.SecondOfferingId.HasValue ? receivedVoucher.Certificates.Count > 1 : receivedVoucher.Certificates.Count > 0)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Certificate_AllAreGenerated);
            }

            if (receivedVoucher.Certificates.Any(e => e.OfferingId == receivedVoucherCertificateDto.OfferingId))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Certificate_AlreadyGenerated);
            }

            if (receivedVoucherCertificateDto.SupplierId != receivedVoucher.SupplierId && receivedVoucherCertificateDto.SupplierId != receivedVoucher.SecondSupplierId)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Certificate_InvalidSupplier);
            }

            if (receivedVoucherCertificateDto.OfferingId != receivedVoucher.OfferingId && receivedVoucherCertificateDto.OfferingId != receivedVoucher.SecondOfferingId)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Certificate_InvalidOffering);
            }
        }
    }
}
