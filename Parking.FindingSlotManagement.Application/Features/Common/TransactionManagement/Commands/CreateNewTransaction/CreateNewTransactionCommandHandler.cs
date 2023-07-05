using AutoMapper;
using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Mapping;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Common.TransactionManagement.Commands.CreateNewTransaction
{
    public class CreateNewTransactionCommandHandler : IRequestHandler<CreateNewTransactionCommand, ServiceResponse<int>>
    {
        private readonly ITransactionRepository _transactionRepository;
        MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public CreateNewTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<ServiceResponse<int>> Handle(CreateNewTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _mapper = config.CreateMapper();
                var entity = _mapper.Map<Transaction>(request);
                entity.PaymentMethod = Domain.Enum.PaymentMethod.thanh_toan_online.ToString();
                await _transactionRepository.Insert(entity);
                return new ServiceResponse<int>
                {
                    Data = entity.TransactionId,
                    Message = "Nạp tiền thành công!!!",
                    StatusCode = 201,
                    Success = true
                };

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
