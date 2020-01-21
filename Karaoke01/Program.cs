using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace Karaoke01
{
    class Program
    {
        enum Command
        {
            LoadFiles,
            ResetDB,
            ProcessZip,
            ProcessCDG,
            BuildBaseForKaraoke,
            ReprocessKaraokeData,
            Unknown
        }
        static Command TranslateCommand(string inputParameter)
        {
            switch (inputParameter)
            {
                case "LoadFiles": return Command.LoadFiles;
                case "ResetDB": return Command.ResetDB;
                case "ProcessZip": return Command.ProcessZip;
                case "ProcessCDG": return Command.ProcessCDG;
                case "BuildBaseForKaraoke": return Command.BuildBaseForKaraoke;
                case "ReprocessKaraokeData": return Command.ReprocessKaraokeData;
                default: return Command.Unknown;
            }
        }
        static void Main(string[] args)
        {
            string appSettingsFile = "appsettings.json";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsFile);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(appSettingsFile, true, true)
                .Build();

            string basePath = config["AppSettings:BasePath"];
            string connectionString = config["ConnectionStrings:SQLite"];
            string UnzipToolPath = config["AppSettings:UnzipToolPath"];
            string ffmpegToolPath = config["AppSettings:ffmpegToolPath"];

            ILogger log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path: "ProcessingOutputLog.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Command command = args.Length > 0 ? TranslateCommand(args[0]) : Command.Unknown;

                DBManager manager = new DBManager(connectionString);

                switch (command)
                {
                    case Command.LoadFiles:
                        log.Information("Loading files from database started...");
                        FolderReader folderReader = new FolderReader(log, basePath, manager);
                        folderReader.LoadFileData();
                        log.Information("Loading files from database completed... " + manager.GetFilesCount() + " files added");
                        break;
                    case Command.ResetDB:
                        log.Information("Deleting all file elements from DB started...");
                        manager.DeleteFilesInTable();
                        log.Information("Deleting all file elements from DB completed...");
                        break;
                    case Command.ProcessZip:
                        log.Information("Processing zip files started...");
                        ZipFileProcessor zipFileProcessor = new ZipFileProcessor(log, basePath, UnzipToolPath);
                        zipFileProcessor.ProcessZipFiles(manager.GetZipFiles());
                        log.Information("Processing zip files completed...");
                        break;
                    case Command.ProcessCDG:
                        log.Information("Processing CDG files started...");
                        log.Information("Processing CDG files completed...");
                        break;
                    case Command.BuildBaseForKaraoke:
                        log.Information("BuildBaseForKaraoke started...");
                        log.Information("BuildBaseForKaraoke completed...");
                        break;
                    case Command.ReprocessKaraokeData:
                        log.Information("ReprocessKaraokeData started...");
                        log.Information("ReprocessKaraokeData completed...");
                        break;
                    case Command.Unknown:
                        log.Information("Command was not provided");
                        break;
                }
            }
            catch (Exception exc)
            {
                log.Error(exc, "Error in application");
            }
        }
    }
}
