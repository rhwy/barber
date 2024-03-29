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
   <text>- </text> @item <text></text>
    
}
"

$list = New-Object System.Collections.ArrayList
$files = get-childItem $home | %{ $list += $_.name}

$model = @{ 
    Name = 'rui'
    List = @($list)
	User = $user
}
Add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine -ErrorAction SilentlyContinue
$json = RtcJson -value $model

rtc $template  $json
