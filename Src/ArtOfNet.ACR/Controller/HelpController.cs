using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtOfNet.ACR
{
    public class HelpController : RtcControllerBase<HelpArgumentModel>
    {
        public override ControllerResult Execute()
        {
            string doc = @"
rtc is a command line utility for transforming Microsoft Razor engine content.

[] Available actions:
=====================

- Help : show this help
- Install : install the current program into your path and declares the Powershell snapin
- Render : process the razor template

[] Render options:
=====================
Rtc can render Razor content with data model from multiple sources, actually including : json, properties and xml
Due to the specificity of each format, the use of the data with a template should vary from one to another. 

-tf : Template file, full path to template file
-tc : Template content, provide directly the content of your template
-mf : Model File,full path to your model file
-mc : Model Content, a string with your model
-t  : Model Type, Your model should be Json,Dictionary,Properties,Csv,Xml,Binary,Scalar
-r  : Result File, full path for the generated result
-id : result identifier : get a string guid to identify the transformed result


[] Samples:
=====================
With perperties content:
>> rtc render -tc ""hello @Model.Prop.Name"" -mc ""[Prop]\r\nName=Rui"" -t ""properties""
With json file and save to result.txt file:
>> rtc render -tc ""hello @Model.Name"" -mc ""user.js"" -t ""json"" -r ""./result.txt""";

            return new StringResult(doc,true);
        }
    }
}
