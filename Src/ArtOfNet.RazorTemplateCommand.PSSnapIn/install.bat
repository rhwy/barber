@ECHO OFF

REM "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\gacutil.exe" -if "E:\Sources\bitbucket\artofnet-razortemplatecommand\Src\ArtOfNet.RazorTemplateCommand.PSSnapIn\bin\Debug\ArtOfNet.RazorTemplateCommand.PSSnapIn.dll"
"C:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe"  -i "E:\Sources\bitbucket\artofnet-razortemplatecommand\Src\ArtOfNet.RazorTemplateCommand.PSSnapIn\bin\Debug\ArtOfNet.RazorTemplateCommand.PSSnapIn.dll"

ECHO add this to your Powershell profile:
ECHO Add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine