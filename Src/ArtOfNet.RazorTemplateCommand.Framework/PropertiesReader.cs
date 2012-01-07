using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Dynamic;
using ArtOfNet.FluentConfiguration.Core;
using FluentConfiguration.Core;
using NLog;

namespace ArtOfNet.RazorTemplateCommand.Framework
{
    public class PropertiesReader
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private string _source;
        private dynamic _properties;

        public PropertiesReader(string source)
        {
            _source = source;
            _logger.Debug("Properties Source : {0}",source);
        }

        public dynamic GetProperties()
        {
            if (_properties == null)
            {
                try
                {
                    ProcessProperties();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);

                    _properties = new DynamicConfigurationValues(true);
                    _properties.HasError = "true";
                    _properties.Error = ex;
                }
                
            }
            return _properties;
        }

        private void ProcessProperties()
        {
            if (string.IsNullOrEmpty(_source))
            {
                return;
            }
            _source = _source.Replace("\\r\\n",Environment.NewLine);
            string[] rows = _source.Split(Environment.NewLine.ToCharArray());
            
            dynamic properties = new DynamicConfigurationValues(true);
            
            string rexGroupPattern = @"\s*\[\s*(\w+)\s*\]\s*";
            string current = string.Empty;

            var validRows = from row in rows
                            where !string.IsNullOrEmpty(row.Trim())
                                && row.Trim().Length > 2                                
                                && !row.TrimStart().StartsWith("#")
                            select row;

            foreach (string row in validRows)
            {
                Match match = Regex.Match(row, rexGroupPattern);
                if (match.Success)
                {
                    current = match.Groups[1].Value;
                    current = SanitizeSectionTitle(current);

                    if (!properties.HasProperty(current))
                    {
                        properties[current] = new DynamicConfigurationValues();
                    }
                    
                }
                else
                {
                    if (row.Contains("=") && !string.IsNullOrEmpty(current))
                    {
                        string[] pair = row.Trim().Split("=".ToCharArray());
                        if(pair.Length==2)
                        {
                            if (!properties[current].HasProperty(pair[0]))
                            {
                                properties[current][pair[0]] = pair[1];
                            }
                            else
                            {
                                if (!(properties[current][pair[0]] is IEnumerable<string>))
                                {
                                    string singlePreviousValue = properties[current][pair[0]];
                                    properties[current][pair[0]] = new List<string>();
                                    properties[current][pair[0]].Add(singlePreviousValue);
                                }
                                properties[current][pair[0]].Add(pair[1]);
                            }
                        }
                        
                    }
                }
            }
            _properties = properties;
        }

        public string SanitizeSectionTitle(string title)
        {
            string sanitizePattern = @"[^a-zA-Z0-9]";
            string result = Regex.Replace(title,sanitizePattern,"");
            return result;
        }
    }
}
