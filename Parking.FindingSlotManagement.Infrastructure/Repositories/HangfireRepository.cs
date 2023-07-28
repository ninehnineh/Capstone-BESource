using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class HangfireRepository : IHangfireRepository
    {
        public async Task<string> DeleteJob(int bookingId, string? methodName)
        {
            try
            {
                var result = "";
                var query = "DELETE FROM HangFire.Job WHERE HangFire.Job.Arguments LIKE '%' + @arguments + '%' AND HangFire.Job.InvocationData LIKE '%' + @methodName + '%'";
                using (var connection = new SqlConnection("server =chinh\\sqlexpress01; database = parkzv10;uid=sa;pwd=123123123;"))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@arguments", bookingId.ToString());
                        command.Parameters.AddWithValue("@methodName", methodName);

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

        public async Task<string> DeleteJob(int bookingId)
        {
            try
            {
                var result = "";
                var query = "DELETE FROM HangFire.Job WHERE HangFire.Job.Arguments LIKE '%' + @arguments + '%'";

                using (var connection = new SqlConnection("server =chinh\\sqlexpress01; database = parkzv10;uid=sa;pwd=123123123;"))
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