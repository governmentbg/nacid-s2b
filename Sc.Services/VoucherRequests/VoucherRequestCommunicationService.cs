using AutoMapper;
using Infrastructure;
using Infrastructure.DomainValidation;
using Microsoft.AspNetCore.SignalR;
using Sc.Models;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.VoucherRequests;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.VoucherRequests;
using Sc.SignalR.Hubs;

namespace Sc.Services.VoucherRequests
{
    public class VoucherRequestCommunicationService
    {
        private readonly IMapper mapper;
        private readonly IVoucherRequestRepository voucherRequestRepository;
        private readonly IVoucherRequestCommunicationRepository communicationRepository;
        private readonly VoucherRequestNotificationService notificationService;
        private readonly UserContext userContext;
        private readonly DomainValidatorService domainValidatorService;
        private readonly ScDbContext context;
        private readonly IHubContext<VrCommunicationHub, ICommunicationHub<VoucherRequestCommunicationDto>> communicationHub;

        public VoucherRequestCommunicationService(
            IMapper mapper,
            IVoucherRequestRepository voucherRequestRepository,
            IVoucherRequestCommunicationRepository communicationRepository,
            VoucherRequestNotificationService notificationService,
            UserContext userContext,
            DomainValidatorService domainValidatorService,
            ScDbContext context,
            IHubContext<VrCommunicationHub, ICommunicationHub<VoucherRequestCommunicationDto>> communicationHub)
        {
            this.mapper = mapper;
            this.voucherRequestRepository = voucherRequestRepository;
            this.communicationRepository = communicationRepository;
            this.notificationService = notificationService;
            this.userContext = userContext;
            this.domainValidatorService = domainValidatorService;
            this.context = context;
            this.communicationHub = communicationHub;
        }

        public async Task<List<VoucherRequestCommunicationDto>> GetCommunications(VoucherRequestCommunicationFilterDto filterDto, CancellationToken cancellationToken)
        {
            var result = await communicationRepository.GetList(filterDto, cancellationToken, communicationRepository.ConstructInclude(IncludeType.None), e => e.OrderByDescending(e => e.CreateDate));

            return mapper.Map<List<VoucherRequestCommunicationDto>>(result.OrderBy(e => e.CreateDate));
        }

        public async Task<VoucherRequestCommunicationDto> SendInitialMessage(VoucherRequestCommunicationDto currentCommunicationDto, CancellationToken cancellationToken)
        {
            currentCommunicationDto.ValidateProperties(domainValidatorService);

            using var transaction = context.BeginTransaction();

            var newCommunicationRoomName = currentCommunicationDto.Entity.SupplierOfferingId.ToString() + currentCommunicationDto.Entity.RequestCompanyId.ToString();

            var createDate = DateTime.Now;
            currentCommunicationDto.CreateDate = createDate;
            currentCommunicationDto.FromUserId = userContext.UserId.Value;
            currentCommunicationDto.FromUsername = userContext.UserName;
            currentCommunicationDto.FromFullname = userContext.FullName;
            currentCommunicationDto.Entity.CreateDate = createDate;
            currentCommunicationDto.Entity.State = VoucherRequestState.Draft;
            currentCommunicationDto.Entity.RequestUserId = userContext.UserId.Value;

            var newCommunication = mapper.Map<VoucherRequestCommunication>(currentCommunicationDto);
            await communicationRepository.Create(newCommunication);

            var communicationDto = mapper.Map<VoucherRequestCommunicationDto>(await communicationRepository.GetById(newCommunication.Id, cancellationToken));

            await communicationHub
                .Clients
                .Group(newCommunicationRoomName)
                .SendText(communicationDto);

            await notificationService.CreateNotifications(communicationDto.EntityId, createDate, NotificationType.Message, null, null, communicationDto.Text, cancellationToken);

            await transaction.CommitAsync();

            return communicationDto;
        }

        public async Task<VoucherRequestCommunicationDto> SendMessage(int voucherRequestId, VoucherRequestState currentState, VoucherRequestCommunicationDto currentCommunicationDto, CancellationToken cancellationToken)
        {
            currentCommunicationDto.ValidateProperties(domainValidatorService);

            var voucherRequest = await voucherRequestRepository.GetById(voucherRequestId, cancellationToken);

            using var transaction = context.BeginTransaction();

            var createDate = DateTime.Now;
            currentCommunicationDto.CreateDate = createDate;
            currentCommunicationDto.FromUserId = userContext.UserId.Value;
            currentCommunicationDto.FromUsername = userContext.UserName;
            currentCommunicationDto.FromFullname = userContext.FullName;
            currentCommunicationDto.EntityId = voucherRequestId;
            currentCommunicationDto.Entity = null;

            var newCommunication = mapper.Map<VoucherRequestCommunication>(currentCommunicationDto);
            await communicationRepository.Create(newCommunication);

            var communicationDto = mapper.Map<VoucherRequestCommunicationDto>(await communicationRepository.GetById(newCommunication.Id, cancellationToken));

            await communicationHub
                .Clients
                .Group(voucherRequest.SupplierOfferingId.ToString() + voucherRequest.RequestCompanyId.ToString())
                .SendText(communicationDto);

            if (currentState == VoucherRequestState.Draft && (!userContext.CompanyId.HasValue || userContext.CompanyId == 0))
            {
                voucherRequest.State = VoucherRequestState.Pending;

                await voucherRequestRepository.SaveEntityChanges(voucherRequest);

                await notificationService.CreateNotifications(communicationDto.EntityId, createDate, NotificationType.ChangedState, voucherRequest.State, null, "Статусът на заявката за код е променен", cancellationToken);
            }
            else
            {
                await notificationService.CreateNotifications(communicationDto.EntityId, createDate, NotificationType.Message, null, null, communicationDto.Text, cancellationToken);
            }

            await transaction.CommitAsync();

            return communicationDto;
        }
    }
}
