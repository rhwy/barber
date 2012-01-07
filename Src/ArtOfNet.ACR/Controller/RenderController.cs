using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ArtOfNet.RazorTemplateCommand.Framework;
using NLog;

namespace ArtOfNet.ACR
{
    public class RenderController : RtcControllerBase<RenderArgumentModel>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public RenderController()
        {

        }

        public override ControllerResult Execute()
        {
            

            if (!string.IsNullOrEmpty(ArgumentModel.TemplateFile))
            {
                if (File.Exists(ArgumentModel.TemplateFile))
                {
                    ArgumentModel.TemplateContent = File.ReadAllText(ArgumentModel.TemplateFile);
                }
            }

            if (!string.IsNullOrEmpty(ArgumentModel.ModelFile))
            {
                if (File.Exists(ArgumentModel.ModelFile))
                {
                    ArgumentModel.ModelContent = File.ReadAllText(ArgumentModel.ModelFile);
                }
            }

            string result = string.Empty;

            if (string.IsNullOrEmpty(ArgumentModel.ModelContent))
            {
                result = ArgumentModel.TemplateContent;
            }
            else
            {
                switch (ArgumentModel.Type)
                {
                    case ModelType.Json:
                        result = RazorEngineHelper.ProcessTemplateWithJsonModel(ArgumentModel.TemplateContent, ArgumentModel.ModelContent);
                        break;
                    case ModelType.Dictionary:
                    case ModelType.Properties:
                        result = RazorEngineHelper.ProcessTemplateWithPropertiesModel(ArgumentModel.TemplateContent, ArgumentModel.ModelContent);
                        break;
                    case ModelType.Csv:
                    case ModelType.Xml:
                        result = RazorEngineHelper.ProcessTemplateWithXmlModel(ArgumentModel.TemplateContent, ArgumentModel.ModelContent);
                        break;
                    case ModelType.Binary:
                    case ModelType.Scalar:
                    default:
                        result = RazorEngineHelper.ProcessTemplateWithJsonModel(ArgumentModel.TemplateContent, ArgumentModel.ModelContent);
                        break;
                }
                
            }

            if (!string.IsNullOrEmpty(ArgumentModel.ResultFile))
            {
                return new FileResult(result,ArgumentModel.ResultFile);
            }
            else
            {
                return new StringResult(result);
            }
            
        }
    }
}
