using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Serilog;

namespace Karaoke01
{
    class Program
    {
        static void Main(string[] args)
        {
            string basePath = "C:\\tmp";
            ILogger log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            DBManager manager = new DBManager("Data Source=c:\\tmp\\KaraokeData.db;Version=3;");
            FolderReader folderReader = new FolderReader(log, basePath, manager);
            folderReader.LoadFileData();
        }
    }
}
