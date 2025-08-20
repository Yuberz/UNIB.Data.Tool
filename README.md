# UNIB.Data.Tool 
Tool for extract data archives from game - [UNDER NIGHT IN-BIRTH II Sys:Celes](https://store.steampowered.com/app/2076010/UNDER_NIGHT_INBIRTH_II_SysCeles/)

This fork adds Linux support.

## Build and run (Arch)

1. Install build dependencies: ```sudo pacman -S mono msbuild```
2. Clone this repo: ```git clone https://github.com/Yuberz/UNIB.Data.Tool.git```
3. Navigate to UNIB.Unpacker.sln and run ```msbuild```
4. Copy UNIB.Unpacker/bin/Debug/Unib.Unpacker.exe to your game directory (next to the d folder)
5. run ```mono UNIB.Unpacker.exe d output```
6. Your extracted files should now be in the output folder!
