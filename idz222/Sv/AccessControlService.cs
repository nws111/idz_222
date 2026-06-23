using idz222.Helpers;
using idz222.Models;
using System;
using System.Data.SqlClient;

namespace idz222.Sv
{
    public class AccessControlService
    {
        private readonly DatabaseHelper _dbHelper;

        public AccessControlService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public (bool IsSuccess, string Message) RegisterEntry(int employeeId)
        {
            bool hasAccess = CheckAccess(employeeId);

            if (!hasAccess)
            {
                return (false, "Доступ запрещен: у сотрудника нет активного пропуска");
            }

            try
            {
                string sql = "INSERT INTO AccessLogs (EmployeeId) VALUES (@employeeId)";
                _dbHelper.ExecuteNonQuery(sql, new SqlParameter("@employeeId", employeeId));

                return (true, "Вход успешно зарегистрирован");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при регистрации входа: {ex.Message}");
            }
        }

        public bool CheckAccess(int employeeId)
        {
            string sql = @"
                SELECT COUNT(*) 
                FROM RoomAccess 
                WHERE EmployeeId = @employeeId 
                AND IsActive = 1";

            using (var reader = _dbHelper.ExecuteReader(sql, new SqlParameter("@employeeId", employeeId)))
            {
                if (reader.Read())
                {
                    return ((int)reader[0]) > 0;
                }
            }
            return false;
        }

        public void RegisterExit(int employeeId)
        {
            
            string findSql = @"
                SELECT TOP 1 Id 
                FROM AccessLogs 
                WHERE EmployeeId = @employeeId 
                AND ExitTime IS NULL 
                ORDER BY EntryTime DESC";

            int? logId = null;

            using (var reader = _dbHelper.ExecuteReader(findSql, new SqlParameter("@employeeId", employeeId)))
            {
                if (reader.Read())
                {
                    logId = (int)reader["Id"];
                }
            }

            if (logId.HasValue)
            {
                string updateSql = "UPDATE AccessLogs SET ExitTime = GETDATE() WHERE Id = @id";
                _dbHelper.ExecuteNonQuery(updateSql, new SqlParameter("@id", logId.Value));
            }
        }
    }
}