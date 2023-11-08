# HotKey79

Simple keyboard shortcuts executor for Microsoft Windows.


## HOW TO USE:

First, you have to create configuration file. Default is `hotkey79.conf` or you can create one with another name and run `hotkey79.exe` with first parameter path to this file.

Configuration file definition is simple. Every line started with `#` is a comment and is ignored. Every other line is one command definition. Structure is simple:

```
modifiers + key : command
```

Where modifiers can be `Alt`, `Ctrl`, `Win` or `Shift`, all connected with `+` char. Key is one character like `a` or `r` or some key like `F11`. You have to define both modifiers and key. And command is anything to run in Windows.

Example:

```
# run command line on Ctrl + Win + F12
Ctrl + Win + F12 : cmd.exe
```

After creating configation file, run `hotkey79.exe`. This utility has no icon at taskbar. If you want to exit this utility, you have to use some task manager and manually kill the `hotkey79.exe` process. You have to do this also if you want to reload configuration.

## HISTORY

- 2.0.0 [2023-11-08] - Move to x64 binary, require .NET Framework 4.8
- 1.0.0 [2015-09-09] - First public version


## REQUIREMENTS

You need .NET Framework 4.8 to run this application (https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48).


## LICENSE

HotKey79 is distributed under New BSD license. See license.md.


https://github.com/forrest79/hotkey79
