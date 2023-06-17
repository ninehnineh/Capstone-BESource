using AutoMapper;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.UpdateCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Queries.GetCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.NonCensorshipManagerAccount.Queries.GetNonCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileById;
using Parking.FindingSlotManagement.Application.Features.Admin.BusinessProfile.BusinessProfileManagement.Queries.GetListBusinessProfile;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Commands.CreateNewPaypal;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetListPaypal;
using Parking.FindingSlotManagement.Application.Features.Admin.Paypal.PaypalManagement.Queries.GetPaypalByManagerId;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Commands.CreateNewVnPay;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByManagerId;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressById;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressByUserId;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Commands.CreateNewVehicleInfo;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetListVehicleInforByUserId;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfo.VehicleInfoManagement.Queries.GetVehicleInforById;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.StaffAccountManagement.Commands.CreateNewStaffAccount;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Commands.CreateNewBusinessProfile;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetBusinessProfileByUserId;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Commands.CreateNewPackagePrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetListParkingHasPriceWithPagination;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Queries.GetParkingHasPriceDetailWithPagination;
using Parking.FindingSlotManagement.Application.Features.Manager.PackagePrice.PackagePriceManagement.Queries.GetPackagePriceById;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Commands.CreateNewParking;
using Parking.FindingSlotManagement.Application.Features.Manager.StaffPakings.StaffParkingManagement.Commands.CreateNewStaffParking;
using Parking.FindingSlotManagement.Domain.Entities;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Commands.CreateNewParkingSpotImage;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSpotImage.ParkingSpotImageManagement.Queries.GetListImageByParkingId;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Commands.Create;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.CreateParkingPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Commands.DisableOrEnableParkingPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingPrice.Queries.GetAllParkingPrice;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Commands.CreateNewTimeline;
using Parking.FindingSlotManagement.Application.Features.Manager.Timeline.TimelineManagement.Queries.GetListTimelineByParkingPriceId;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestYou;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Commands.CreateVehicleInfoForGuest;
using Parking.FindingSlotManagement.Application.Features.Customer.VehicleInfoForGuest.VehicleInfoForGuestManagement.Queries.GetVehicleInfoForGuestById;
using Parking.FindingSlotManagement.Application.Features.Customer.Authentication.AuthenticationManagement.Commands.CustomerRegister;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetAvailableSlots;

