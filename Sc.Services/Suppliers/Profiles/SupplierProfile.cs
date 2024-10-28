using AutoMapper;
using Sc.Models.Dtos.Suppliers;
using Sc.Models.Dtos.Suppliers.Junctions;
using Sc.Models.Dtos.Suppliers.Search;
using Sc.Models.Dtos.Suppliers.Search.SupplierOfferingGroup;
using Sc.Models.Entities.Suppliers;
using Sc.Models.Entities.Suppliers.Junctions;
using Sc.Models.Enums.Suppliers;

namespace Sc.Services.Suppliers.Profiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile() 
        {
            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierRepresentative, SupplierRepresentativeDto>();
            CreateMap<SupplierTeam, SupplierTeamDto>();
            CreateMap<SupplierOffering, SupplierOfferingDto>();
            CreateMap<SupplierOfferingTeam, SupplierOfferingTeamDto>();
            CreateMap<SupplierOfferingFile, SupplierOfferingFileDto>();
            CreateMap<SupplierOfferingSmartSpecialization, SupplierOfferingSmartSpecializationDto>();
            CreateMap<SupplierEquipment, SupplierEquipmentDto>();
            CreateMap<SupplierEquipmentFile, SupplierEquipmentFileDto>();

            CreateMap<Supplier, SupplierSearchDto>()
                .ForMember(dest => dest.RootId, opt => opt.MapFrom(src => src.Institution.RootId))
                .ForPath(dest => dest.RootName, opt => opt.MapFrom(src => src.Institution.Root.Name))
                .ForPath(dest => dest.RootNameAlt, opt => opt.MapFrom(src => src.Institution.Root.NameAlt));

            CreateMap<SupplierOffering, SupplierOfferingSearchDto>()
                .ForPath(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Type == SupplierType.Institution ? src.Supplier.Institution.Name : src.Supplier.Complex.Name))
                .ForPath(dest => dest.SupplierNameAlt, opt => opt.MapFrom(src => src.Supplier.Type == SupplierType.Institution ? src.Supplier.Institution.NameAlt : src.Supplier.Complex.NameAlt))
                .ForPath(dest => dest.InstitutionId, opt => opt.MapFrom(src => src.Supplier.InstitutionId))
                .ForPath(dest => dest.RootInstitutionId, opt => opt.MapFrom(src => src.Supplier.Institution.RootId))
                .ForPath(dest => dest.RootInstitutionShortName, opt => opt.MapFrom(src => src.Supplier.Institution.Root.ShortName))
                .ForPath(dest => dest.RootInstitutionShortNameAlt, opt => opt.MapFrom(src => src.Supplier.Institution.Root.ShortNameAlt))
                .ForPath(dest => dest.RepresentativeUserName, opt => opt.MapFrom(src => src.Supplier.Representative.UserName))
                .ForPath(dest => dest.RepresentativeName, opt => opt.MapFrom(src => src.Supplier.Representative.Name))
                .ForPath(dest => dest.RepresentativeNameAlt, opt => opt.MapFrom(src => src.Supplier.Representative.NameAlt))
                .ForPath(dest => dest.RepresentativePhoneNumber, opt => opt.MapFrom(src => src.Supplier.Representative.PhoneNumber));

            CreateMap<SupplierDto, Supplier>();
            CreateMap<SupplierRepresentativeDto, SupplierRepresentative>();
            CreateMap<SupplierTeamDto, SupplierTeam>();
            CreateMap<SupplierOfferingDto, SupplierOffering>();
            CreateMap<SupplierOfferingTeamDto, SupplierOfferingTeam>();
            CreateMap<SupplierOfferingFileDto, SupplierOfferingFile>();
            CreateMap<SupplierOfferingSmartSpecializationDto, SupplierOfferingSmartSpecialization>();
            CreateMap<SupplierEquipmentDto, SupplierEquipment>();
            CreateMap<SupplierEquipment, SupplierEquipmentDto>();
            CreateMap<SupplierEquipmentFileDto, SupplierEquipmentFile>();
            CreateMap<SupplierOfferingEquipmentDto, SupplierOfferingEquipment>();
            CreateMap<SupplierOfferingEquipment, SupplierOfferingEquipmentDto>()
            .ForMember(dest => dest.SupplierOffering, opt => opt.MapFrom(src => src.SupplierOffering));
        }
    }
}
