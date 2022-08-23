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

Invoke-Expression "${bsp_exe} -game '${gamedir}' '${mapdir}/${map_name}'"
Invoke-Expression "${vis_exe} -game '${gamedir}' '${mapdir}/${map_name}'"
Invoke-Expression "${light_exe} -both -final -StaticPropLighting -StaticPropPolys -game '${gamedir}' '${mapdir}/${map_name}'"

Invoke-Expression "${bzip2_exe} '${mapdir}/${bsp_file}' -f"