namespace Parking.FindingSlotManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User Mapping
            CreateMap<User, CensorshipManagerAccountResponse>().ReverseMap();
            CreateMap<User, CreateNewCensorshipManagerAccountCommand>().ReverseMap();
            CreateMap<User, UpdateCensorshipManagerAccountCommand>().ReverseMap();
            CreateMap<User, RequestResponse>().ReverseMap();
            CreateMap<User, NonCensorshipManagerAccountResponse>().ReverseMap();
            CreateMap<User, CreateNewStaffAccountCommand>().ReverseMap();
            CreateMap<User, CustomerRegisterCommand>().ReverseMap();
            #endregion

            #region Traffic Mapping
            CreateMap<Traffic, CreateNewTrafficCommand>().ReverseMap();
            CreateMap<Traffic, GetListTrafficResponse>().ReverseMap();
            CreateMap<Traffic, GetTrafficResponse>().ReverseMap();
            #endregion

            #region BusinessProfile Mapping
            CreateMap<BusinessProfile, GetBusinessProfileByIdResponse>().ReverseMap();
            CreateMap<BusinessProfile, GetListBusinessProfileResponse>()
                .ForMember(dto => dto.UserName, act => act.MapFrom(obj => obj.User.Name))
                .ReverseMap();
            CreateMap<BusinessProfile, CreateNewBusinessProfileCommand>().ReverseMap();
            CreateMap<BusinessProfile, GetBusinessProfileResponse>().ReverseMap();
            #endregion

            #region VnPay Mapping
            CreateMap<VnPay, CreateNewVnPayCommand>().ReverseMap();
            CreateMap<VnPay, GetVnPayByManagerIdResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            #endregion

            #region PayPal Mapping
            CreateMap<PayPal, CreateNewPaypalCommand>().ReverseMap();
            CreateMap<PayPal, GetPaypalByManagerIdResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            CreateMap<PayPal, GetListPaypalResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            #endregion

            #region Parking Mapping
            CreateMap<Domain.Entities.Parking, CreateNewParkingCommand>().ReverseMap();
            CreateMap<Domain.Entities.Parking, GetListParkingNearestYouQueryResponse>().ReverseMap();
            #endregion

            #region StaffParking Mapping
            CreateMap<StaffParking, CreateNewStaffParkingCommand>().ReverseMap();
            #endregion

            #region Floor Mapping
            CreateMap<Floor, CreateNewFloorCommand>().ReverseMap();
            CreateMap<Floor, GetListFloorResponse>().ReverseMap();
            #endregion

            #region TimeLine Mapping
            CreateMap<TimeLine, CreateNewPackagePriceCommand>().ReverseMap();
            //CreateMap<TimeLine, GetPackagePriceByIdResponse>()
            //    .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name))
            //    .ReverseMap();
            #endregion

            #region ParkingSpotImage Mapping
            CreateMap<ParkingSpotImage, CreateNewParkingSpotImageCommand>().ReverseMap();
            CreateMap<ParkingSpotImage, GetListImageByParkingIdResponse>().ReverseMap();
            #endregion

            #region ParkingPrice Mapping
            CreateMap<ParkingPrice, CreateParkingPriceCommand>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
            CreateMap<ParkingPrice, DisableOrEnableParkingPriceCommand>().ReverseMap();
            CreateMap<ParkingPrice, GetAllParkingPriceQueryResponse>().ReverseMap();
            #endregion

            #region FavoriteAddress Mapping
            CreateMap<FavoriteAddress, CreateNewFavoriteAddressCommand>().ReverseMap();
            CreateMap<FavoriteAddress, GetFavoriteAddressByIdResponse>().ReverseMap();
            CreateMap<FavoriteAddress, GetFavoriteAddressByUserIdResponse>().ReverseMap();
            #endregion

            #region VehicleInfor Mapping
            CreateMap<VehicleInfor, VehicleInfoCommand>().ReverseMap();
            CreateMap<VehicleInfor, GetVehicleInforByIdResponse>().ReverseMap();
            CreateMap<VehicleInfor, GetListVehicleInforByUserIdResponse>()
                .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name)).ReverseMap();
            CreateMap<VehicleInfor, VehicleInfoForGuestCommand>().ReverseMap();
            CreateMap<VehicleInfor, GetVehicleInfoForGuestByIdResponse>().ReverseMap();
            #endregion

            #region ParkingHasPrice Mapping
            CreateMap<ParkingHasPrice, GetListParkingHasPriceWithPaginationResponse>()
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.Parking.Name))
                .ForMember(dto => dto.ParkingPriceName, act => act.MapFrom(obj => obj.ParkingPrice.ParkingPriceName))
                .ReverseMap();
            CreateMap<ParkingHasPrice, GetParkingHasPriceDetailWithPaginationResponse>()
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.Parking.Name))
                .ForMember(dto => dto.ParkingPriceName, act => act.MapFrom(obj => obj.ParkingPrice.ParkingPriceName))
                .ReverseMap();
            CreateMap<ParkingHasPrice, CreateParkingHasPriceCommand>().ReverseMap();
            #endregion

            #region Parkingslots Mapping
            CreateMap<ParkingSlot, CreateParkingSlotsCommand>().ReverseMap();
            CreateMap<ParkingSlot, GetAvailableSlotsResponse>().ReverseMap();
            #endregion

            #region Timeline Mapping
            CreateMap<TimeLine, CreateNewTimelineCommandMapper>().ReverseMap();
            CreateMap<TimeLine, GetListTimelineByParkingPriceIdResponse>().ReverseMap();
            #endregion

            #region Booking Mapping
            CreateMap<Booking, BookingDto>().ReverseMap();
            #endregion
        }
    }
}
