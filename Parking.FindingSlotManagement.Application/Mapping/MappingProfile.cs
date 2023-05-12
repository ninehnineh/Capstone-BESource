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
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Common Mapping
            //***Mapping For Admin
            //ForAccount
            CreateMap<User, CensorshipManagerAccountResponse>().ReverseMap();
            CreateMap<User, CreateNewCensorshipManagerAccountCommand>().ReverseMap();
            CreateMap<User, UpdateCensorshipManagerAccountCommand>().ReverseMap();
            CreateMap<User, RequestResponse>().ReverseMap();
            CreateMap<User, NonCensorshipManagerAccountResponse>().ReverseMap();
            //ForTraffic
            CreateMap<Traffic, CreateNewTrafficCommand>().ReverseMap();
            CreateMap<Traffic, GetListTrafficResponse>().ReverseMap();
            CreateMap<Traffic, GetTrafficResponse>().ReverseMap();
            //For BusinessProfile
            CreateMap<BusinessProfile, GetBusinessProfileByIdResponse>().ReverseMap();
            CreateMap<BusinessProfile, GetListBusinessProfileResponse>()
                .ForMember(dto => dto.UserName, act => act.MapFrom(obj => obj.User.Name))
                .ReverseMap();
            //For VnPay
            CreateMap<VnPay, CreateNewVnPayCommand>().ReverseMap();
            CreateMap<VnPay, GetVnPayByManagerIdResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            //For Paypal
            CreateMap<PayPal, CreateNewPaypalCommand>().ReverseMap();
            CreateMap<PayPal, GetPaypalByManagerIdResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            CreateMap<PayPal, GetListPaypalResponse>()
                .ForMember(dto => dto.ManagerName, act => act.MapFrom(obj => obj.Manager.Name))
                .ReverseMap();
            //***Mapping For Manager
            //For Parking
            CreateMap<Domain.Entities.Parking, CreateNewParkingCommand>().ReverseMap();
            //For StaffParking
            CreateMap<StaffParking, CreateNewStaffParkingCommand>().ReverseMap();
            //For Floor
            CreateMap<Floor, CreateNewFloorCommand>().ReverseMap();
            CreateMap<Floor, GetListFloorResponse>().ReverseMap();
            //For PackagePrice
            CreateMap<PackagePrice, CreateNewPackagePriceCommand>().ReverseMap();
            CreateMap<PackagePrice, GetPackagePriceByIdResponse>()
                .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name))
                .ReverseMap();
            //For Account
            CreateMap<User, CreateNewStaffAccountCommand>().ReverseMap();
            //For ParkingSpotImage
            CreateMap<ParkingSpotImage, CreateNewParkingSpotImageCommand>().ReverseMap();
            CreateMap<ParkingSpotImage, GetListImageByParkingIdResponse>().ReverseMap();
            //***Mapping For Staff
            //***Mapping For Customer
            //For FavoriteAddress
            CreateMap<FavoriteAddress, CreateNewFavoriteAddressCommand>().ReverseMap();
            CreateMap<FavoriteAddress, GetFavoriteAddressByIdResponse>().ReverseMap(); 
            CreateMap<FavoriteAddress, GetFavoriteAddressByUserIdResponse>().ReverseMap();
            //For Vehicle Infor 
            CreateMap<VehicleInfor, VehicleInfoCommand>().ReverseMap();
            CreateMap<VehicleInfor, GetVehicleInforByIdResponse>().ReverseMap();
            CreateMap<VehicleInfor, GetListVehicleInforByUserIdResponse>()
                .ForMember(dto => dto.TrafficName, act => act.MapFrom(obj => obj.Traffic.Name)).ReverseMap();
            //For BusinessProfile 
            CreateMap<BusinessProfile, CreateNewBusinessProfileCommand>().ReverseMap();
            CreateMap<BusinessProfile, GetBusinessProfileResponse>().ReverseMap();

            #region ParkingHasPrice Mapping
            CreateMap<ParkingHasPrice, GetListParkingHasPriceWithPaginationResponse>().ReverseMap();
            CreateMap<ParkingHasPrice, GetParkingHasPriceDetailWithPaginationResponse>().ReverseMap();
            #endregion

            #region Parkingslots Mapping
            CreateMap<ParkingSlot, CreateParkingSlotsCommand>().ReverseMap();
            #endregion
        }
    }
}
