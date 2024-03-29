
clear
$template = "
=====================
 Hello @Model.Name
=====================
it is @DateTime.Now
Your are logged as @Model.User
Here is the files on the current folder:
@foreach(var item in @Model.List)
{
   <text>-</text> @item <text></text>
   
}
"
$list = New-Object System.Collections.ArrayList
$files = get-childItem $home | %{ $list += $_.name}
$model = @{ 
    Name = 'rui'
    List = @($list)
	User = $user
}
#add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine 
#-ErrorAction SilentlyContinue
$json = RtcJson -value $model -quotes
$json
#RtcCmd -Info
$toto = barber -m $model -tc $template -t "Json"  -o "e:\test.txt"

Write-Host "__________________________________________________"
Write-Host ""
Write-Host "Template transformation result  "
Write-Host "__________________________________________________"
Write-Host $toto -fore green

edit "e:\test.txt"