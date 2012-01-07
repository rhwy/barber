
# Command Line

RTC is available from command line from everywhere (Addded to your PATH).
Sample usage: 
With perperties content:
	
	 rtc render -tc "hello @Model.Prop.Name" -mc "[Prop]\r\nName=Rui" -t "properties"

With json file and save to result.txt file:
	
	rtc render -tc "hello @Model.Name" -mf "user.js" -t "json" -r "./result.txt"
                         

#Powershell

You can also run it from Powershell, dont forget to register the snapin in your $profile or everytime you start a console and need it:
Add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine
Here is a sample script mixing Powershell objects rendered with a Razor template:

	clear
	Add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine

	$template = @"
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
	"@
	$list = New-Object System.Collections.ArrayList
	$files = get-childItem $home | %{ $list += $_.name}
	$model = @{ 
	    Name = 'rui'
	    List = @($list)
	    User = $user
	}

	$result = barber -m $model -tc $template -t "json"

	Write-Host "Template tranformation result:"
	Write-Host $result -fore green

