using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commands.CreateBooking;
using Parking.FindingSlotManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons
{
    public class CaculatePriceBooking
    {
        public static decimal CaculateExpectedPrice(DateTime start, DateTime end, ParkingPrice parkingPrice, IEnumerable<TimeLine> timeLines)
        {
            var startTimeBooking = start.TimeOfDay.TotalHours;
            var endTimeBooking = end.TimeOfDay.TotalHours;
            var startTimeDate = start.Date;
            var endTimeDate = end.Date;
            var startingTime = parkingPrice.StartingTime;
            var extraTimeStep = parkingPrice.ExtraTimeStep;
            bool foundStartPoint = false;
            var hitEndPoint = false;
            var totalPrice = 0M;
            var startingPoint = 0;
            var extraFeePoint = 0;
            var isPass = false;
            var startPointIsFoundInFirstTimeLine = false;

            while (hitEndPoint == false)
            {
                foreach (var package in timeLines)
                {
                    var startTimePackage = package.StartTime?.TotalHours;
                    var endTimePackage = package.EndTime?.TotalHours;

                    if (!foundStartPoint &&
                        startTimePackage > endTimePackage &&
                        startPointIsFoundInFirstTimeLine == false)
                    {
                        startPointIsFoundInFirstTimeLine = true;
                        endTimePackage += 24;

                        if (start.Date == end.Date && startTimeBooking < startTimePackage)
                            startTimeBooking += 24;
                        //if (endTimeBooking <= startTimePackage)
                        //{
                        //    endTimeBooking += 24;
                        //}
                    }

                    if (startTimePackage > endTimePackage && foundStartPoint == true)
                    {
                        endTimePackage += 24;
                    }

                    if (startTimeDate == endTimeDate)
                    {
                        // một gói, trong ngày
                        if (endTimeBooking < endTimePackage && endTimeBooking < startTimePackage)
                            endTimeBooking += 24;

                        if (BookingTimeInOnePackage(startTimeBooking, endTimeBooking,
                            startTimePackage, endTimePackage))
                        {
                            if (package.StartTime > package.EndTime)
                            {
                                if (Math.Abs(24 - startTimeBooking) >= (24 - startTimePackage) &&
                                    Math.Abs(24 - endTimeBooking) >= (24 - startTimePackage))
                                {
                                    startTimeBooking += 24;
                                    endTimeBooking += 24;
                                }
                                package.EndTime += TimeSpan.FromHours(24);
                            }

                            var BookedTime = (decimal)(endTimeBooking - startTimeBooking);
                            if (startingTime < BookedTime)
                            {
                                var ExtraFeeHours = BookedTime - startingTime;
                                var so_step = (int)ExtraFeeHours / (int)extraTimeStep!;
                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee);
                            }
                            else
                            {
                                totalPrice = package.Price;
                            }

                            if (endTimeBooking > 24 &&
                                startTimeBooking > 24)
                            {
                                endTimeBooking -= 24;
                                startTimeBooking -= 24;
                            }
                            hitEndPoint = true;
                            break;
                        }

                        // nhiều gói, trong ngày
                        else
                        {

                            if (endTimePackage > 24 && startTimeBooking < 24)
                            {
                                startTimeBooking += 24;
                            }
                            if (startTimeBooking >= startTimePackage &&
                                startTimeBooking < endTimePackage &&
                                foundStartPoint == false)
                            {
                                foundStartPoint = true;
                                var step = 0;
                                var startingPrice = 0M;

                                startingPrice =
                                    (double)(startTimeBooking + startingTime)! == endTimePackage
                                    ? package.Price
                                    : package.Price;

                                var extraPrice = 0M;
                                startingPoint = (int)(startTimeBooking + startingTime)!;
                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;
                                if (startingPoint == endTimePackage)
                                {
                                    isPass = true;
                                }
                                while (extraFeePoint <= endTimePackage)
                                {
                                    step++;
                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
                                    extraPrice = step * (decimal)package.ExtraFee!;
                                };
                                totalPrice += startingPrice + extraPrice;

                                continue;
                            }

                            if (foundStartPoint == true)
                            {
                                var priceOfTimeLineTwo = 0M;
                                // if (startingPoint == startTimePackage ||
                                //     startingPoint == startTimePackage + 24)
                                // {
                                //     totalPrice += (decimal)package.ExtraFee!;
                                // }
                                if (extraFeePoint > 24 && endTimeBooking < 24)
                                    endTimeBooking += 24;

                                if (startPointIsFoundInFirstTimeLine)
                                    endTimePackage += 24;

                                while (extraFeePoint <= endTimePackage &&
                                    extraFeePoint <= endTimeBooking)
                                {
                                    priceOfTimeLineTwo = (decimal)package.ExtraFee!;
                                    totalPrice += priceOfTimeLineTwo;
                                    extraFeePoint += (int)extraTimeStep!;

                                    if (endTimePackage < startTimePackage)
                                        endTimePackage += 24;

                                    if (extraFeePoint > 24 && endTimePackage < 24)
                                        endTimePackage += 24;

                                }
                                if (extraFeePoint <= endTimeBooking)
                                {
                                    continue;
                                }
                                hitEndPoint = true;
                                break;
                            }

                        }
                    }

                    if (startTimeDate < endTimeDate)
                    {

                        // một gói, qua ngày
                        if (BookingTimeInManyPackage(startTimeBooking, endTimeBooking,
                            startTimePackage, endTimePackage))
                        {
                            if (package.StartTime > package.EndTime)
                            {
                                if (startTimeBooking > endTimeBooking)
                                {
                                    endTimeBooking += 24;
                                }
                                package.EndTime += TimeSpan.FromHours(24);
                            }

                            var BookedTime = (decimal)(endTimeBooking - startTimeBooking);
                            if (startingTime < BookedTime)
                            {
                                var ExtraFeeHours = BookedTime - startingTime;
                                var so_step = (int)ExtraFeeHours / (int)extraTimeStep!;
                                totalPrice = package.Price + (decimal)(so_step * package.ExtraFee!);
                            }
                            else
                            {
                                totalPrice = package.Price;
                            }

                            if (endTimeBooking > 24)
                            {
                                endTimeBooking -= 24;
                            }
                            hitEndPoint = true;
                            break;
                        }

                        // nhiều gói, qua ngày
                        else
                        {

                            if (endTimePackage < startTimePackage)
                                package.EndTime += TimeSpan.FromHours(24);

                            if (startTimeBooking < startTimePackage && 
                                startTimeBooking < endTimePackage)
                                startTimeBooking += 24;

                            if (startTimeBooking >= startTimePackage &&
                                startTimeBooking < endTimePackage &&
                                foundStartPoint == false)
                            {
                                foundStartPoint = true;
                                var step = 0;
                                var startingPrice = 0M;

                                startingPrice =
                                    (double)(startTimeBooking + startingTime)! == endTimePackage
                                    ? package.Price
                                    : package.Price;

                                var extraPrice = 0M;
                                var bookedTime = (int)(endTimePackage - startTimeBooking);

                                startingPoint = (int)(startTimeBooking + startingTime!);
                                extraFeePoint = (int)(startingPoint + extraTimeStep)!;

                                while (extraFeePoint <= endTimePackage)
                                {
                                    step++;
                                    extraFeePoint = (int)(extraFeePoint + extraTimeStep)!;
                                    extraPrice = step * (decimal)package.ExtraFee!;
                                };
                                totalPrice = startingPrice + extraPrice;

                                continue;
                            }

                            if (foundStartPoint)
                            {

                                endTimeBooking += 24;

                                if (extraFeePoint > 24 && endTimeBooking < 24)
                                    endTimeBooking += 24;

                                if (startPointIsFoundInFirstTimeLine)
                                    endTimePackage += 24;

                                if (extraFeePoint > endTimePackage)
                                    endTimePackage += 24;

                                if (extraFeePoint > endTimeBooking)
                                    endTimeBooking += 24;

                                while (extraFeePoint <= endTimePackage &&
                                    extraFeePoint <= endTimeBooking)
                                {
                                    totalPrice += (decimal)package.ExtraFee!;
                                    extraFeePoint += (int)extraTimeStep!;
                                    
                                    if (endTimePackage < startTimePackage)
                                        endTimePackage += 24;

                                    if (extraFeePoint > 24 && endTimePackage < 24)
                                        endTimePackage += 24;
                                }
                                if (extraFeePoint <= endTimeBooking)
                                {
                                    endTimeBooking -= 24;
                                    continue;
                                }

                                hitEndPoint = true;
                                break;
                            }
                        }
                    }
                }
            }


            return totalPrice;
        }

        private static bool BookingTimeInManyPackage(double startTimeBooking, double endTimeBooking, double? startTimePackage, double? endTimePackage)
        {
            return startTimeBooking >= startTimePackage &&
                                    startTimeBooking < endTimePackage &&
                                    endTimeBooking > startTimeBooking &&
                                    endTimeBooking >= startTimePackage &&
                                    endTimeBooking <= endTimePackage ||
                                    startTimeBooking >= startTimePackage &&
                                    startTimeBooking < endTimePackage &&
                                    (endTimeBooking + 24) > startTimeBooking &&
                                    (endTimeBooking + 24) >= startTimePackage &&
                                    (endTimeBooking + 24) <= endTimePackage;
        }

        private static bool BookingTimeInOnePackage(double startTimeBooking, double endTimeBooking, double? startTimePackage, double? endTimePackage)
        {
            return startTimeBooking >= startTimePackage &&
                                    startTimeBooking <= endTimePackage &&
                                    endTimeBooking > startTimeBooking &&
                                    endTimeBooking >= startTimePackage &&
                                    endTimeBooking <= endTimePackage ||
                                    (startTimeBooking + 24) >= startTimePackage &&
                                    (startTimeBooking + 24) <= endTimePackage &&
                                    (endTimeBooking + 24) > startTimeBooking &&
                                    (endTimeBooking + 24) >= startTimePackage &&
                                    (endTimeBooking + 24) <= endTimePackage;
        }
    }
}