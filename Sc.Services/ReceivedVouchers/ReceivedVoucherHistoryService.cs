using AutoMapper;
using Sc.Models.Dtos.Base.Search;
using Sc.Models.Entities.ReceivedVouchers;
using Sc.Models.Enums.Common;
using Sc.Models.FilterDtos.ReceivedVouchers;
using Sc.Repositories.Helpers;
using Sc.Repositories.ReceivedVouchers;

namespace Sc.Services.ReceivedVouchers
{
    public class ReceivedVoucherHistoryService
    {
        private readonly IMapper mapper;
        private readonly IReceivedVoucherHistoryRepository receivedVoucherHistoryRepository;

        public ReceivedVoucherHistoryService(
            IMapper mapper,
            IReceivedVoucherHistoryRepository receivedVoucherHistoryRepository
            )
        {
            this.mapper = mapper;
            this.receivedVoucherHistoryRepository = receivedVoucherHistoryRepository;
        }

        public async Task<SearchResultDto<TSearchDto>> GetSearchResultDto<TSearchDto>(ReceivedVoucherHistoryFilterDto filterDto, CancellationToken cancellationToken)
            where TSearchDto : class
        {
            var (result, count) = await receivedVoucherHistoryRepository.GetAll(filterDto, cancellationToken, receivedVoucherHistoryRepository.ConstructInclude(IncludeType.NavProperties), e => e.OrderByDescending(e => e.Id));

            var searchResult = new SearchResultDto<TSearchDto>
            {
                Result = mapper.Map<List<TSearchDto>>(result),
                TotalCount = count
            };

            return searchResult;
        }

        public async Task Create(ReceivedVoucher receivedVoucher)
        {
            var newHistory = new ReceivedVoucherHistory
            {
                ReceivedVoucherId = receivedVoucher.Id
            };

            EntityHelper.CloneProperties(receivedVoucher, newHistory);
            await receivedVoucherHistoryRepository.Create(newHistory);
        }
    }
}
