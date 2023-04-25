﻿using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.Accounts.Commands.UpdateCensorshipManagerAccount
{
    public class UpdateCensorshipManagerAccountCommandHandler : IRequestHandler<UpdateCensorshipManagerAccountCommand, ServiceResponse<string>>
    {
        private readonly IAccountRepository _accountRepository;
        MapperConfiguration cofig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public UpdateCensorshipManagerAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateCensorshipManagerAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkExist = await _accountRepository.GetById(request.UserId);
                if(checkExist == null)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Not found user.",
                        StatusCode = 200,
                        Success = true,
                        Count = 0
                    };
                }
                if(!string.IsNullOrEmpty(request.Name))
                {
                    checkExist.Name = request.Name;
                }
                if(!string.IsNullOrEmpty(request.Email))
                {
                    var checkEmailExist = await _accountRepository.GetItemWithCondition(x => x.Email.Equals(request.Email));
                    if (checkEmailExist != null)
                    {
                        return new ServiceResponse<string>
                        {
                            StatusCode = 200,
                            Success = true,
                            Count = 0,
                            Message = "The email has exist. Please enter new email!!!"
                        };
                    }
                    checkExist.Email = request.Email;
                }
                if (!string.IsNullOrEmpty(request.Password))
                {
                    checkExist.Password = request.Password;
                }
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    checkExist.Phone = request.Phone;
                }
                if (!string.IsNullOrEmpty(request.Avatar))
                {
                    checkExist.Avatar = request.Avatar;
                }
                if (!string.IsNullOrEmpty(request.DateOfBirth.ToString()))
                {
                    checkExist.DateOfBirth = request.DateOfBirth;
                }
                if (!string.IsNullOrEmpty(request.Gender))
                {
                    checkExist.Gender = request.Gender;
                }
                await _accountRepository.Update(checkExist);
                return new ServiceResponse<string>
                {
                    Message = "Successfully",
                    StatusCode = 204,
                    Success = true,
                    Count = 0
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
