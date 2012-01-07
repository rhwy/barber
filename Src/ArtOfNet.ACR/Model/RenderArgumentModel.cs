using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;
using System.ComponentModel;

namespace ArtOfNet.ACR
{
    [ArgsModel(SwitchDelimiter = "-")]
    public class RenderArgumentModel
    {
        [Description("Template file : full path to template file")]
        [ArgsMemberSwitch("tf","TemplateFile")]
        public string TemplateFile { get; set; }

        [Description("Template content : provide directly the content of your template")]
        [ArgsMemberSwitch("tc","TemplateContent")]
        public string TemplateContent { get; set; }

        [Description("Model File : full path to your model file")]
        [ArgsMemberSwitch("mf","ModelFile")]
        public string ModelFile { get; set; }

        [Description("Model Content : a string with your model")]
        [ArgsMemberSwitch("mc","ModelContent")]
        public string ModelContent { get; set; }

        [Description("Model Type : Your model should be Json,Dictionary,Properties,Csv,Xml,Binary,Scalar")]
        [ArgsMemberSwitch("t","type")]
        [DefaultValue(ModelType.Json)]
        public ModelType Type {get;set;}

        [Description("Result File : full path for the generated result")]
        [ArgsMemberSwitch("r","result")]
        public string ResultFile { get; set; }

        [Description("result identifier : get a string guid to identify the transformed result")]
        [ArgsMemberSwitch("id")]
        public string Id { get; set; }


    }
}
