using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Models;
using Parking.FindingSlotManagement.Application.Models.Booking;
using Parking.FindingSlotManagement.Application.Models.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.VnPay
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private static string _hashSecret;
        private static string _hashSecretDeposit;
        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentUrl(BookingTransaction model, string tmnCode, string hashSecret, HttpContext context)
        {
            _hashSecret = hashSecret;
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.TotalPrice}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], hashSecret);

            return paymentUrl;
        }
        public string CreatePaymentUrlForDeposit(DepositTransaction model, string tmnCode, string hashSecret, HttpContext context)
        {
            _hashSecretDeposit = hashSecret;
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrlDeposit"];
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.TotalPrice}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack + "?userId=" + model.UserId);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], hashSecret);

            return paymentUrl;
        }
        public string CreatePaymentUrlForDepositForManager(DepositTransaction model, string tmnCode, string hashSecret, HttpContext context)
        {
            _hashSecretDeposit = hashSecret;
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrlManager"];
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.TotalPrice}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack + "?userId=" + model.UserId);
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], hashSecret);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var x = _hashSecret;
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _hashSecret);

            return response;
        }
        public PaymentResponseModel PaymentExecuteForDeposit(IQueryCollection collections)
        {
            var x = _hashSecretDeposit;
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _hashSecretDeposit);

            return response;
        }
    }
}
