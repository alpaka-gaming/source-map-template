![logo](res/source_engine_logo.svg)

# Source Engine Map Repository

[![Fast](https://github.com/alpaka-gaming/source-map-template/actions/workflows/fast.yml/badge.svg?branch=develop)](https://github.com/alpaka-gaming/source-map-template/actions/workflows/fast.yml) [![Normal](https://github.com/alpaka-gaming/source-map-template/actions/workflows/normal.yml/badge.svg?branch=testing)](https://github.com/alpaka-gaming/source-map-template/actions/workflows/normal.yml) [![Publish](https://github.com/alpaka-gaming/source-map-template/actions/workflows/publish.yml/badge.svg?branch=master)](https://github.com/alpaka-gaming/source-map-template/actions/workflows/publish.yml)

---------------------------------------

See the [changelog](CHANGELOG.md) for changes.

---------------------------------------

## Contents ##
- [Source Engine Map Repository](#source-engine-map-repository)
  - [Contents](#contents)
    - [Build](#build)
      - [Process](#process)
      - [Stages](#stages)
    - [Implementing](#implementing)
    - [License](#license)

---------------------------------------

### Build

#### Process

- [Step 1: VBSP](https://developer.valvesoftware.com/wiki/VBSP)
- [Step 2: VVIS](https://developer.valvesoftware.com/wiki/VVIS)
- [Step 3: VRAD](https://developer.valvesoftware.com/wiki/VRAD)
- [Step 4: CUBEMAPS](https://developer.valvesoftware.com/wiki/Cubemaps#Building_cubemaps)
- [Step 5: PACK](https://developer.valvesoftware.com/wiki/VPK)

#### Stages

- Fast ("Alpha" or 'almost-playable')

    > This will trigger by adding `[a**]` version tag in the repo, this will run a fast build with develop branch and create a draft release.
    - Runs VVIS with args: `[fast -nosort]`
    - Runs VRAD with args: `[fast -bounce 2]`

- Normal ("Beta" or 'barely-playable')

    > This will trigger by adding `[b**]` version tag in the repo, this will run a fast build with develop branch and create a preliminar release
    - Runs VVIS with args: `[fast -nosort]`

- Publish ("Released" or a 'perfectly-viable')
  
    > This will trigger by adding `[v**]` version tag in the repo, this will run a fast build with master branch and create a release
    - Runs VRAD with args: `[-final -StaticPropPolys -both]`
    - ~~Runs CUBEMAPS with args: `[+mat_specular 0 +mat_hdr_level 2]`~~

### Implementing

For initial implementation you must edit the file [.env](.env) and set acording to the game/map these variables:
```
STEAM_SDK_APPID=243750
GAME_NAME=hl2mp
MAP_NAME=sdk_background
```

`STEAM_SDK_APPID`
- Source SDK Base 2013 Multiplayer
> https://steamdb.info/app/243750/
- Source SDK Base 2013 Singleplayer
> https://steamdb.info/app/243730/

`GAME_NAME`
- HL2MP
- CSTRIKE
- CSGO

`MAP_NAME`
- Must be the exact name as the `.vmf` file in the [src/maps](src/maps/) folder

Aditionally some secret variables must been defined under the `Settings > Secrets` in GitHub

- STEAM_USERNAME
- STEAM_PASSWORD

> **NOTE:** The Steam Account must be free of Steam Guard (2FA) to allow the workflows to download Source SDK Tools. 
> More information about how to turn this off at 
> (https://help.steampowered.com/faqs/view/7EFD-3CAE-64D3-1C31#disable)


### License

Released under [MIT License](LICENSE)