using Moq;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Application.UnitTests.HandlerTesting.Booking
{
    public class CaculatePriceBookingHandlerTests
    {


        public CaculatePriceBookingHandlerTests()
        {
            
        }

        [Theory]
        [MemberData(nameof(CalculatorTestData.TestCases), MemberType = typeof(CalculatorTestData))]
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
            DateTime startTime = DateTime.Parse($"2023-05-25T{x}:00:00");
            DateTime endTime = DateTime.Parse($"2023-05-25T{y}:00:00");

            //var result = GetPackagesApplied.GetPackageApplied(packages, startTime, endTime);

            //Assert.Equal(price, result.Price);
        }

        public class CalculatorTestData
        {
            public static TheoryData<string, string, decimal> TestCases()
            {
                var data = new TheoryData<string, string, decimal>();

                data.Add("06", "11", 36000);
                data.Add("06", "12", 36000);
                data.Add("06", "13", 42000);
                data.Add("06", "14", 42000);
                data.Add("06", "15", 48000);

                data.Add("07", "11", 30000);
                data.Add("07", "12", 36000);
                data.Add("07", "13", 36000);
                data.Add("07", "14", 42000);
                data.Add("07", "15", 42000);

                data.Add("08", "11", 31000);
                data.Add("08", "12", 31000);
                data.Add("08", "13", 37000);
                data.Add("08", "14", 37000);
                data.Add("08", "15", 43000);

                data.Add("09", "11", 26000);
                data.Add("09", "12", 32000);
                data.Add("09", "13", 32000);
                data.Add("09", "14", 38000);
                data.Add("09", "15", 38000);
                //===========================
                data.Add("06", "16", 48000);
                data.Add("06", "17", 55000);
                data.Add("06", "18", 55000);
                data.Add("06", "19", 62000);
                data.Add("06", "20", 62000);

                data.Add("07", "16", 49000);
                data.Add("07", "17", 49000);
                data.Add("07", "18", 56000);
                data.Add("07", "19", 56000);
                data.Add("07", "20", 63000);
                //===========================
                data.Add("10", "16", 38000);
                data.Add("10", "17", 45000);
                data.Add("10", "18", 45000);
                data.Add("10", "19", 52000);
                data.Add("10", "20", 52000);

                data.Add("11", "16", 39000);
                data.Add("11", "17", 39000);
                data.Add("11", "18", 46000);
                data.Add("11", "19", 46000);
                data.Add("11", "20", 53000);

                data.Add("14", "16", 27000);
                data.Add("14", "17", 34000);
                data.Add("14", "18", 34000);
                data.Add("14", "19", 41000);
                data.Add("14", "20", 41000);


                // Add more test cases here

                return data;
            }
        }
    }
}
