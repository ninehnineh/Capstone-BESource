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
        [MemberData(nameof(CalculatorTestData.QuaNgayTL1), MemberType = typeof(CalculatorTestData))]
        public void Test1(string x, string y, decimal price)
        {
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
            DateTime endTime = DateTime.Parse($"2023-05-26T{y}:00:00");

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }

        // Trong ngày
        [Theory]
        [MemberData(nameof(CalculatorTestData.TrongNgayTL1), MemberType = typeof(CalculatorTestData))]
        public void Test2(string x, string y, decimal price)
        {

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

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }


        [Theory]
        [MemberData(nameof(CalculatorTestData.TrongNgayTL2), MemberType = typeof(CalculatorTestData))]
        public void Test3(string x, string y, decimal price)
        {
            List<TimeLine> packages = new()
            {
                new TimeLine { StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00"), Price = 20000, ExtraFee = 5000},
                new TimeLine { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 20000, ExtraFee = 6000},
            };

            var parkingPrice = new ParkingPrice()
            {
                StartingTime = 1,
                ExtraTimeStep = 2,
            };

            DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            DateTime endTime = DateTime.Parse($"2023-05-25T{y}:00:00");

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }

        [Theory]
        [MemberData(nameof(CalculatorTestData.QuaNgayTL2), MemberType = typeof(CalculatorTestData))]
        public void Test4(string x, string y, decimal price)
        {
            List<TimeLine> packages = new()
            {
                new TimeLine { StartTime = TimeSpan.Parse("06:00:00"), EndTime = TimeSpan.Parse("18:00:00"), Price = 20000, ExtraFee = 5000},
                new TimeLine { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("06:00:00"), Price = 20000, ExtraFee = 6000},
            };

            var parkingPrice = new ParkingPrice()
            {
                StartingTime = 1,
                ExtraTimeStep = 2,
            };

            DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            DateTime endTime = DateTime.Parse($"2023-05-26T{y}:00:00");

            var result = CaculatePriceBooking.CaculateExpectedPrice(startTime, endTime, parkingPrice, packages);

            Assert.Equal(price, result);
        }

        public class CalculatorTestData
        {
            public static TheoryData<string, string, decimal> QuaNgayTL1()
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

            public static TheoryData<string, string, decimal> TrongNgayTL1()
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

            public static TheoryData<string, string, decimal> TrongNgayTL2()
            {
                var data = new TheoryData<string, string, decimal>();

                // Add more test cases here
                // trong ngay
                var a = "01";
                data.Add(a, "02", 20000);
                data.Add(a, "03", 20000);
                data.Add(a, "04", 26000);
                data.Add(a, "05", 26000);
                data.Add(a, "06", 32000);
                data.Add(a, "07", 32000);
                data.Add(a, "08", 37000);
                data.Add(a, "09", 37000);
                data.Add(a, "10", 42000);
                data.Add(a, "11", 42000);
                data.Add(a, "12", 47000);
                data.Add(a, "13", 47000);
                data.Add(a, "14", 52000);
                data.Add(a, "15", 52000);
                data.Add(a, "16", 57000);
                data.Add(a, "17", 57000);
                data.Add(a, "18", 62000);
                data.Add(a, "19", 62000);
                data.Add(a, "20", 68000);
                data.Add(a, "21", 68000);
                data.Add(a, "22", 74000);
                data.Add(a, "23", 74000);

                var b = "03";
                data.Add(b, "04", 20000);
                data.Add(b, "05", 20000);
                data.Add(b, "06", 26000);
                data.Add(b, "07", 26000);
                data.Add(b, "08", 31000);
                data.Add(b, "09", 31000);
                data.Add(b, "10", 36000);
                data.Add(b, "11", 36000);
                data.Add(b, "12", 41000);
                data.Add(b, "13", 41000);
                data.Add(b, "14", 46000);
                data.Add(b, "15", 46000);
                data.Add(b, "16", 51000);
                data.Add(b, "17", 51000);
                data.Add(b, "18", 56000);
                data.Add(b, "19", 56000);
                data.Add(b, "20", 62000);
                data.Add(b, "21", 62000);
                data.Add(b, "22", 68000);
                data.Add(b, "23", 68000);

                var c = "04";
                data.Add(c, "05", 20000);
                data.Add(c, "06", 20000);
                data.Add(c, "07", 25000);
                data.Add(c, "08", 25000);
                data.Add(c, "09", 30000);
                data.Add(c, "10", 30000);
                data.Add(c, "11", 35000);
                data.Add(c, "12", 35000);
                data.Add(c, "13", 40000);
                data.Add(c, "14", 40000);
                data.Add(c, "15", 45000);
                data.Add(c, "16", 45000);
                data.Add(c, "17", 50000);
                data.Add(c, "18", 50000);
                data.Add(c, "19", 56000);
                data.Add(c, "20", 56000);
                data.Add(c, "21", 62000);
                data.Add(c, "22", 62000);
                data.Add(c, "23", 68000);

                var d = "05";
                data.Add(d, "06", 20000);
                data.Add(d, "07", 20000);
                data.Add(d, "08", 25000);
                data.Add(d, "09", 25000);
                data.Add(d, "10", 30000);
                data.Add(d, "11", 30000);
                data.Add(d, "12", 35000);
                data.Add(d, "13", 35000);
                data.Add(d, "14", 40000);
                data.Add(d, "15", 40000);
                data.Add(d, "16", 45000);
                data.Add(d, "17", 45000);
                data.Add(d, "18", 50000);
                data.Add(d, "19", 50000);
                data.Add(d, "20", 56000);
                data.Add(d, "21", 56000);
                data.Add(d, "22", 62000);
                data.Add(d, "23", 62000);

                var e = "06";
                data.Add(e, "07", 20000);
                data.Add(e, "08", 20000);
                data.Add(e, "09", 25000);
                data.Add(e, "10", 25000);
                data.Add(e, "11", 30000);
                data.Add(e, "12", 30000);
                data.Add(e, "13", 35000);
                data.Add(e, "14", 35000);
                data.Add(e, "15", 40000);
                data.Add(e, "16", 40000);
                data.Add(e, "17", 45000);
                data.Add(e, "18", 45000);
                data.Add(e, "19", 51000);
                data.Add(e, "20", 51000);
                data.Add(e, "21", 57000);
                data.Add(e, "22", 57000);
                data.Add(e, "23", 63000);

                var f = "15";
                data.Add(f, "16", 20000);
                data.Add(f, "17", 20000);
                data.Add(f, "18", 25000);
                data.Add(f, "19", 25000);
                data.Add(f, "20", 31000);
                data.Add(f, "21", 31000);
                data.Add(f, "22", 37000);
                data.Add(f, "23", 37000);

                var g = "16";
                data.Add(g, "17", 20000);
                data.Add(g, "18", 20000);
                data.Add(g, "19", 26000);
                data.Add(g, "20", 26000);
                data.Add(g, "21", 32000);
                data.Add(g, "22", 32000);
                data.Add(g, "23", 38000);

                var h = "17";
                data.Add(h, "18", 20000);
                data.Add(h, "19", 20000);
                data.Add(h, "20", 26000);
                data.Add(h, "21", 26000);
                data.Add(h, "22", 32000);
                data.Add(h, "23", 32000);

                var xx = "18";
                data.Add(xx, "19", 20000);
                data.Add(xx, "20", 20000);
                data.Add(xx, "21", 26000);
                data.Add(xx, "22", 26000);
                data.Add(xx, "23", 32000);

                var k = "19";
                data.Add(k, "20", 20000);
                data.Add(k, "21", 20000);
                data.Add(k, "22", 26000);
                data.Add(k, "23", 26000);

                return data;
            }

            public static TheoryData<string, string, decimal> QuaNgayTL2()
            {
                var data = new TheoryData<string, string, decimal>();

                // Add more test cases here
                // trong ngay
                var a = "01";
                data.Add(a, "00", 80000);
                data.Add(a, "01", 80000);

                var b = "03";
                data.Add(b, "00", 74000);
                data.Add(b, "01", 74000);
                data.Add(b, "02", 80000);
                data.Add(b, "03", 80000);

                var c = "04";
                data.Add(c, "00", 68000);
                data.Add(c, "01", 74000);
                data.Add(c, "02", 74000);
                data.Add(c, "03", 80000);
                data.Add(c, "04", 80000);

                var d = "05";
                data.Add(d, "00", 68000);
                data.Add(d, "01", 68000);
                data.Add(d, "02", 74000);
                data.Add(d, "03", 74000);
                data.Add(d, "04", 80000);
                data.Add(d, "05", 80000);

                var e = "06";
                data.Add(e, "00", 63000);
                data.Add(e, "01", 69000);
                data.Add(e, "02", 69000);
                data.Add(e, "03", 75000);
                data.Add(e, "04", 75000);
                data.Add(e, "05", 81000);
                data.Add(e, "06", 81000);

                var f = "15";
                data.Add(f, "00", 43000);
                data.Add(f, "01", 43000);
                data.Add(f, "02", 49000);
                data.Add(f, "03", 49000);
                data.Add(f, "04", 55000);
                data.Add(f, "05", 55000);
                data.Add(f, "06", 61000);
                data.Add(f, "07", 61000);
                data.Add(f, "08", 66000);
                data.Add(f, "09", 66000);
                data.Add(f, "10", 71000);
                data.Add(f, "11", 71000);
                data.Add(f, "12", 76000);
                data.Add(f, "13", 76000);
                data.Add(f, "14", 81000);
                data.Add(f, "15", 81000);

                var g = "16";
                data.Add(g, "00", 38000);
                data.Add(g, "01", 44000);
                data.Add(g, "02", 44000);
                data.Add(g, "03", 50000);
                data.Add(g, "04", 50000);
                data.Add(g, "05", 56000);
                data.Add(g, "06", 56000);
                data.Add(g, "07", 61000);
                data.Add(g, "08", 61000);
                data.Add(g, "09", 66000);
                data.Add(g, "10", 66000);
                data.Add(g, "11", 71000);
                data.Add(g, "12", 71000);
                data.Add(g, "13", 76000);
                data.Add(g, "14", 76000);
                data.Add(g, "15", 81000);
                data.Add(g, "16", 81000);

                var h = "17";
                data.Add(h, "00", 38000);
                data.Add(h, "01", 38000);
                data.Add(h, "02", 44000);
                data.Add(h, "03", 44000);
                data.Add(h, "04", 50000);
                data.Add(h, "05", 50000);
                data.Add(h, "06", 56000);
                data.Add(h, "07", 56000);
                data.Add(h, "08", 61000);
                data.Add(h, "09", 61000);
                data.Add(h, "10", 66000);
                data.Add(h, "11", 66000);
                data.Add(h, "12", 71000);
                data.Add(h, "13", 71000);
                data.Add(h, "14", 76000);
                data.Add(h, "15", 76000);
                data.Add(h, "16", 81000);
                data.Add(h, "17", 81000);

                var i = "18";
                data.Add(i, "00", 32000);
                data.Add(i, "01", 38000);
                data.Add(i, "02", 38000);
                data.Add(i, "03", 44000);
                data.Add(i, "04", 44000);
                data.Add(i, "05", 50000);
                data.Add(i, "06", 50000);
                data.Add(i, "07", 55000);
                data.Add(i, "08", 55000);
                data.Add(i, "09", 60000);
                data.Add(i, "10", 60000);
                data.Add(i, "11", 65000);
                data.Add(i, "12", 65000);
                data.Add(i, "13", 70000);
                data.Add(i, "14", 70000);
                data.Add(i, "15", 75000);
                data.Add(i, "16", 75000);
                data.Add(i, "17", 80000);
                data.Add(i, "18", 80000);

                var k = "19";
                data.Add(k, "00", 32000);
                data.Add(k, "01", 32000);
                data.Add(k, "02", 38000);
                data.Add(k, "03", 38000);
                data.Add(k, "04", 44000);
                data.Add(k, "05", 44000);
                data.Add(k, "06", 50000);
                // data.Add(k, "07", 50000);
                data.Add(k, "08", 55000);
                data.Add(k, "09", 55000);
                data.Add(k, "10", 60000);
                data.Add(k, "11", 60000);
                data.Add(k, "12", 65000);
                data.Add(k, "13", 65000);
                data.Add(k, "14", 70000);
                data.Add(k, "15", 70000);
                data.Add(k, "16", 75000);
                data.Add(k, "17", 75000);
                data.Add(k, "18", 80000);
                data.Add(k, "19", 80000);

                return data;
            }
        }
    }
}