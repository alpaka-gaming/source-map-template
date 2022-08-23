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

.\_paths.ps1

Invoke-Expression "$bsp_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$vis_exe -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$light_exe -final -StaticPropPolys -both -game ""$gamedir"" ""$path\$file"""
Invoke-Expression "$bzip2_exe ""$path/$bsp_file"""