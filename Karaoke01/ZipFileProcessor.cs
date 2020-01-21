using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Serilog;
using System.Data;

namespace Karaoke01
{
    class ZipFileProcessor
    {
        ILogger log;
        string UnzipToolPath;
        string BasePath;
        public ZipFileProcessor(ILogger _log, string basePath, string unzipToolPath)
        {
            log = _log;
            BasePath = basePath;
            UnzipToolPath = unzipToolPath;
        }

        public void ProcessZipFiles(DataSet zipFilesDS)
        {
            log.Information("Found " + zipFilesDS.Tables[0].Rows.Count + " zip files...");
            string fullFileName;
            string unzipcommand;
            for (int rowIndex = 0; rowIndex < zipFilesDS.Tables[0].Rows.Count; rowIndex++)
            {
                fullFileName = BasePath + "\\"
                    + zipFilesDS.Tables[0].Rows[rowIndex]["RelativePath"].ToString()
                    + (zipFilesDS.Tables[0].Rows[rowIndex]["RelativePath"].ToString() == "" ? "" : "\\")
                    + zipFilesDS.Tables[0].Rows[rowIndex]["FileName"].ToString() + "."
                    + zipFilesDS.Tables[0].Rows[rowIndex]["FileExtension"].ToString();
                //log.Information("Processing \"" + fullFileName + "\"");
                if (ProcessIndividualZipFFile(UnzipToolPath, "x " + "\"" + fullFileName + "\" -o" + BasePath + "\\" + zipFilesDS.Tables[0].Rows[rowIndex]["RelativePath"].ToString()) == 0)
                {
                    log.Information("Processed \"" + fullFileName + "\"");
                }
                else
                {
                    log.Error("Problem with zip file \"" + fullFileName + "\"");
                }
            }
        }

        private int ProcessIndividualZipFFile(string tool, string parameters)
        {
            Process process = new Process();
            // Stop the process from opening a new window
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Setup executable and parameters
            process.StartInfo.FileName = tool;
            process.StartInfo.Arguments = parameters;

            // Go
            process.Start();
            process.WaitForExit();

            return process.ExitCode;
        }
    }
}
