param(
     [Parameter()]
     [string]$map
)

if ((Test-Path -path "_variables.private.ps1"))
{
    Write-Output "Using private variables"
    .\_variables.private.ps1
}
else
{
    .\_variables.ps1
}

.\_paths.ps1

Invoke-Expression "${bsp_exe} -game '${gamedir}' '${mapdir}/${map_name}'"
Invoke-Expression "${vis_exe} -game '${gamedir}' '${mapdir}/${map_name}'"
Invoke-Expression "${light_exe} -both -final -game '${gamedir}' '${mapdir}/${map_name}'"

if (!(Test-Path -path "${gamedir}/maps")) {New-Item "${gamedir}/maps" -Type Directory}
Copy-Item "${mapdir}/${bsp_file}" -Destination "${gamedir}/maps" -Force
if (!(Test-Path -path "${gamedir}/bin")) {New-Item "${gamedir}/bin" -Type Directory}
Copy-Item "${sourcetest}/bin/*.*" -Destination "${gamedir}/bin" -Force
Invoke-Expression "${cubemaps_exe} -steam -game ${game_name} -windowed -novid -nosound +mat_specular 0 +mat_hdr_level 2 +map ${map} -buildcubemaps"

#TODO: Pack
Invoke-Expression "${bspzip_exe} -dir '${gamedir}/maps/${bsp_file}' > '${mapdir}/${map_name}.lst'"
Get-Content "${mapdir}/${map_name}.lst" | Select -Skip 3 | Set-Content "${mapdir}/${map_name}.tmp"
Move-Item "${mapdir}/${map_name}.tmp" "${mapdir}/${map_name}.lst" -Force
#Invoke-Expression "${bspzip_exe} -game ${gamedir} -addlist '${mapdir}/${bsp_file}' '${mapdir}/${map_name}.lst' '${mapdir}/${map_name}.new'"
#Copy-Item "${gamedir}/maps/${bsp_file}" -Destination "${mapdir}" -Force

Invoke-Expression "${bzip2_exe} '${mapdir}/${bsp_file}' -f -k"