using AutoMapper;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Commands.UpdateCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.CensorshipManagerAccount.Queries.GetCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.NonCensorshipManagerAccount.Queries.GetNonCensorshipManagerAccountList;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.RequestCensorshipManagerAccount.Queries;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.CreateNewTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Commands.UpdateTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetListTraffic;
using Parking.FindingSlotManagement.Application.Features.Admin.Traffics.TrafficManagement.Queries.GetTraffic;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Commands.CreateNewFavoriteAddress;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressById;
using Parking.FindingSlotManagement.Application.Features.Customer.FavoriteAddress.FavoriteAddressManagement.Queries.GetFavoriteAddressByUserId;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Commands.CreateNewFloor;
using Parking.FindingSlotManagement.Application.Features.Manager.Floors.FloorManagement.Queries.GetListFloor;
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
            //Mapping For Admin
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
            //Mapping For Manager
            //For Parking
            CreateMap<Domain.Entities.Parking, CreateNewParkingCommand>().ReverseMap();
            //For StaffParking
            CreateMap<StaffParking, CreateNewStaffParkingCommand>().ReverseMap();
            //For Floor
            CreateMap<Floor, CreateNewFloorCommand>().ReverseMap();
            CreateMap<Floor, GetListFloorResponse>().ReverseMap();
            //Mapping For Staff
            //Mapping For Customer
            //For FavoriteAddress
            CreateMap<FavoriteAddress, CreateNewFavoriteAddressCommand>().ReverseMap();
            CreateMap<FavoriteAddress, GetFavoriteAddressByIdResponse>().ReverseMap(); 
            CreateMap<FavoriteAddress, GetFavoriteAddressByUserIdResponse>().ReverseMap();
        }
    }
}
