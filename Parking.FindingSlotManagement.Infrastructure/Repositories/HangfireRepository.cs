using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Application.Models.Parking;

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
                var connection = new SqlConnection(connectionString);
                connection.Open();

                var query = "DELETE FROM HangFire.Job WHERE HangFire.Job.Arguments LIKE '%' + @arguments + '%' AND HangFire.Job.InvocationData LIKE '%' + @methodName + '%'";
                var command = new SqlCommand(query, connection);
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

                // Use the connection object for other queries

                // Close the connection when you are done
                connection.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at DeleteJob by booking id and method name: Message " + ex.Message);
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
                throw new Exception($"Error at DeleteJob by booking id: Message " + ex.Message);
            }
        }

        public async Task<string> DeleteScheduledJob(int parkingId, DateTime disableDate)
        {
            var methodName = "DisableParkingByDate";
            try
            {
                var result = "";
                var query = "DELETE FROM HangFire.Job " +
                            "WHERE JSON_VALUE(HangFire.Job.InvocationData, '$.m') = @MethodName AND " +
                                    "JSON_VALUE(HangFire.Job.Arguments, '$[0]') = @ParkingId AND " +
                                    "HangFire.Job.Arguments LIKE '%' + @DisableDate + '%'";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MethodName", methodName);
                        command.Parameters.AddWithValue("@ParkingId", parkingId.ToString());
                        command.Parameters.AddWithValue("@DisableDate", disableDate.ToString());

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
                throw new Exception("Error at DeleteScheduledJob: Message " + ex.Message);
            }
        }

        public async Task<IEnumerable<Job>> GetHistoryDisableParking(int parkingId)
        {
            string methodName = "DisableParkingByDate";
            try
            {
                var result = new List<Job>();
                // var query = "SELECT * " +
                //             "FROM HangFire.Job " +
                //             "WHERE JSON_VALUE(HangFire.Job.InvocationData, '$.m') = '@MethodName' AND JSON_VALUE(HangFire.Job.Arguments, '$[0]') = '@ParkingId' ";

                var query = "SELECT * " +
                            "FROM HangFire.Job " +
                            "WHERE JSON_VALUE(HangFire.Job.InvocationData, '$.m') = @MethodName AND JSON_VALUE(HangFire.Job.Arguments, '$[0]') = @ParkingId";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MethodName", methodName);
                        command.Parameters.AddWithValue("@ParkingId", parkingId.ToString());

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                // DateTime expireAt = reader.GetDateTime(reader.GetOrdinal("ExpireAt"));
                                var history = new Job
                                {
                                    StateName = reader["StateName"].ToString(),
                                    InvocationData = reader["InvocationData"].ToString(),
                                    Arguments = reader["Arguments"].ToString(),
                                    CreatedAt = createdAt,
                                    // ExpireAt = expireAt,
                                };

                                result.Add(history);
                            }
                            reader.Close();
                        };
                    };
                }

                return result.ToList();
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Error at GetHistoryDisableParking: Message {ex.Message}");
            }
        }
    }
}