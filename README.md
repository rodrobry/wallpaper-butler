# Wallpaper Randomizer

![Platform](https://img.shields.io/badge/platform-Windows%2010%20%7C%2011-blue)
![License](https://img.shields.io/github/license/rodrobry/wallpaper-manager)

A lightweight C# utility to randomize wallpapers across multi-monitor setups on demand.

## Overview
Unlike background swappers that constantly poll in the background, Wallpaper Randomizer runs strictly when triggered. This generates 2 major benefits:
- **Zero Performance Overhead:** Prevents unexpected resource spikes or lag during heavy workloads and gaming (ideal for lower-end machines).
- **Higher Visual Control:** Change wallpapers only when you want to, cycling until you hit a combination you like.

## Wallpaper Folder Structure
- The app categorizes wallpapers by **topic** subfolders inside a 'BaseFolder'.
- The 'BaseFolder' defaults to 'C:\Users\<Username>\Pictures\wallpapers', but can be customized through the **config.json** file.
- Example:

```text
<BaseFolder>/
├── gta6/
├── vRising/
│   ...
└── categoryN/
```

### Supported Image Formats
- `.jpg` / `.jpeg`
- `.png`