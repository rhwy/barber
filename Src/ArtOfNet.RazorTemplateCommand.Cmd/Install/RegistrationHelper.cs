using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ArtOfNet.RazorTemplateCommand.Cmd
    {
    public class RegistrationHelper
        {

        private StringBuilder _sbLog = new StringBuilder();
        public StringBuilder Log
            {
            get
                {
                return _sbLog;
                }
            }

        public static bool ConsoleMode { get; set; }
        private string _target;

        public RegistrationHelper(string target)
            {
            _target = target;
            }

        public void LogLine(string content)
            {
            Log.AppendLine(content);
            if (RegistrationHelper.ConsoleMode)
                {
                Console.WriteLine(content);
                }
            }

        public void Save(string file)
            {
            string logFile = Path.Combine(_target, file);
            File.WriteAllText(logFile, Log.ToString());     
            }

        public bool RegisterSnapIn()
            {
            bool result = true;
            result &= RegisterInGac();
            result &= RegisterInstallUtil();
            result &= RegisterPath();
            return result;
            }

        private string FindFile(string pattern, string defaultPath)
            {
            string msNetDir = Path.Combine(Environment.GetEnvironmentVariable("windir"), "Microsoft.Net");
            string progFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            string progFilesX64 = Environment.GetEnvironmentVariable("ProgramW6432");

            FileInfo result =
            ShellUtil
                .FindFile(msNetDir, pattern)
                .JoinFile(
                    ShellUtil.FindFile(progFilesX64, pattern),
                    ShellUtil.FindFile(progFilesX86, pattern))
                .OrderByDescending(
                    f => f.CreationTime)
                .FirstOrDefault(defaultPath);

            return result.FullName;
            }
        
        private bool RegisterInGac()
            {
            if (string.IsNullOrEmpty(_target))
                {
                LogLine("registration target is null or empty, we can not continue");
                return false;
                }

            string defaultCommand = DefaultConfiguration.Current.GacUtilCommand;//
            string gacCommandTemplate = @"""{0}"" ";
            string gacArgumentsTemplate = @" -if ""{0}""";

            string gacCommand = string.Format(gacCommandTemplate,FindFile("gacutil.exe", defaultCommand));
            string library = Path.Combine(_target, "ArtOfNet.RazorTemplateCommand.PSSnapIn.dll");
            string gacArguments = string.Format(gacArgumentsTemplate, library);

            if(string.IsNullOrEmpty(gacCommand))
                {
                LogLine("GacUtil command was NOT found on your system");
                return false;
                }
            else
                {
                LogLine("GacUtil command found on your system, we'll execute this commmand:");
                LogLine("Command  :" + gacCommand);
                LogLine("Args     :" + gacArguments);

                try
                    {
                    Process ps = new Process();
                    ProcessStartInfo psi = new ProcessStartInfo(gacCommand, gacArguments);
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                    }
                catch (Exception ex)
                    {
                    string message = "Error calling gacutil:";
                    message += "Command : " + gacCommand;
                    message += "Args    : " + gacArguments;
                    message += "Error   : " + ex.Message;

                    throw new Exception(message);
                    }
                
                }
            return true;
            }

        private bool RegisterInstallUtil()
            {
            if (string.IsNullOrEmpty(_target))
                {
                LogLine("registration target is null or empty, we can not continue");
                return false;
                }

            string defaultCommand = @"C:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe";//
            string gacCommandTemplate = @"""{0}"" ";
            string gacArgumentsTemplate = @" ""{0}""";

            string gacCommand = string.Format(gacCommandTemplate,FindFile("installutil.exe", defaultCommand));
            string library = Path.Combine(_target, "ArtOfNet.RazorTemplateCommand.PSSnapIn.dll");
            string gacArguments = string.Format(gacArgumentsTemplate, library);

            if(string.IsNullOrEmpty(gacCommand))
                {
                LogLine("InstallUtil command was NOT found on your system");
                return false;
                }
            else
                {
                LogLine("Install Util command found on your system, we'll execute this commmand:");
                LogLine("Command  :" + gacCommand);
                LogLine("Args     :" + gacArguments);

                try
                    {
                    Process ps = new Process();
                    ProcessStartInfo psi = new ProcessStartInfo(gacCommand, gacArguments);
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                    }
                catch (Exception ex)
                    {
                    string message = "Error calling gacutil:";
                    message += "Command : " + gacCommand;
                    message += "Args    : " + gacArguments;
                    message += "Error   : " + ex.Message;

                    throw new Exception(message);
                    }
                
                }
            return true;
            }

        private bool RegisterPath()
            {
            //add folder to path:
            string currentPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            if (!currentPath.Contains(_target))
                {
                currentPath += ";" + _target + ";";
                Environment.SetEnvironmentVariable("Path", currentPath,EnvironmentVariableTarget.Machine);

                LogLine("add installtion folder to your PATH");
                }
            else
                {
                LogLine("installtion folder is already in your PATH");
                }

            //verify
            currentPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            if (!currentPath.Contains(_target))
                {
                LogLine("registration unsuccessfull, directory is not in your path, add it manually in order to user rtc.exe everywhere");
                return false;
                }
            else
                {
                LogLine("registration successfull");
                return true;
                }
            }
        }


    }
