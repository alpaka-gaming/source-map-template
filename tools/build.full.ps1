param(
     [Parameter()]
     [string]$map
)

if ((Test-Path -path "_variables.private.ps1"))
{
    echo "Using private variables"
    .\_variables.private.ps1
}
else
{
    .\_variables.ps1
}

$path = Resolve-Path "..\src\maps"
$file = "$map.vmf"
$gamedir = Resolve-Path "depots\$steam_app\$game_name"
$bin_path = "depots\$steam_app\bin"
$bsp_exe = Resolve-Path "$bin_path\vbsp.exe"
$vis_exe = Resolve-Path "$bin_path\vvis.exe"
$light_exe = Resolve-Path "$bin_path\vrad.exe"
$bzip2_exe = Resolve-Path "bzip2/bzip2.exe"
$bsp_file = "$map.bsp"

Invoke-Expression "$bsp_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$vis_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$light_exe -final -StaticPropPolys -both -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$bzip2_exe ""$path/$bsp_file"""