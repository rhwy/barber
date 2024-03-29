#Test with passing a powershell object serialized as json (default binding)

clear
$template = "
@{
    int childrenCount = 0;
    if(Model.Family.Children is IEnumerable<string>)
    {
        childrenCount = Model.Family.Children.Count;
    } else {
        childrenCount = 1;
        Model.Family.Children = new List<string>(){Model.Family.Children};
    }
}

=====================
 Hello @Model.User.Name
=====================
it is @DateTime.Now

You have @childrenCount Children :
@foreach(var child in @Model.Family.Children)
{
   <text> -</text> @child <text></text>
}
"

$model = "
[User]
Name=Rui

[Family]
Children=Thais
Children=Leandre

"
#add-PSSnapIn ArtOfNet.Powershell.RazorTemplateEngine 
#-ErrorAction SilentlyContinue

$result = barber -m $model -tc $template -t "Properties"

Write-Host "__________________________________________________"
Write-Host ""
Write-Host "Template transformation result  "
Write-Host "__________________________________________________"
Write-Host $result -fore green

