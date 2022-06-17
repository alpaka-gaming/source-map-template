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
$sourcetest = Resolve-Path "depots\$steam_app\sourcetest"
$bin_path = "depots\$steam_app\bin"
$bsp_exe = Resolve-Path "$bin_path\vbsp.exe"
$vis_exe = Resolve-Path "$bin_path\vvis.exe"
$light_exe = Resolve-Path "$bin_path\vrad.exe"
$cubemaps_exe = Resolve-Path "$bin_path\..\hl2.exe"
$bzip2_exe = Resolve-Path "bzip2/bzip2.exe"
$bsp_file = "$map.bsp"

Invoke-Expression "$bsp_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$vis_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$light_exe -final -StaticPropPolys -both -game ""$gamedir"" ""$path\$file"""
if (!(Test-Path -path "$gamedir\maps")) {New-Item "$gamedir\maps" -Type Directory}
Copy-Item "$path\$map.bsp" -Destination "$gamedir\maps" -Force
if (!(Test-Path -path "$gamedir\bin")) {New-Item "$gamedir\bin" -Type Directory}
Copy-Item "$sourcetest\bin\*.*" -Destination "$gamedir\bin" -Force
Invoke-Expression "$cubemaps_exe -steam -game $game_name -windowed -novid -nosound +mat_specular 0 +mat_hdr_level 2 +map $map -buildcubemaps"
Invoke-Expression "$bzip2_exe ""$path/$bsp_file"""