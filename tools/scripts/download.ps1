param(
     [Parameter()]
     [string]$mode = "depotdownloader"
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

if (!(Test-Path -path "../bzip2.zip"))
{
    Invoke-WebRequest https://github.com/philr/bzip2-windows/releases/download/v1.0.8.0/bzip2-1.0.8.0-win-x64.zip -O ../bzip2.zip
    Expand-Archive ../bzip2.zip ../bzip2 -Force
}

if ($mode -eq "steamcmd")
{
    if (!(Test-Path -path "../steamcmd.zip"))
    {
        Invoke-WebRequest https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip -O ../steamcmd.zip
        Expand-Archive ../steamcmd.zip ../steamcmd -Force
    }
    Invoke-Expression "../steamcmd/steamcmd.exe +force_install_dir ../depots/$steam_app +login $steam_username $steam_password +app_update $steam_app validate +quit"
}
if ($mode -eq "depotdownloader")
{
    if (!(Test-Path -path "../depotdownloader.zip"))
    {
        Invoke-WebRequest https://github.com/SteamRE/DepotDownloader/releases/latest/download/depotdownloader-2.4.6.zip -O ../depotdownloader.zip
        Expand-Archive ../depotdownloader.zip ../depotdownloader -Force
    }
    dotnet ../depotdownloader/DepotDownloader.dll -username $steam_username -password $steam_password -app $steam_app -dir ../depots/$steam_app
}

if ((Test-Path -path "../slammintools.zip"))
{
    Expand-Archive ../slammintools.zip ../slammintools -Force
}