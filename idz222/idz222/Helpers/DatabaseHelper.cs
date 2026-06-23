using System.Data;
using System.Data.SqlClient;

namespace idz222.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString = "Server=DESKTOP-9VF0AQD;Database=idz;User ID=test2;Password=514251425142;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            var connection = GetConnection();
            connection.Open();
            var command = new SqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}