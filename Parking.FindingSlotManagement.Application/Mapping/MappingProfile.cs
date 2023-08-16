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
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayByUserId;
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
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetListStaffAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.StaffAccountManagement.Queries.GetStaffAccountById;
using Parking.FindingSlotManagement.Application.Features.Manager.Account.RegisterCensorshipBusinessAccount.Commands.RegisterBusinessAccount;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Commands.CreateNewAccountForKeeper;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetListKeeperByManagerId;
using Parking.FindingSlotManagement.Application.Features.Manager.KeeperAccount.KeeperAccountManagement.Queries.GetKeeperById;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByManagerId;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetParkingById;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetListParkingDesByRating;
using Parking.FindingSlotManagement.Application.Features.Customer.Parking.Queries.GetParkingDetails;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetBookingDetails;
using Parking.FindingSlotManagement.Application.Models.Floor;
using Parking.FindingSlotManagement.Application.Models.User;
using Parking.FindingSlotManagement.Application.Models.Traffic;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetListBookingByManagerId;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetBookingById;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloorByParkingId;
using Parking.FindingSlotManagement.Application.Features.Manager.ParkingSlots.Queries.GetListParkingSlotByFloorId;
using Parking.FindingSlotManagement.Application.Features.Manager.Parkings.ParkingManagement.Queries.GetListParkingByParkingPriceId;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Queries.GetListBookingFollowCalendar;
using Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingSlot.Queries.GetAvailableSlotByFloorId;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetCustomerProfileById;
using Parking.FindingSlotManagement.Application.Models.Transaction;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Application.Models.VehicleInfor;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetAllParkingRequest;
using Parking.FindingSlotManagement.Application.Features.Customer.ParkingNearest.Queries.GetListParkingNearestWithDistance;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Commands.CreateNewFee;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetListFee;
using Parking.FindingSlotManagement.Application.Features.Admin.Fee.Queries.GetFeeById;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetListCustomer;
using Parking.FindingSlotManagement.Application.Features.Manager.BusinessProfile.BusinessProfileManagement.Queries.GetInforOfBusinessByManagerId;
using Parking.FindingSlotManagement.Application.Features.Admin.ParkingManagement.Queries.GetAllParkingForAdmin;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Commands.CreateNewApproveParking;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListApproveParkingByParkingId;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.GetAllCustomer.Queries.GetCustomerById;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingById;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetListParkingWaitingToAccept;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetListParkingNewWNoApprove;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.SearchRequestBooking;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.CreateBookingForPasserby;
using Parking.FindingSlotManagement.Application.Features.Customer.Wallet.Queries.GetWalletByUserId;
using Parking.FindingSlotManagement.Application.Features.Customer.Transaction.Queries.GetAllTransactionByUserId;
using Parking.FindingSlotManagement.Application.Features.Staff.ApproveParking.Queries.GetApproveParkingWithIntial;
using Parking.FindingSlotManagement.Application.Features.Admin.Bill.BillManagement.Queries.GetAllBills;
using Parking.FindingSlotManagement.Application.Features.Keeper.Queries.GetAllConflictRequestByKeeperId;
using Parking.FindingSlotManagement.Application.Features.Admin.Booking.BookingManagement.Queries.GetAllBookingForAdmin;
using Parking.FindingSlotManagement.Application.Features.Manager.Booking.Queries.GetAllBookingByParkingId;
using Parking.FindingSlotManagement.Application.Features.Admin.VnPay.VnPayManagement.Queries.GetVnPayById;
using Parking.FindingSlotManagement.Application.Features.Keeper.Commands.DisableParkingSlotByDate.Model;
using Parking.FindingSlotManagement.Application.Features.Customer.Account.AccountManagement.Queries.GetBanCountByUserId;
using Parking.FindingSlotManagement.Application.Features.Admin.Wallet.Queries.GetWalletForAdmin;
using Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab;

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
            CreateMap<User, GetListStaffAccountResponse>().ReverseMap();
            CreateMap<User, GetStaffAccountByIdResponse>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.Name))
                .ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<User, CreateNewAccountForKeeperCommand>().ReverseMap();
            CreateMap<User, GetKeeperByIdResponse>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.Name))
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.Parking.Name))
                .ReverseMap();
            CreateMap<User, UserBookingDto>().ReverseMap();
            CreateMap<User, GetListKeeperByManagerIdResponse>()
                .ForMember<string>(dto => dto.ParkingName, act => act.MapFrom<string>(obj => obj.Parking.Name))
                .ReverseMap();
            CreateMap<User, GetCustomerProfileByIdResponse>().ReverseMap();
            CreateMap<User, GetListCustomerResponse>().ReverseMap();
            CreateMap<User, GetInforOfBusinessByManagerIdResponse>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.Name))
                .ForMember(dto => dto.BusinessProfileId, act => act.MapFrom(obj => obj.BusinessProfile.BusinessProfileId))
                .ForMember(dto => dto.BusinessProfileName, act => act.MapFrom(obj => obj.BusinessProfile.Name))
                .ForMember(dto => dto.Address, act => act.MapFrom(obj => obj.BusinessProfile.Address))
                .ForMember(dto => dto.FrontIdentification, act => act.MapFrom(obj => obj.BusinessProfile.FrontIdentification))
                .ForMember(dto => dto.BackIdentification, act => act.MapFrom(obj => obj.BusinessProfile.BackIdentification))
                .ForMember(dto => dto.BusinessLicense, act => act.MapFrom(obj => obj.BusinessProfile.BusinessLicense))
                .ForMember(dto => dto.Type, act => act.MapFrom(obj => obj.BusinessProfile.Type))
                .ReverseMap();
            CreateMap<User, GetCustomerByIdResponse>()
                .ForMember(dto => dto.RoleName, act => act.MapFrom(obj => obj.Role.Name))
                .ReverseMap();
            CreateMap<User, UserForGetAllBookingByParkingIdResponse>().ReverseMap();
            CreateMap<User, UserForGetAllBookingForAdminResponse>().ReverseMap();
            CreateMap<User, GetBanCountByUserIdResponse>().ReverseMap();
            #endregion

            #region Traffic Mapping
            CreateMap<Traffic, CreateNewTrafficCommand>().ReverseMap();
            CreateMap<Traffic, GetListTrafficResponse>().ReverseMap();
            CreateMap<Traffic, GetTrafficResponse>().ReverseMap();
            CreateMap<Traffic, TrafficDto>().ReverseMap();
            #endregion

            #region BusinessProfile Mapping
            CreateMap<BusinessProfile, GetBusinessProfileByIdResponse>().ReverseMap();
            CreateMap<BusinessProfile, GetListBusinessProfileResponse>()
                .ForMember(dto => dto.UserName, act => act.MapFrom(obj => obj.User.Name))
                .ReverseMap();
            CreateMap<BusinessProfile, CreateNewBusinessProfileCommand>().ReverseMap();
            CreateMap<BusinessProfile, GetBusinessProfileResponse>().ReverseMap();
            CreateMap<BusinessProfile, BusinessProfileEntity>().ReverseMap();
            #endregion

            #region VnPay Mapping
            CreateMap<VnPay, CreateNewVnPayCommand>().ReverseMap();
            CreateMap<VnPay, GetVnPayByUserIdResponse>()
                .ForMember(dto => dto.UserName, act => act.MapFrom(obj => obj.User.Name))
                .ReverseMap();
            CreateMap<VnPay, GetVnPayByIdResponse>().ReverseMap();
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
            CreateMap<Domain.Entities.Parking, GetListParkingNearestYouQueryResponse>()
                .ForMember(dto => dto.Avatar, act => act.MapFrom(obj => obj.ParkingSpotImages.FirstOrDefault().ImgPath))
                .ReverseMap();
            CreateMap<Domain.Entities.Parking, GetListParkingNearestWithDistanceResponse>()
                .ForMember(dto => dto.Avatar, act => act.MapFrom(obj => obj.ParkingSpotImages.FirstOrDefault().ImgPath))
                .ReverseMap();
            CreateMap<Domain.Entities.Parking, GetListParkingByManagerIdResponse>().ReverseMap();
            CreateMap<Domain.Entities.Parking, ParkingEntity>().ReverseMap();
            CreateMap<Domain.Entities.Parking, ParkingShowInCusDto>()
                .ForMember(dto => dto.Avatar, act => act.MapFrom(obj => obj.ParkingSpotImages.FirstOrDefault().ImgPath))
                .ReverseMap();
            CreateMap<Domain.Entities.Parking, ParkingDto>()
                .ForMember(des => des.ParkingHasPrices, src => src.MapFrom(obj => obj.ParkingHasPrices))
                .ReverseMap();
            CreateMap<Domain.Entities.Parking, GetAllParkingRequestResponse>()
                .ForMember(dto => dto.BusinessProfileName, act => act.MapFrom(obj => obj.BusinessProfile.Name))
                .ForMember(dto => dto.ApproveParkingStatus, act => act.MapFrom(obj => obj.ApproveParkings.FirstOrDefault().Status))
                .ReverseMap();
            CreateMap<Domain.Entities.Parking, ParkingWithBookingDetailDto>().ReverseMap();
            CreateMap<Domain.Entities.Parking, GetAllParkingForAdminResponse>().ReverseMap();
            CreateMap<Domain.Entities.Parking, GetListParkingNewWNoApproveResponse>().ReverseMap();
            CreateMap < Domain.Entities.Parking, ParkingSearchResult>().ReverseMap();
            CreateMap<Domain.Entities.Parking, ParkingDtoForAdmin>().ReverseMap();
            CreateMap<Domain.Entities.Parking, DisableParkingSlotParking>().ReverseMap();
            
            #endregion

            #region StaffParking Mapping
            #endregion

            #region Floor Mapping
            CreateMap<Floor, CreateNewFloorCommand>().ReverseMap();
            CreateMap<Floor, GetListFloorResponse>().ReverseMap();
            CreateMap<Floor, FloorDto>().ReverseMap();
            CreateMap<Floor, GetListFloorByParkingIdResponse>().ReverseMap();
            CreateMap<Floor, FloorWithBookingDetailDto>().ReverseMap();
            CreateMap<Floor, FloorDtoForAdmin>().ReverseMap();
            CreateMap<Floor, DisableParkingSlotFloor>().ReverseMap();
            #endregion

            #region TimeLine Mapping
            CreateMap<TimeLine, CreateNewPackagePriceCommand>().ReverseMap();
            //CreateMap<TimeLine, GetPackagePriceByIdResponse>()
            //    .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name))
            //    .ReverseMap();
            CreateMap<TimeLine, TimeLineDto>().ReverseMap();

            #endregion

            #region ParkingSpotImage Mapping
            CreateMap<ParkingSpotImage, CreateNewParkingSpotImageCommand>().ReverseMap();
            CreateMap<ParkingSpotImage, GetListImageByParkingIdResponse>().ReverseMap();
            CreateMap<ParkingSpotImage, ParkingSpotImageDto>().ReverseMap();
            #endregion

            #region ParkingPrice Mapping
            CreateMap<ParkingPrice, CreateParkingPriceCommandDTO>()
                .ReverseMap();
            CreateMap<ParkingPrice, DisableOrEnableParkingPriceCommand>().ReverseMap();
            CreateMap<ParkingPrice, GetAllParkingPriceQueryResponse>().ReverseMap();
            CreateMap<ParkingPrice, ParkingPriceDto>().ReverseMap();
            CreateMap<ParkingPrice, ListParkingPrices>().ReverseMap();
            CreateMap<ParkingPrice, ParkingPriceRes>().ReverseMap();
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
            CreateMap<VehicleInfor, BookingVehicleInforDTO>().ReverseMap();
            CreateMap<VehicleInfor, VehicleInforDtoos>().ReverseMap();
            CreateMap<VehicleInfor, VehicleInforSearchResult>()
                .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name))
                .ReverseMap();
            CreateMap<VehicleInfor, VehicleInformationForPasserby>().ReverseMap();
            CreateMap<VehicleInfor, VehicleForGetAllBookingByParkingIdResponse>().ReverseMap();
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
            CreateMap<ParkingHasPrice, ParkingHasPriceDto>()
                .ReverseMap();
            CreateMap<ParkingHasPrice, GetListParkingByParkingPriceIdResponse>()
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.Parking.Name))
                .ReverseMap();
            #endregion

            #region Parkingslots Mapping
            CreateMap<ParkingSlot, CreateParkingSlotsCommand>().ReverseMap();
            CreateMap<ParkingSlot, GetAvailableSlotsResponse>().ReverseMap();
            CreateMap<ParkingSlot, GetListParkingSlotByFloorIdResponse>().ReverseMap();
            CreateMap<ParkingSlot, ParkingSlotDto>().ReverseMap();
            CreateMap<ParkingSlot, ParkingSlotWithBookingDetailDto>().ReverseMap();
            CreateMap<ParkingSlot, ParkingSlotSearchResult>()
                .ForMember(dto => dto.FloorName, act => act.MapFrom(obj => obj.Floor.FloorName))
                .ReverseMap();
            CreateMap<ParkingSlot, SlotDtoForAdmin>().ReverseMap();
            
            #endregion

            #region Timeline Mapping
            CreateMap<TimeLine, CreateNewTimelineCommandMapper>().ReverseMap();
            CreateMap<TimeLine, TimeLineRes>().ReverseMap();
            #endregion

            #region Booking Mapping
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Booking, BookingDetailsDto>().ReverseMap();
            CreateMap<Booking, GetListBookingByManagerIdResponse>()
                .ForMember(dto => dto.CustomerName, act => act.MapFrom(obj => obj.User.Name))
                .ForMember(dto => dto.Phone, act => act.MapFrom(obj => obj.User.Phone))
                .ForMember(dto => dto.CustomerAvatar, act => act.MapFrom(obj => obj.User.Avatar))
                .ForMember(dto => dto.Position, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Name))
                .ForMember(dto => dto.LicensePlate, act => act.MapFrom(obj => obj.VehicleInfor.LicensePlate))
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Floor.Parking.Name))
                .ReverseMap();
            CreateMap<Booking, GetBookingByIdResponse>()
                .ForMember(dto => dto.CustomerName, act => act.MapFrom(obj => obj.User.Name))
                .ForMember(dto => dto.CustomerPhone, act => act.MapFrom(obj => obj.User.Phone))
                .ForMember(dto => dto.CustomerAvatar, act => act.MapFrom(obj => obj.User.Avatar))
                .ForMember(dto => dto.ParkingSlotName, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Name))
                .ForMember(dto => dto.FloorName, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Floor.FloorName))
                .ForMember(dto => dto.ParkingId, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Floor.Parking.ParkingId))
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Floor.Parking.Name))

                .ForMember(dto => dto.LicensePlate, act => act.MapFrom(obj => obj.VehicleInfor.LicensePlate))
                .ForMember(dto => dto.VehicleName, act => act.MapFrom(obj => obj.VehicleInfor.VehicleName))
                .ForMember(dto => dto.Color, act => act.MapFrom(obj => obj.VehicleInfor.Color))
