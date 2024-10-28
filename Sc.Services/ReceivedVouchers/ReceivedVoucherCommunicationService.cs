using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Sc.Models;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Notifications;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;
using Sc.SignalR.Hubs;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherCommunicationService
    {
        private readonly IMapper mapper;
        private readonly IReceivedVoucherCommunicationRepository communicationRepository;
        private readonly ReceivedVoucherNotificationService notificationService;
        private readonly UserContext userContext;
        private readonly ScDbContext context;
        private readonly IHubContext<RvCommunicationHub, ICommunicationHub<ReceivedVoucherCommunicationDto>> communicationHub;

        public ReceivedVoucherCommunicationService(
            IMapper mapper,
            IReceivedVoucherCommunicationRepository communicationRepository,
            ReceivedVoucherNotificationService notificationService,
            UserContext userContext,
            ScDbContext context,
            IHubContext<RvCommunicationHub, ICommunicationHub<ReceivedVoucherCommunicationDto>> communicationHub
            )
        {
            this.mapper = mapper;
            this.communicationRepository = communicationRepository;
            this.notificationService = notificationService;
            this.userContext = userContext;
            this.context = context;
            this.communicationHub = communicationHub;
        }

        public async Task<List<ReceivedVoucherCommunicationDto>> GetCommunications(ReceivedVoucherCommunicationFilterDto filterDto, CancellationToken cancellationToken)
        {
            var result = await communicationRepository.GetList(filterDto, cancellationToken, communicationRepository.ConstructInclude(IncludeType.None), e => e.OrderByDescending(e => e.CreateDate));

            return mapper.Map<List<ReceivedVoucherCommunicationDto>>(result.OrderBy(e => e.CreateDate));
        }

        public async Task<ReceivedVoucherCommunicationDto> SendMessage(int receivedVoucherId, ReceivedVoucherCommunicationDto currentCommunicationDto, CancellationToken cancellationToken)
        {
            using var transaction = context.BeginTransaction();

            var createDate = DateTime.Now;
            currentCommunicationDto.CreateDate = createDate;
            currentCommunicationDto.FromUserId = userContext.UserId.Value;
            currentCommunicationDto.FromUsername = userContext.UserName;
            currentCommunicationDto.FromFullname = userContext.FullName;
            currentCommunicationDto.EntityId = receivedVoucherId;

            var newCommunication = mapper.Map<ReceivedVoucherCommunication>(currentCommunicationDto);
            await communicationRepository.Create(newCommunication);

            var communicationDto = mapper.Map<ReceivedVoucherCommunicationDto>(await communicationRepository.GetById(newCommunication.Id, cancellationToken));
            
            await communicationHub
            .Clients
            .Group(receivedVoucherId.ToString())
                .SendText(communicationDto);

            await notificationService.CreateNotifications(communicationDto.EntityId, createDate, NotificationType.Message, null, communicationDto.Text, cancellationToken);

            await transaction.CommitAsync();

            return communicationDto;
        }
    }
}
