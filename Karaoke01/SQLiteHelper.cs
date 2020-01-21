using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Karaoke01
{
    class SQLiteHelper
    {
        public static SQLiteConnection GetConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }
        public static SQLiteCommand GetCommand(SQLiteConnection connection, string commandText, Dictionary<string, string> parameters)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(new SQLiteParameter(parameter.Key, parameter.Value));
                }
            }
            return command;
        }
        public static SQLiteDataAdapter GetDataAdapter(SQLiteConnection connection, string commandText)
        {
            return GetDataAdapter(connection, commandText, null);
        }
        public static SQLiteDataAdapter GetDataAdapter(SQLiteConnection connection, string commandText, Dictionary<string, string> parameters)
        {
            var command = GetCommand(connection, commandText, parameters);
            return new SQLiteDataAdapter(command);
        }
        public static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            ExecuteNonQuery(connection, query, null);
        }
        public static void ExecuteNonQuery(SQLiteConnection connection, string query, Dictionary<string, string> parameters)
        {
            var cmd = GetCommand(connection, query, parameters);
            cmd.ExecuteNonQuery();
        }
        public static SQLiteDataReader ExecuteReader(SQLiteConnection connection, string query)
        {
            return ExecuteReader(connection, query, null);
        }
        public static SQLiteDataReader ExecuteReader(SQLiteConnection connection, string query, Dictionary<string, string> parameters)
        {
            var cmd = GetCommand(connection, query, parameters);
            return cmd.ExecuteReader();
        }
        public static string ExecuteScalar(SQLiteConnection connection, string query)
        {
            return ExecuteScalar(connection, query, null);
        }
        public static string ExecuteScalar(SQLiteConnection connection, string query, Dictionary<string, string> parameters)
        {
            var cmd = GetCommand(connection, query, parameters);
            return cmd.ExecuteScalar().ToString();
        }
    }
}
