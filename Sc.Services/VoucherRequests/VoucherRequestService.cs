using AutoMapper;
using Sc.Models;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.Entities.VoucherRequests;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Notifications;
using Sc.Models.Enums.VoucherRequests;
using Sc.Models.FilterDtos.VoucherRequests;
using Sc.Repositories.VoucherRequests;

namespace Sc.Services.VoucherRequests
{
    public class VoucherRequestService
    {
        private readonly IMapper mapper;
        private readonly IVoucherRequestRepository voucherRequestRepository;
        private readonly VoucherRequestNotificationService notificationService;
        private readonly ScDbContext context;

        public VoucherRequestService(
            IMapper mapper,
            IVoucherRequestRepository voucherRequestRepository,
            VoucherRequestNotificationService notificationService,
            ScDbContext context
            )
        {
            this.mapper = mapper;
            this.voucherRequestRepository = voucherRequestRepository;
            this.notificationService = notificationService;
            this.context = context;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(VoucherRequestFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await voucherRequestRepository.GetAll(filterDto, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties), e => e.OrderByDescending(e => e.CreateDate));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task<VoucherRequestDto> GetById(int id, CancellationToken cancellationToken)
        {
            var voucherRequest = await voucherRequestRepository.GetById(id, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<VoucherRequestDto>(voucherRequest);
        }

        public async Task<VoucherRequestDto> GetBySupplierOfferingCompany(int supplierOfferingId, int companyId, CancellationToken cancellationToken)
        {
            var voucherRequest = await voucherRequestRepository.GetByProperties(e => e.SupplierOfferingId == supplierOfferingId && e.RequestCompanyId == companyId, cancellationToken, voucherRequestRepository.ConstructInclude(IncludeType.NavProperties));

            return mapper.Map<VoucherRequestDto>(voucherRequest);
        }

        public async Task<ChangedStateDto> ChangeState(VoucherRequest voucherRequest, VoucherRequestStateDto voucherRequestStateDto, CancellationToken cancellationToken)
        {
            using var transaction = context.BeginTransaction();

            voucherRequest.State = voucherRequestStateDto.State;

            if (voucherRequest.State == VoucherRequestState.Generated && string.IsNullOrWhiteSpace(voucherRequest.Code))
            {
                voucherRequest.Code = await GenerateCode(cancellationToken);
                await notificationService.CreateNotifications(voucherRequest.Id, DateTime.Now, NotificationType.GeneratedCode, null, voucherRequest.Code, $"За заявката е генериран код: {voucherRequest.Code}", cancellationToken);
            }

            await voucherRequestRepository.SaveEntityChanges(voucherRequest);

            await notificationService.CreateNotifications(voucherRequest.Id, DateTime.Now, NotificationType.ChangedState, voucherRequest.State, null, "Статусът на заявката за код е променен", cancellationToken);

            await transaction.CommitAsync();

            return new ChangedStateDto { State = voucherRequest.State, Code = voucherRequest.Code };
        }

        private async Task<string> GenerateCode(CancellationToken cancellationToken)
        {
            var random = new Random();
            string acceptedChars = "ABCDEFGHKLMNPQRSTUVWXYZ0123456789";

            var voucherRequestsWithCode = await voucherRequestRepository.GetList(new VoucherRequestFilterDto
            {
                GetAllData = true,
                HasGeneratedCode = true
            }, cancellationToken);

            var generatedCodes = voucherRequestsWithCode.Select(e => e.Code).ToHashSet();

            var code = string.Empty;

            while (string.IsNullOrWhiteSpace(code) || generatedCodes.Contains(code))
            {
                code = new string(Enumerable.Repeat(acceptedChars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            return code;
        }
    }
}
