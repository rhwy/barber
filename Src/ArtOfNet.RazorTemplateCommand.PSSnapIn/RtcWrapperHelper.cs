using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Pipes;
using Newtonsoft.Json;
using System.IO;
using ArtOfNet.NamedPipesHelper;
using NLog;

namespace ArtOfNet.RazorTemplateCommand.PSSnapIn
{
    public class RtcWrapperHelper
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        public string MakePipedCommand(string template, object model)
        {
            //_logger.Info("Entering Pipe command mode");
            string serverApp = "rtc.exe";
            string result = string.Empty;
            
            using (PipedChildProcess process = new PipedChildProcess())
            {
                process.FileName = serverApp;
                process.Arguments = "-m NamedPipes";
                process.PipeName = "InProc";
                process.Start();
                string value = JsonConvert.SerializeObject(model);
                result = process.Run("Render", "-tc", template, "-mc", value);
                
            }
            return result;
        }

        public string RunCommand(string template, object model, string outputFile, string type, bool quotes, bool debug)
        {
            //_logger.Info("Entering standard process command mode");
            string value = JsonConvert.SerializeObject(model);
            if (quotes)
            {
                value = value.Replace(@"""", "'");
            }
            string serverApp = "rtc.exe";
            
            string result = string.Empty;
            string resultFilePath = outputFile;
            bool isTempOutput = false;
            if (string.IsNullOrEmpty(resultFilePath))
            {
                string id = Guid.NewGuid().ToString();
                string tempDir = Path.Combine(Environment.CurrentDirectory, "Temp");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                resultFilePath = Path.Combine(tempDir, string.Format("Template_{0}.rr", id));
                isTempOutput = true;
            }

            if (string.IsNullOrEmpty(type))
            {
                type = "Json";
            }
            string commandArgs = string.Format(@"render -tc ""{0}"" -mc ""{1}"" -r ""{2}"" -t ""{3}""", template, value, resultFilePath,type);
            //_logger.Info("Args : " + commandArgs);
            if (debug)
            {
                commandArgs = "StandAlone -o wait " + commandArgs;
            }
            Process process = new Process();
            process.StartInfo.FileName = serverApp;
            process.StartInfo.Arguments = commandArgs;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = false;

            if (!process.Start())
            {
                result = "Process could not be started";
            }
            else
            {
                process.WaitForExit();
                process.Close();
                //_logger.Info("Child process closed successfully");
                if (File.Exists(resultFilePath))
                {
                    //_logger.Info("Result temp file exists");
                    result = File.ReadAllText(resultFilePath);
                    if (!string.IsNullOrEmpty(result) && isTempOutput)
                    {
                        //_logger.Info("result length : {0} chars", result.Length);
                        File.Delete(resultFilePath);
                    }
                    else
                    {
                        //_logger.Info("result seems to be empty, temp file was not deleted");
                    }
                }
            }
            return result;
        }
    }
}
