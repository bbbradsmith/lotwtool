Legacy of the Wizard Tool

Version 2 beta
2020
Brad Smith

https://github.com/bbbradsmith/lotwtool
http://rainwarrior.ca


This is an editor for Legacy of the Wizard (NES) and Dragon Slayer IV (Famicom).
It will allow full editing of as much of the game's content as I can provide.

Download:
https://github.com/bbbradsmith/lotwtool/releases


Tips
----

You can Ctrl + Alt + Left Click to quickly test the room you're editing.
The tile you clicked on will become the entry point of the dungeon,
instead of map 0,0. This will save the ROM with a starting location patch
and open it in your program associated with .NES files.

The editor will automatically undo the patch in memory, but if the file
is accidentally left unsaved after a test, the patch may persist.
This can be removed in the Global menu.

I recommend also using the Global menu to give yourself all items,
and a supply of keys as well, and temporarily editing the home map 3,16 to put
an extra ladder next to the door of the Drasle residence to enter more quickly.


The Inn in map 1,16 will look strange in the editor. To make changes to it,
use the Map Properties tool to temporarily set its Metatile Page to $1F,
instead of $0D, and its correct tiles will appear. (The game has a special
override that does this just for the inn.)


The Dragon is stored like another room of the map, but its images are
put together with Metatile Page definitions. Open the Dragon map, and use
the Map Properties to select pages $10 through $17 to view and edit the
Dragon's graphics.


CHR can be edited by clicking a tile in the CHR view, but it may be easier
to edit pixel art like this in a graphics editor of your choice. Right
click on the CHR page in the CHR view for an option to export/import as PNG.


The title screen editor is functional, but simple. In its action menu,
you can export a NAM file that is a standard NES nametable format.
If you export the CHR pages as well, you can use Shiru's "NES Screen Tool"
to edit instead, which has a few more features:

https://shiru.untergrund.net/software.shtml


The "Unused Screen" seems to be an early version of the title screen
that was accidentally left in the ROM, in the same format as the actual
title screen. Unfortunately its CHR tiles seem to have been lost, but
access is provided in this editor for the curious.


Requirements
------------

.NET 4 framework
Windows XP SP3, Vista, 7, 8, 10

The .NET runtime can be downloaded here:
https://www.microsoft.com/en-ca/download/details.aspx?id=17718

This project is open source, and might be compatible with other .NET frameworks
like Mono, which may allow use on other platforms for users that can compile
it from source.

Unstable preview builds:
https://ci.appveyor.com/project/bbbradsmith/lotwtool/branch/master/artifacts


Acknowledgements
----------------

YY - DS4v
https://www.romhacking.net/utilities/170/
An old Legacy of the Wizard editor for Windows 95.

NetBrian - Leghack
https://www.romhacking.net/documents/86/
A few helpful notes about the map format that made reverse engineering quicker.

Binta - Item and Stats Hacking Guide
https://gamefaqs.gamespot.com/nes/587404-legacy-of-the-wizard/faqs/26604
Some notes about the game's memory usage that helped me figure out cheats.

LSD4 - Unofficial archive of Legacy of the Wizard / Dragon Slayer 4
http://lsd4.starfree.jp/


Changes
-------

1. 2020-04-10
- First release.

2 beta. 2020-04-12
- Adjusted selection colours.
- Every room has exactly 9 monsters. Deleting one is impossible. Removed item create/delete features, replaced "delete" with "default".
- Enemy byte 7 determined to be an animation style control.
- Inn cost can be edited.


License
-------

This program was written by Brad Smith.
It is made freely available under the the terms of the Creative Commons Attribution license:
https://creativecommons.org/licenses/by/4.0/

Source code is available at GitHub:
https://github.com/bbbradsmith/binxelview

Support this project on Patreon:
https://www.patreon.com/rainwarrior

Author's website:
http://rainwarrior.ca
