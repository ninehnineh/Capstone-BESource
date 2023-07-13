using MediatR;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Admin.ApproveParking.Queries.GetParkingInformationTab
{
    public class GetParkingInformationTabQueryHandler : IRequestHandler<GetParkingInformationTabQuery, ServiceResponse<GetParkingInformationTabResponse>>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IFieldWorkParkingImgRepository _fieldWorkParkingImgRepository;
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IParkingSlotRepository _parkingSlotRepository;
        private readonly IParkingHasPriceRepository _parkingHasPriceRepository;

        public GetParkingInformationTabQueryHandler(
            IParkingRepository parkingRepository, 
            IFieldWorkParkingImgRepository fieldWorkParkingImgRepository, 
            ITimeSlotRepository timeSlotRepository, 
            IParkingSlotRepository parkingSlotRepository,
            IParkingHasPriceRepository parkingHasPriceRepository)
        {
            _parkingRepository = parkingRepository;
            _fieldWorkParkingImgRepository = fieldWorkParkingImgRepository;
            _timeSlotRepository = timeSlotRepository;
            _parkingSlotRepository = parkingSlotRepository;
            _parkingHasPriceRepository = parkingHasPriceRepository;
        }
        public async Task<ServiceResponse<GetParkingInformationTabResponse>> Handle(GetParkingInformationTabQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Expression<Func<Domain.Entities.Parking, object>>> includes = new List<Expression<Func<Domain.Entities.Parking, object>>>
                {
                    x => x.BusinessProfile,
                    x => x.ApproveParkings,
                    x => x.Floors
                };
                var parkingExist = await _parkingRepository.GetItemWithCondition(x => x.ParkingId == request.ParkingId, includes, true);
                if(parkingExist == null)
                {
                    return new ServiceResponse<GetParkingInformationTabResponse>
                    {
                        Message = "Không tìm thấy bãi giữ xe.",
                        StatusCode = 200,
                        Success = true
                    };
                }
                GetParkingInformationTabResponse entityRes = new()
                {
                    ParkingId = parkingExist.ParkingId,
                    BusinessId = parkingExist.BusinessId,
                    BusinessName = parkingExist.BusinessProfile.Name,
                    ApproveParkingStatus = parkingExist.ApproveParkings.FirstOrDefault(x => x.Status.Equals(Domain.Enum.ApproveParkingStatus.Đã_duyệt.ToString())).Status,
                    Stars = parkingExist.Stars,
                    Description = parkingExist.Description,
                    Address = parkingExist.Address,
                    IsFull = parkingExist.IsFull,
                    CarSpot = parkingExist.CarSpot,
                    TotalFloor = parkingExist.Floors.Count()
                };
                List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>> includes2 = new List<Expression<Func<Domain.Entities.ParkingHasPrice, object>>>
                {
                    x => x.ParkingPrice
                };
                var lstParkingHasPrice = await _parkingHasPriceRepository.GetAllItemWithCondition(x => x.ParkingId == request.ParkingId, includes2, null, true);
                List<string> lstParkingHasPriceOfParking = new();
                foreach (var item in lstParkingHasPrice)
                {
                    lstParkingHasPriceOfParking.Add(item.ParkingPrice.ParkingPriceName);
                }
                entityRes.ParkingPrices = lstParkingHasPriceOfParking;
                var includes3 = new List<Expression<Func<Domain.Entities.TimeSlot, object>>>
                {
                   x => x.Parkingslot,
                   x => x.Parkingslot.Floor
                };
                var timime = DateTime.UtcNow.AddHours(7);
                DateTime currentTime = timime.Date.AddHours(timime.Hour);
                var totalSlotBooked = 0;
                foreach (var itemm in parkingExist.Floors)
                {
                    var currentLstBookedSlot = await _timeSlotRepository.GetAllItemWithCondition(x =>
                                                            x.Parkingslot.FloorId == itemm.FloorId &&
                                                            x.StartTime >= currentTime && x.Status == "Booked", includes3);
                    HashSet<int> listParkingSlotIdExist = new();
                    foreach (var item in currentLstBookedSlot)
                    {
                        if (!listParkingSlotIdExist.Contains((int)item.ParkingSlotId))
                        {
                            listParkingSlotIdExist.Add((int)item.ParkingSlotId);
                        }
                    }
                    var lstParkingSlotHasBooked = await _parkingSlotRepository.GetAllItemWithCondition(x => listParkingSlotIdExist.Contains((int)x.ParkingSlotId) || x.FloorId == itemm.FloorId && x.IsAvailable == false);
                    totalSlotBooked += lstParkingSlotHasBooked.Count();
                }
                entityRes.SlotHasBooked = totalSlotBooked;
                var lstImages = await _fieldWorkParkingImgRepository.GetAllItemWithConditionByNoInclude(x => x.ApproveParkingId == parkingExist.ApproveParkings.FirstOrDefault().ApproveParkingId);
                List<string> imgRes = new();
                foreach (var item in lstImages)
                {
                    imgRes.Add(item.Url);
                }
                entityRes.Images = imgRes;
                return new ServiceResponse<GetParkingInformationTabResponse>
                {
                    Data = entityRes,
                    Message = "Thành công",
                    Success = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
