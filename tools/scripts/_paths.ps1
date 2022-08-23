$lastTag = git describe --abbrev=0

$vmf = "${map}.vmf"
$vmf_file = "${map}_${lastTag}.vmf"
$bsp_file = "${map}_${lastTag}.bsp"

$mapdir = Resolve-Path "../../src/maps"
$depotdir = Resolve-Path "../depots"
$toolsdir = Resolve-Path ".."
$bindir = Resolve-Path "${depotdir}/${steam_app}/bin"
$gamedir = Resolve-Path "${depotdir}/${steam_app}/${game_name}"
$sourcetest = Resolve-Path "${depotdir}/${steam_app}/sourcetest"

if ((Test-Path -path "${mapdir}/${vmf_file}"))
{
    Remove-Item "${mapdir}/${vmf_file}"
}
Copy-Item "${mapdir}/${vmf}" "${mapdir}/${vmf_file}"

$bsp_exe = Resolve-Path "${bindir}/vbsp.exe"
$vis_exe = Resolve-Path "${bindir}/vvis.exe"
$light_exe = Resolve-Path "${bindir}/vrad.exe"
$cubemaps_exe = Resolve-Path "${bindir}/../hl2.exe"
$bzip2_exe = Resolve-Path "${toolsdir}/bzip2/bzip2.exe"