/*                .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.VehicleInfor.Traffic.Name))*/
                .ReverseMap();
            CreateMap<Booking, GetListBookingFollowCalendarResponse > ()
                .ForMember(dto => dto.ParkingId, act => act.MapFrom(obj => obj.BookingDetails.First().TimeSlot.Parkingslot.Floor.ParkingId))
                .ReverseMap();
            CreateMap<Booking, BookingCheckoutDto>().ReverseMap();
            CreateMap<Booking, BookingSearchResult>().ReverseMap();
            CreateMap<Booking, BookingDtoForAdmin>().ReverseMap();
            CreateMap<Booking, BookingForGetAllBookingByParkingIdResponse>().ReverseMap();
            #endregion

            #region Transaction Mapping
            CreateMap<Transaction, CreateNewTransactionCommand>().ReverseMap();
            CreateMap<Transaction, BookingTransactionDto>().ReverseMap();
            CreateMap<Transaction, TransactionWithBookingDetailDto>().ReverseMap();
            CreateMap<Transaction, GetAllTransactionByUserIdResponse>().ReverseMap();
            #endregion

            #region Fee Mapping
            CreateMap<Fee, CreateNewFeeCommand>().ReverseMap();
            CreateMap<Fee, GetListFeeResponse>().ReverseMap();
            CreateMap<Fee, GetFeeByIdResponse>().ReverseMap();
            #endregion

            #region ApproveParking
            CreateMap<ApproveParking, CreateNewApproveParkingCommand>().ReverseMap();
            CreateMap<ApproveParking, GetListApproveParkingByParkingIdRes>()
                .ForMember(dto => dto.StaffName, act => act.MapFrom(obj => obj.User.Name))
                .ReverseMap();
            CreateMap<ApproveParking, GetListParkingWaitingToAcceptResponse>()
                .ForMember(dto => dto.StaffName, act => act.MapFrom(obj => obj.User.Name))
                .ForMember(dto => dto.ParkingName, act => act.MapFrom(obj => obj.Parking.Name))
                .ReverseMap();
            CreateMap<ApproveParking, GetApproveParkingWithIntialResponse>().ReverseMap();
            #endregion

            #region FieldWorkParkingImg
            CreateMap<FieldWorkParkingImg, ImagesOfRequestApprove>();
            #endregion

            #region Wallet Mapping
            CreateMap<Wallet, GetWalletByUserIdResponse>().ReverseMap();
            CreateMap<Wallet, GetWalletForAdminResponse>().ReverseMap();
            #endregion

            #region Bill Mapping
            CreateMap<Bill, GetAllBillsResponse>()
                .ForMember(x => x.BusinessName, act => act.MapFrom(obj => obj.businessProfile.Name))
                .ReverseMap();
            #endregion

            #region ConflictRequest Mapping
            CreateMap<ConflictRequest, GetAllConflictRequestByKeeperIdResponse>().ReverseMap();
            #endregion
        }
    }
}
