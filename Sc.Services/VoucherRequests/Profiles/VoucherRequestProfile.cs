using AutoMapper;
using Sc.Models.Dtos.VoucherRequests;
using Sc.Models.Entities.VoucherRequests;

namespace Sc.Services.VoucherRequests.Profiles
{
    public class VoucherRequestProfile : Profile
    {
        public VoucherRequestProfile()
        {
            CreateMap<VoucherRequest, VoucherRequestDto>();
            CreateMap<VoucherRequestCommunication, VoucherRequestCommunicationDto>();

            CreateMap<VoucherRequestDto, VoucherRequest>();
            CreateMap<VoucherRequestCommunicationDto, VoucherRequestCommunication>();
        }
    }
}
