# Wallpaper Butler

![Platform](https://img.shields.io/badge/platform-Windows%2010%20%7C%2011-blue)
![License](https://img.shields.io/github/license/rodrobry/wallpaper-butler)

A lightweight C# utility to swap wallpapers on demand.
Seamlessly handles multi-monitor setups with different orientations (horizontal/vertical).
Allows selection from different subfolders (categories/topic) and packs a bit of a personality.

## Overview
Unlike most background swappers that constantly poll in the background, **Wallpaper Butler** acts strictly when "called". This generates 2 major benefits:
- **Zero Performance Overhead:** Prevents unexpected resource spikes or lag during heavy workloads and gaming (ideal for lower-end machines).
- **Higher Visual Control:** Change wallpapers only when you want to, cycling until you hit a combination you like.

## Wallpaper Folder Structure
- The app expects wallpapers in **category/topic** subfolders inside a 'BaseFolder'.
- The 'BaseFolder' defaults to 'C:\Users\<Username>\Pictures\wallpapers', but can be customized through the **config.json** file, or through the CLI.
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