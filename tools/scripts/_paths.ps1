$lastTag = git describe --abbrev=0

$vmf = "${map}.vmf"
$map_name = "${map}_${lastTag}"
$vmf_file = "${map_name}.vmf"
$bsp_file = "${map_name}.bsp"

$mapdir = Resolve-Path "../../src/maps"
$depotdir = Resolve-Path "../depots"
$toolsdir = Resolve-Path ".."
$bindir = Resolve-Path "${depotdir}/${steam_app}/bin"
$gamedir = Resolve-Path "${depotdir}/${steam_app}/${game_name}"
$sourcetest = Resolve-Path "${depotdir}/${steam_app}/sourcetest"

Copy-Item "${mapdir}/${vmf}" "${mapdir}/${vmf_file}" -Force

if ($steam_app -eq "243750") #HL2MP
{
    Copy-Item -Path "${toolsdir}/slammintools/mp/*" -Destination "${bindir}" -Force -Recurse
}

$bsp_exe = Resolve-Path "${bindir}/vbsp.exe"
$vis_exe = Resolve-Path "${bindir}/vvis.exe"
$light_exe = Resolve-Path "${bindir}/vrad.exe"
$bspzip_exe = Resolve-Path "${bindir}/bspzip.exe"

$cubemaps_exe = Resolve-Path "${bindir}/../hl2.exe"
$bzip2_exe = Resolve-Path "${toolsdir}/bzip2/bzip2.exe"

# Global Variables

Set-Variable -name "vmf" -value $vmf -Scope Global
Set-Variable -name "map_name" -value $map_name -Scope Global
Set-Variable -name "vmf_file" -value $vmf_file -Scope Global
Set-Variable -name "bsp_file" -value $bsp_file -Scope Global

Set-Variable -name "mapdir" -value $mapdir -Scope Global
Set-Variable -name "depotdir" -value $depotdir -Scope Global
Set-Variable -name "toolsdir" -value $toolsdir -Scope Global
Set-Variable -name "bindir" -value $bindir -Scope Global
Set-Variable -name "gamedir" -value $gamedir -Scope Global
Set-Variable -name "sourcetest" -value $sourcetest -Scope Global


Set-Variable -name "bsp_exe" -value $bsp_exe -Scope Global
Set-Variable -name "vis_exe" -value $vis_exe -Scope Global
Set-Variable -name "light_exe" -value $light_exe -Scope Global
Set-Variable -name "bspzip_exe" -value $bspzip_exe -Scope Global

Set-Variable -name "cubemaps_exe" -value $cubemaps_exe -Scope Global
Set-Variable -name "bzip2_exe" -value $bzip2_exe -Scope Global