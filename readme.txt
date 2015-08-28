HotKey79 © Jakub Trmota, 2015 (http://forrest79.net)


Simple executor via keyboard shortcuts for Microsoft Windows.


HOW TO USE:
===========
First, you have to create configuration file. Default is "hotkey79.conf" or you can create one with another name and run "hotkey79.exe" with first parameter path to this file.

Configuration file definition is simple. Every line starts with # is comment and is ignored. Every line is one command definition. Structure is simple:

modifiers + key : command

Where modifiers can be "alt", "ctrl", "win" or "shift", all comment with "+" char. Key is one character like "a" or "r" or some key like "F11". You have to define both modifiers and key. And command is anything to run in Windows.

Example:

# run command line on Ctrl + Win + F12
Ctrl + Win + F12 : cmd.exe

Then run "hotkey79.exe". This utility has no icon on control form. If you want to exit this, you have to use some task manager and manually kill the "hotkey79.exe" process. You have to do this also if you want to reload configuration.

HISTORY
=======
1.0.0 [2015-08-28] - First public version


REQUIREMENTS
============
You need .NET Framework 2 to run this application (https://www.microsoft.com/cs-cz/download/details.aspx?id=1639).


LICENSE
=======
HotKey79 is distributed under BSD license. See license.txt.


https://github.com/forrest79/hotkey79
