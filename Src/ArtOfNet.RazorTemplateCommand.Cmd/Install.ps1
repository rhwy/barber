$log = "[$(get-date)]`r`n"

Write-Host "=============================================================" -fore cyan
Write-Host " Installing Razor Template Command (RTC)"  -fore cyan
Write-Host "============================================================="  -fore cyan

# Get Current Path
$currentPath =  $myinvocation.mycommand.path | split-path -parent
Write-Host "Working Path is : $currentPath" -fore white
$log += "Working Path is : $currentPath`r`n"

if(($env:path).Contains("$currentPath"))
{
    Write-Host "Your path already contains the RTC path"  -fore yellow  
    $log += "already in Path`r`n"
}
else
{
    Write-Host "Adding RTC Working folder to your Path" 
    set-item -path env:path -value ($env:path + "$currentPath")
    $log += "added to Path`r`n"
}

$tag = "$currentPath\Install_$([System.DateTime]::Now.ToString("yyyyMMdd")).log"
if(Test-Path $tag)
{
    del $tag
} 
New-Item $tag -Type File
Set-Content $tag $log
