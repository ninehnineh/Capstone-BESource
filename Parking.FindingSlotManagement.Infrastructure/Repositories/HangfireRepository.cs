using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class HangfireRepository : IHangfireRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public HangfireRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> DeleteJob(int bookingId, string? methodName)
        {
            try
            {
                var result = "";
                var query = "DELETE FROM HangFire.Job WHERE HangFire.Job.Arguments LIKE '%' + @arguments + '%' AND HangFire.Job.InvocationData LIKE '%' + @methodName + '%'";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@arguments", bookingId.ToString());
                        command.Parameters.AddWithValue("@methodName", methodName);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = "Xóa thành công.";
                        }
                        else
                        {
                            result = "Không tìm thấy dữ liệu để xóa.";
                        }
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Get Tao Lao" + ex.Message);
            }
        }

        public async Task<string> DeleteJob(int bookingId)
        {
            try
            {
                var result = "";
                var query = "DELETE FROM HangFire.Job WHERE HangFire.Job.Arguments LIKE '%' + @arguments + '%'";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@arguments", bookingId.ToString());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = "Xóa thành công.";
                        }
                        else
                        {
                            result = "Không tìm thấy dữ liệu để xóa.";
                        }
                    }
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Get Tao Lao" + ex.Message);
            }
        }
    }
}