using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parking.FindingSlotManagement.Application.Features.Customer.Booking.Commons;
using Parking.FindingSlotManagement.Domain.Entities;
using Xunit;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Booking
{
    public class CaculatePriceBookingTests
    {
        // Qua ngày
        [Theory]
        [MemberData(nameof(CalculatorTestData.TestCases1), MemberType = typeof(CalculatorTestData))]
        public void Test1(string x, string y, decimal price)
        {
            //List<Package> packages = new()
            //{
            //    new Package { PackageId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("10:00:00"), Price = 20000, ExtraFee = 5000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 2, StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("15:00:00"), Price = 20000, ExtraFee = 6000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 3, StartTime = TimeSpan.Parse("15:00:00"), EndTime = TimeSpan.Parse("20:00:00"), Price = 20000, ExtraFee = 7000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 4, StartTime = TimeSpan.Parse("20:00:00"), EndTime = TimeSpan.Parse("03:00:00"), Price = 20000, ExtraFee = 8000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 5, StartTime = TimeSpan.Parse("03:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 20000, ExtraFee = 9000, StartingTime = 1, ExtraTimeStep = 2 },
            //};

            List<TimeLine> packages = new()
            {
                new TimeLine { StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00"), Price = 25000, ExtraFee = 10000},
                new TimeLine { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 25000, ExtraFee = 15000},
            };

            var parkingPrice = new ParkingPrice()
            {
                StartingTime = 2,
                ExtraTimeStep = 1,
            };

            // DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            // DateTime endTime = DateTime.Parse($"2023-05-25T{y}:00:00");

            DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            DateTime endTime = DateTime.Parse($"2023-05-26T{y}:00:00");

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }

        // Trong ngày
        [Theory]
        [MemberData(nameof(CalculatorTestData.TestCases2), MemberType = typeof(CalculatorTestData))]
        public void Test2(string x, string y, decimal price)
        {
            //List<Package> packages = new()
            //{
            //    new Package { PackageId = 1, StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("10:00:00"), Price = 20000, ExtraFee = 5000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 2, StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("15:00:00"), Price = 20000, ExtraFee = 6000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 3, StartTime = TimeSpan.Parse("15:00:00"), EndTime = TimeSpan.Parse("20:00:00"), Price = 20000, ExtraFee = 7000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 4, StartTime = TimeSpan.Parse("20:00:00"), EndTime = TimeSpan.Parse("03:00:00"), Price = 20000, ExtraFee = 8000, StartingTime = 1, ExtraTimeStep = 2 },
            //    new Package { PackageId = 5, StartTime = TimeSpan.Parse("03:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 20000, ExtraFee = 9000, StartingTime = 1, ExtraTimeStep = 2 },
            //};

            List<TimeLine> packages = new()
            {
                new TimeLine { StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00"), Price = 25000, ExtraFee = 10000},
                new TimeLine { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 25000, ExtraFee = 15000},
            };

            var parkingPrice = new ParkingPrice()
            {
                StartingTime = 2,
                ExtraTimeStep = 1,
            };

            DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            DateTime endTime = DateTime.Parse($"2023-05-25T{y}:00:00");

            // DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            // DateTime endTime = DateTime.Parse($"2023-05-26T{y}:00:00");

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }

        public class CalculatorTestData
        {
            public static TheoryData<string, string, decimal> TestCases1()
            {
                var data = new TheoryData<string, string, decimal>();

                var r = "05";
                data.Add(r, "00", 225000);
                data.Add(r, "01", 240000);
                data.Add(r, "02", 255000);
                data.Add(r, "03", 270000);
                data.Add(r, "04", 285000);
                data.Add(r, "05", 300000);

                var w = "15";
                data.Add(w, "00", 125000);
                data.Add(w, "01", 140000);
                data.Add(w, "02", 155000);
                data.Add(w, "03", 170000);
                data.Add(w, "04", 185000);
                data.Add(w, "05", 200000);
                data.Add(w, "06", 215000);
                data.Add(w, "07", 225000);
                data.Add(w, "08", 235000);
                data.Add(w, "09", 245000);
                data.Add(w, "10", 255000);
                data.Add(w, "11", 265000);
                data.Add(w, "12", 275000);
                data.Add(w, "13", 285000);
                data.Add(w, "14", 295000);
                data.Add(w, "15", 305000);

                var q = "16";
                data.Add(q, "00", 115000);
                data.Add(q, "01", 130000);
                data.Add(q, "02", 145000);
                data.Add(q, "03", 160000);
                data.Add(q, "04", 175000);
                data.Add(q, "05", 190000);
                data.Add(q, "06", 205000);
                data.Add(q, "07", 215000);
                data.Add(q, "08", 225000);
                data.Add(q, "09", 235000);
                data.Add(q, "10", 245000);
                data.Add(q, "11", 255000);
                data.Add(q, "12", 265000);
                data.Add(q, "13", 275000);
                data.Add(q, "14", 285000);
                data.Add(q, "15", 295000);
                data.Add(q, "16", 305000);

                var x = "17";
                data.Add(x, "00", 100000);
                data.Add(x, "01", 115000);
                data.Add(x, "02", 130000);
                data.Add(x, "03", 145000);
                data.Add(x, "04", 160000);
                data.Add(x, "05", 175000);
                data.Add(x, "06", 190000);
                data.Add(x, "07", 200000);
                data.Add(x, "08", 210000);
                data.Add(x, "09", 220000);
                data.Add(x, "10", 230000);
                data.Add(x, "11", 240000);
                data.Add(x, "12", 250000);
                data.Add(x, "13", 260000);
                data.Add(x, "14", 270000);
                data.Add(x, "15", 280000);
                data.Add(x, "16", 290000);
                data.Add(x, "17", 300000);

                var y = "18";

                data.Add(y, "00", 85000);
                data.Add(y, "01", 100000);
                data.Add(y, "02", 115000);
                data.Add(y, "03", 130000);
                data.Add(y, "04", 145000);
                data.Add(y, "05", 160000);
                data.Add(y, "06", 175000);
                data.Add(y, "07", 185000);
                data.Add(y, "08", 195000);
                data.Add(y, "09", 205000);
                data.Add(y, "10", 215000);
                data.Add(y, "11", 225000);
                data.Add(y, "12", 235000);
                data.Add(y, "13", 245000);
                data.Add(y, "14", 255000);
                data.Add(y, "15", 265000);
                data.Add(y, "16", 275000);
                data.Add(y, "17", 285000);
                data.Add(y, "18", 295000);

                var z = "19";
                data.Add(z, "00", 70000);
                data.Add(z, "01", 85000);
                data.Add(z, "02", 100000);
                data.Add(z, "03", 115000);
                data.Add(z, "04", 130000);
                data.Add(z, "05", 145000);
                data.Add(z, "06", 160000);
                data.Add(z, "07", 170000);
                data.Add(z, "08", 180000);
                data.Add(z, "09", 190000);
                data.Add(z, "10", 200000);
                data.Add(z, "11", 210000);
                data.Add(z, "12", 220000);
                data.Add(z, "13", 230000);
                data.Add(z, "14", 240000);
                data.Add(z, "15", 250000);
                data.Add(z, "16", 260000);
                data.Add(z, "17", 270000);
                data.Add(z, "18", 280000);
                data.Add(z, "19", 295000);

                // Add more test cases here

                return data;
            }

            public static TheoryData<string, string, decimal> TestCases2()
            {
                var data = new TheoryData<string, string, decimal>();

                // Add more test cases here


                // trong ngay
                var e = "15";
                data.Add(e, "16", 25000);
                data.Add(e, "17", 25000);
                data.Add(e, "18", 35000);
                data.Add(e, "19", 50000);
                data.Add(e, "20", 65000);
                data.Add(e, "21", 80000);
                data.Add(e, "22", 95000);
                data.Add(e, "23", 110000);

                var t = "03";
                data.Add(t, "07", 50000);
                data.Add(t, "08", 60000);
                data.Add(t, "09", 70000);
                data.Add(t, "10", 80000);
                data.Add(t, "11", 90000);
                data.Add(t, "12", 100000);
                data.Add(t, "13", 110000);
                data.Add(t, "14", 120000);
                data.Add(t, "15", 130000);
                data.Add(t, "16", 140000);
                data.Add(t, "17", 150000);
                data.Add(t, "18", 160000);
                data.Add(t, "19", 175000);
                data.Add(t, "20", 190000);
                data.Add(t, "21", 205000);
                data.Add(t, "22", 220000);
                data.Add(t, "23", 235000);

                var a = "16";
                data.Add(a, "17", 25000);
                data.Add(a, "18", 25000);
                data.Add(a, "19", 40000);
                data.Add(a, "20", 55000);
                data.Add(a, "21", 70000);
                data.Add(a, "22", 85000);
                data.Add(a, "23", 100000);

                return data;
            }
        }
    }
}