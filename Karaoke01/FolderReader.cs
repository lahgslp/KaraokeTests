using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Serilog;

namespace Karaoke01
{
    public class FolderReader
    {
        ILogger log;
        string baseFolder;
        DBManager manager;
        public FolderReader(ILogger _log, string _baseFolder, DBManager _manager)
        {
            log = _log;
            baseFolder = _baseFolder + (_baseFolder.EndsWith('\\') ? "" : "\\");
            manager = _manager;
        }
        public void LoadFileData()
        {
            LoadDirectory(baseFolder);
        }

        void LoadDirectory(string directory)
        {
            foreach (var folderItem in Directory.GetFiles(directory))
            {
                string fullFileName = folderItem.Replace(baseFolder, "");
                log.Information(fullFileName);
                manager.InsertFile(SplitFilePath(log, fullFileName));
            }
            foreach (var directoryItem in Directory.GetDirectories(directory))
            {
                LoadDirectory(directoryItem);
            }
        }
        static Dictionary<string, string> SplitFilePath(ILogger log, string fullFileName)
        {
            Dictionary<string, string> fileData = new Dictionary<string, string>();
            int lastDotIndex = fullFileName.LastIndexOf('.');
            int lastSlashIndex = fullFileName.LastIndexOf('\\');

            if (lastDotIndex > 0 && lastSlashIndex < lastDotIndex)
            {
                if (lastSlashIndex < 0)
                {
                    fileData.Add("relativePath", "");
                    fileData.Add("fileName", fullFileName.Substring(0, lastDotIndex));
                    fileData.Add("fileExtension", fullFileName.Substring(lastDotIndex + 1, fullFileName.Length - lastDotIndex - 1));
                }
                else
                {
                    fileData.Add("relativePath", fullFileName.Substring(0, lastSlashIndex));
                    fileData.Add("fileName", fullFileName.Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1));
                    fileData.Add("fileExtension", fullFileName.Substring(lastDotIndex + 1, fullFileName.Length - lastDotIndex - 1));
                }
            }
            else
            {
                //Sin extension?
                log.Information("Unknown pattern on file: " + fullFileName);
            }
            return fileData;
        }
    }
}
