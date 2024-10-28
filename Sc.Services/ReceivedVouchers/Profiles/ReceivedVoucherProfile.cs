using AutoMapper;
using Sc.Models.Dtos.ReceivedVouchers;
using Sc.Models.Entities.ReceivedVouchers;

namespace Sc.Services.ReceivedVouchers.Profiles
{
    public class ReceivedVoucherProfile : Profile
    {
        public ReceivedVoucherProfile()
        {
            CreateMap<ReceivedVoucher, ReceivedVoucherDto>()
                .ForMember(dest => dest.HistoriesCount, opt => opt.MapFrom(src => src.Histories.Count));
            CreateMap<ReceivedVoucherHistory, ReceivedVoucherHistoryDto>();
            CreateMap<ReceivedVoucherFile, ReceivedVoucherFileDto>();
            CreateMap<ReceivedVoucherHistoryFile, ReceivedVoucherHistoryFileDto>();
            CreateMap<ReceivedVoucherCertificate, ReceivedVoucherCertificateDto>();
            CreateMap<ReceivedVoucherCertificateFile, ReceivedVoucherCertificateFileDto>();
            CreateMap<ReceivedVoucherCommunication, ReceivedVoucherCommunicationDto>();

            CreateMap<ReceivedVoucherDto, ReceivedVoucher>();
            CreateMap<ReceivedVoucherHistoryDto, ReceivedVoucherHistory>();
            CreateMap<ReceivedVoucherFileDto, ReceivedVoucherFile>();
            CreateMap<ReceivedVoucherHistoryFileDto, ReceivedVoucherHistoryFile>();
            CreateMap<ReceivedVoucherCertificateDto, ReceivedVoucherCertificate>();
            CreateMap<ReceivedVoucherCertificateFileDto, ReceivedVoucherCertificateFile>();
            CreateMap<ReceivedVoucherCommunicationDto, ReceivedVoucherCommunication>();
        }
    }
}
