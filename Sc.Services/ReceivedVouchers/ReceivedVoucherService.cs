using AutoMapper;
using Infrastructure.DomainValidation;
using Infrastructure.DomainValidation.Models.ErrorCodes;
using Sc.Models;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.ReceivedVouchers;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.ReceivedVouchers;
using Sc.Services.Base;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherService : ValidateService<ReceivedVoucher, ReceivedVoucherDto>
    {
        private readonly ScDbContext context;
        private readonly IMapper mapper;
        private readonly IReceivedVoucherRepository receivedVoucherRepository;
        private readonly ReceivedVoucherHistoryService receivedVoucherHistoryService;
        private readonly ReceivedVoucherNotificationService notificationService;

        public ReceivedVoucherService(
            ScDbContext context,
            IMapper mapper,
            IReceivedVoucherRepository receivedVoucherRepository,
            ReceivedVoucherHistoryService receivedVoucherHistoryService,
            DomainValidatorService domainValidatorService,
            ReceivedVoucherNotificationService notificationService
            ) : base(domainValidatorService)
        {
            this.context = context;
            this.mapper = mapper;
            this.receivedVoucherHistoryService = receivedVoucherHistoryService;
            this.receivedVoucherRepository = receivedVoucherRepository;
            this.notificationService = notificationService;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(ReceivedVoucherFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await receivedVoucherRepository.GetAll(filterDto, cancellationToken, receivedVoucherRepository.ConstructInclude(IncludeType.NavProperties), e => e.OrderByDescending(e => e.ContractDate));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<ReceivedVoucherDto> GetById(int id, IncludeType includeType, CancellationToken cancellationToken)
        {
            var receivedVoucher = await receivedVoucherRepository.GetById(id, cancellationToken, receivedVoucherRepository.ConstructInclude(includeType));
            return mapper.Map<ReceivedVoucherDto>(receivedVoucher);
        }

        public async Task Create(ReceivedVoucherDto receivedVoucherDto, CancellationToken cancellationToken)
        {
            receivedVoucherDto.ValidateProperties(domainValidatorService);

            using var transaction = context.BeginTransaction();

            await CreateValidation(receivedVoucherDto, cancellationToken);

            var newReceivedVoucher = mapper.Map<ReceivedVoucher>(receivedVoucherDto);
            await receivedVoucherRepository.Create(newReceivedVoucher);

            await notificationService.CreateNotifications(newReceivedVoucher.Id, DateTime.Now, NotificationType.ChangedState, newReceivedVoucher.State, $"Добавен е нов ваучер с номер на договор {newReceivedVoucher.ContractNumber}", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        public async Task<ReceivedVoucherDto> Update(ReceivedVoucher receivedVoucherForUpdate, ReceivedVoucherDto receivedVoucherDto, CancellationToken cancellationToken)
        {
            receivedVoucherDto.ValidateProperties(domainValidatorService);
            await UpdateValidation(receivedVoucherForUpdate, receivedVoucherDto, cancellationToken);

            using var transaction = context.BeginTransaction();

            await receivedVoucherHistoryService.Create(receivedVoucherForUpdate);
            await receivedVoucherRepository.UpdateFromDto(receivedVoucherForUpdate, receivedVoucherDto);

            await notificationService.CreateNotifications(receivedVoucherForUpdate.Id, DateTime.Now, NotificationType.ChangedState, receivedVoucherForUpdate.State, $"Редактиран е ваучер с номер на договор {receivedVoucherForUpdate.ContractNumber}", cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return await GetById(receivedVoucherForUpdate.Id, IncludeType.All, cancellationToken);
        }

        public async Task<ReceivedVoucherDto> TerminateVoucher(int id, CancellationToken cancellationToken)
        {
            var receivedVoucher = await receivedVoucherRepository.GetById(id, cancellationToken, receivedVoucherRepository.ConstructInclude(IncludeType.All));

            using var transaction = context.BeginTransaction();

            receivedVoucher.State = ReceivedVoucherState.Terminated;

            await receivedVoucherRepository.SaveEntityChanges(receivedVoucher);

            await notificationService.CreateNotifications(receivedVoucher.Id, DateTime.Now, NotificationType.ChangedState, ReceivedVoucherState.Terminated, $"Прекратен е ваучер с номер на договор {receivedVoucher.ContractNumber}", cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return await GetById(id, IncludeType.All, cancellationToken);
        }

        public async Task<int> GetReceivedVoucherCountAsync(int userId, CancellationToken cancellationToken)
        {
            var vouchers = await receivedVoucherRepository.GetListByProperties(v => v.CompanyUserId == userId, cancellationToken);

            return vouchers.Count;
        }


        #region IValidationService
        protected async override Task CreateValidation(ReceivedVoucherDto dto, CancellationToken cancellationToken)
        {
            if (await receivedVoucherRepository.AnyEntity(e => e.ContractNumber.Trim().ToLower() == dto.ContractNumber.Trim().ToLower(), cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Entity_Exists);
            }

            if ((dto.Offering != null && !dto.Offering.IsActive)
                || (dto.SecondOffering != null && !dto.SecondOffering.IsActive))
            {
                domainValidatorService.ThrowErrorMessage(SupplierOfferingErrorCode.SupplierOffering_Entity_Inactive);
            }
        }

        protected async override Task UpdateValidation(ReceivedVoucher entity, ReceivedVoucherDto dto, CancellationToken cancellationToken)
        {
            if (entity.State != ReceivedVoucherState.Draft)
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_State_Required);
            }

            if (await receivedVoucherRepository
                .AnyEntity(e => e.ContractNumber.Trim().ToLower() == dto.ContractNumber.Trim().ToLower()
                    && e.Id != dto.Id, cancellationToken))
            {
                domainValidatorService.ThrowErrorMessage(ReceivedVoucherErrorCode.ReceivedVoucher_Entity_Exists);
            }
        }
        #endregion
    }
}
