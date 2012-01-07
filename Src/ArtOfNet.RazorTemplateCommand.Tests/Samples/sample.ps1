param(
	$type = "json"
)
$currentPath =  $myinvocation.mycommand.path | split-path -parent
if($type -eq "json")
{
	rtc render -tf "$currentPath\Sample.cshtml" -mf "$currentPath\Sample.json" -t  "json" -r "$currentPath\result.txt"
}

if($type -eq "xml")
{
	rtc render -tf "$currentPath\Sample.cshtml" -mf "$currentPath\Sample.xml" -t  "xml" -r "$currentPath\result.txt"
}

if($type -eq "ini")
{
	rtc render -tf "$currentPath\Sample.cshtml" -mf "$currentPath\Sample.ini" -t  "properties" -r "$currentPath\result.txt"
}