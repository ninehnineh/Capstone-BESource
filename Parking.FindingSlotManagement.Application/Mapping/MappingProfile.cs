using AutoMapper;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.CreateNewCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.UpdateCensorshipManagerAccount;
using Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Queries.GetCensorshipManagerAccountList;
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
            CreateMap<User, CensorshipManagerAccountResponse>().ReverseMap();
            CreateMap<User, CreateNewCensorshipManagerAccountCommand>().ReverseMap();
            CreateMap<User, UpdateCensorshipManagerAccountCommand>().ReverseMap();
            //Mapping For Manager
            //Mapping For Staff
            //Mapping For Customer
        }
    }
}
