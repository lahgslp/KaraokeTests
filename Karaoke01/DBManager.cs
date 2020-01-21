using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;

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
            if (fileInfo != null && fileInfo.Count > 0)
            {
                string insertFileCommand = "INSERT INTO File(RelativePath, FileName, FileExtension) VALUES(?,?,?);";
                SQLiteHelper.ExecuteNonQuery(connection, insertFileCommand, fileInfo);
            }
        }
        public void DeleteFilesInTable()
        {
            string deleteFilesInTableCommand = "DELETE FROM File;";
            SQLiteHelper.ExecuteNonQuery(connection, deleteFilesInTableCommand);
        }
        public string GetFilesCount()
        {
            string query = "SELECT COUNT(*) FROM File;";
            return SQLiteHelper.ExecuteScalar(connection, query);
        }

        public DataSet GetZipFiles()
        {
            DataSet ds = new DataSet();
            string query = "SELECT Id, RelativePath, FileName, FileExtension FROM File WHERE lower(FileExtension) = 'zip' ORDER BY RelativePath, FileName;";
            var adapter = SQLiteHelper.GetDataAdapter(connection, query);
            adapter.Fill(ds);
            return ds;
        }
        public DataSet GetCDGFiles()
        {
            DataSet ds = new DataSet();
            string query = "SELECT Id, RelativePath, FileName, FileExtension FROM File WHERE lower(FileExtension) = 'cdg' ORDER BY RelativePath, FileName;";
            var adapter = SQLiteHelper.GetDataAdapter(connection, query);
            adapter.Fill(ds);
            return ds;
        }
    }
}
