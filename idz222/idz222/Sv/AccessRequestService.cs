using idz222.Helpers;
using System.Data.SqlClient;

namespace idz222.Sv
{
    public class AccessRequestService
    {
        private readonly DatabaseHelper _dbHelper = new DatabaseHelper();

        public void CreateRequest(int employeeId, string requestType)
        {
            string sql = "INSERT INTO AccessRequests (EmployeeId, RequestType) VALUES (@employeeId, @requestType)";
            _dbHelper.ExecuteNonQuery(sql,
                new SqlParameter("@employeeId", employeeId),
                new SqlParameter("@requestType", requestType));
        }

        public void ApproveRequest(int requestId)
        {
            string sql = @"
                UPDATE AccessRequests SET Status = 'Approved' 
                WHERE Id = @id AND Status = 'Pending';
                
                INSERT INTO RoomAccess (EmployeeId, IsActive)
                SELECT EmployeeId, 1 FROM AccessRequests 
                WHERE Id = @id AND RequestType = 'Grant';";

            _dbHelper.ExecuteNonQuery(sql, new SqlParameter("@id", requestId));
        }

        public void RejectRequest(int requestId)
        {
            string sql = @"
       
        UPDATE AccessRequests 
        SET Status = 'Rejected' 
        WHERE Id = @id;

        
        DELETE FROM RoomAccess
        WHERE EmployeeId IN (
            SELECT EmployeeId 
            FROM AccessRequests 
            WHERE Id = @id
        );";

            _dbHelper.ExecuteNonQuery(sql, new SqlParameter("@id", requestId));
        }
    }
}