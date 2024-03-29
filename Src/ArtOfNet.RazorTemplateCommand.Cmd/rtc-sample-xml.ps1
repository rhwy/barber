#Test with passing a powershell object serialized as json (default binding)

clear
$template = "
=====================
 Hello @Model.User.Name
=====================
it is @DateTime.Now


"

$model = "
<User Name=""rui"" />
"
#add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine 
#-ErrorAction SilentlyContinue

$result = barber -m $model -tc $template -t "Xml"

Write-Host "__________________________________________________"
Write-Host ""
Write-Host "Template transformation result  "
Write-Host "__________________________________________________"
Write-Host $result -fore green

