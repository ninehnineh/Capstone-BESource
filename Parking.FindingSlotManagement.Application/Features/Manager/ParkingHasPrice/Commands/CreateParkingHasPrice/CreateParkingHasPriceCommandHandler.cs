//using AutoMapper;
//using MediatR;
//using Parking.FindingSlotManagement.Application.Contracts.Persistence;
//using Parking.FindingSlotManagement.Application.Mapping;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Parking.FindingSlotManagement.Application.Features.Manager.ParkingHasPrice.Commands.CreateParkingHasPrice
//{
//    public class CreateParkingHasPriceCommandHandler : IRequestHandler<CreateParkingHasPriceCommand, ServiceResponse<int>>
//    {
//        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;
//        private readonly IPackagePriceRepository _packagePriceRepository;
//        MapperConfiguration config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile(new MappingProfile());
//        });

//        public CreateParkingHasPriceCommandHandler(IParkingHasPriceRepository parkingHasPriceRepository, 
//            IPackagePriceRepository packagePriceRepository)
//        {
//            _parkingHasPriceRepository = parkingHasPriceRepository;
//            _packagePriceRepository = packagePriceRepository;
//        }

//        public async Task<ServiceResponse<int>> Handle(CreateParkingHasPriceCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var packagePrice = await _packagePriceRepository.GetById(request.ParkingPriceId!);
//                if (packagePrice.IsActive == true)
//                {
//                    return new ServiceResponse<int>
//                    {
//                        Message = "Gói đang được áp dụng, vui lòng chọn gói khác",
//                        StatusCode = 400,
//                        Success = false,
//                    };
//                }

//                List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>> includes = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
//                {
//                    x => x.ParkingPrice!,
//                };

//                var listPackingHasPrice = await _parkingHasPriceRepository
//                    .GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, includes);
//                if (listPackingHasPrice.Count() > 0)
//                {
//                    //qua ngay hom sau
//                    if(packagePrice.EndTime.Value.Date > packagePrice.StartTime.Value.Date)
//                    {
//                        foreach (var item in listPackingHasPrice)
//                        {
//                            var goicuEnd = item.ParkingPrice!.EndTime;
//                            var goicuStart = item.ParkingPrice!.StartTime;
//                            var gói_đang_định_dùng_start = packagePrice.StartTime;
//                            var gói_đang_định_dùng_end = packagePrice.EndTime;

///*                            if (gói_đang_định_dùng_end.Value.Date > gói_đang_định_dùng_start.Value.Date)
//                            {*/
//                                if (gói_đang_định_dùng_start.Value.TimeOfDay < goicuEnd.Value.TimeOfDay)
//                                {
//                                    return new ServiceResponse<int>
//                                    {
//                                        Message = "Gói không hợp lệ",
//                                        StatusCode = 400,
//                                        Success = false,
//                                    };
//                                }
///*                            }*/
///*                            else
//                            {*/
                                
//                            /*}*/
//                        }
//                    }
//                    else // trong ngay
//                    {
//                        foreach (var item in listPackingHasPrice)
//                        {
//                            var goicuEnd = item.ParkingPrice!.EndTime;
//                            var goicuStart = item.ParkingPrice!.StartTime;
//                            var gói_đang_định_dùng_start = packagePrice.StartTime;
//                            var gói_đang_định_dùng_end = packagePrice.EndTime;
//                            if (gói_đang_định_dùng_start.Value.TimeOfDay < goicuEnd.Value.TimeOfDay)
//                            {
//                                return new ServiceResponse<int>
//                                {
//                                    Message = "Gói không hợp lệ",
//                                    StatusCode = 400,
//                                    Success = false,
//                                };
//                            }
//                        }
                            
//                    }
                    
//                }

//                var _mapper = config.CreateMapper();
//                var entity = _mapper.Map<Domain.Entities.ParkingHasPrice>(request);
//                await _parkingHasPriceRepository.Insert(entity);
//                packagePrice.IsActive = true;
//                await _packagePriceRepository.Save();
//                return new ServiceResponse<int>
//                {
//                    Data = entity.ParkingHasPriceId,
//                    Message = "Thành công",
//                    StatusCode = 201,
//                    Success = true,
//                };
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }
//    }
//}
