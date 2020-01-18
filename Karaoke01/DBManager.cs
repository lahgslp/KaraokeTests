using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Karaoke01
{
    public class DBManager
    {
        SQLiteConnection connection;
        public DBManager(string connectionString)
        {
            connection = SQLiteHelper.GetConnection(connectionString);
            connection.Open();
            Initialize();
        }
        public SQLiteConnection GetConnection()
        {
            return connection;
        }
        void Initialize()
        {
            CreateTable();
        }

        void CreateTable()
        {
            string createTableCommand = "CREATE TABLE IF NOT EXISTS File(Id INTEGER PRIMARY KEY, RelativePath TEXT, FileName TEXT, FileExtension TEXT);";
            SQLiteHelper.ExecuteNonQuery(connection, createTableCommand);
        }

        public void InsertFile(Dictionary<string, string> fileInfo)
        {
            string insertFileCommand = "INSERT INTO File(RelativePath, FileName, FileExtension) VALUES(?,?,?);";
            SQLiteHelper.ExecuteNonQuery(connection, insertFileCommand, fileInfo);
        }
        public string GetFilesCount()
        {
            string query = "SELECT COUNT(*) FROM File;";
            return SQLiteHelper.ExecuteScalar(connection, query);
        }
    }
}
