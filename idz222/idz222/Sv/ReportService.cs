using idz222.Helpers;
using idz222.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace idz222.Sv
{
    public class ReportService
    {
        private readonly DatabaseHelper _dbHelper;

        public ReportService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public List<AccessLog> GetAccessLogs(DateTime date)
        {
            var logs = new List<AccessLog>();
            string sql = "SELECT * FROM AccessLogs WHERE CONVERT(DATE, EntryTime) = @date ORDER BY EntryTime DESC";

            using (var reader = _dbHelper.ExecuteReader(sql, new SqlParameter("@date", date.Date)))
            {
                while (reader.Read())
                {
                    logs.Add(new AccessLog
                    {
                        Id = (int)reader["Id"],
                        EmployeeId = (int)reader["EmployeeId"],
                        EntryTime = (DateTime)reader["EntryTime"],
                        ExitTime = reader["ExitTime"] as DateTime?
                    });
                }
            }
            return logs;
        }

        public string GenerateAccessReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== ОТЧЕТ ПО ДОСТУПАМ ===");
            report.AppendLine("Дата формирования: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            report.AppendLine();

            report.AppendLine("Сотрудники с доступом:");
            string sql = @"SELECT e.FullName, d.Name as Department 
                          FROM RoomAccess ra
                          JOIN Employees e ON ra.EmployeeId = e.Id
                          JOIN Departments d ON e.DepartmentId = d.Id
                          WHERE ra.IsActive = 1";

            using (var reader = _dbHelper.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    report.AppendLine($"- {reader["FullName"]} ({reader["Department"]})");
                }
            }

            report.AppendLine();
            report.AppendLine("Последние 5 входов:");
            var lastEntries = GetAccessLogs(DateTime.Today);
            foreach (var entry in lastEntries.Take(5))
            {
                report.AppendLine($"{entry.EntryTime:HH:mm:ss} - Сотрудник ID: {entry.EmployeeId}");
            }

            return report.ToString();
        }
    }
}