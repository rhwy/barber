using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.RazorTemplateCommand.Cmd
    {
    public class DefaultConfiguration
        {
        private static readonly DefaultConfiguration _current = new DefaultConfiguration();
        public static DefaultConfiguration Current
            {
            get
                {
                return _current;
                }
            }

        private string _gacUtilCommand = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\gacutil.exe";
        public string GacUtilCommand
            {
            get
                {
                return _gacUtilCommand;
                }
            set
                {
                _gacUtilCommand = value;
                }
            }

        private string _installUtilCommand = @"C:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe";
        public string InstallUtilCommand
            {
            get
                {
                return _installUtilCommand;
                }
            set
                {
                _installUtilCommand = value;
                }
            }

        }

        
    }
