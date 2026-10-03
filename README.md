# FadeAll

<p align="center"><img src="assets/pill.png" alt="The FadeAll pill: a slim dark bar with 'Fade all windows', a Size button and a drag grip" width="592"></p>

A tiny Windows pill that fades all your open windows out of sight - and brings them back when you need them.

Click the pill and every window fades away. Hover the pill to peek at your screen; click again to bring everything back exactly as it was. Right-click the pill for a menu with every action.

## Features

- **Fade / restore everything** - one click hides all windows, another click restores them.
- **Hover to peek** - while hidden, hovering the pill shows your screen; move the mouse away and it fades out again.
- **Opacity steps** - dim one step (100% down to 0%, then wraps), step back up (stops at 100%), or jump straight between fully visible and fully hidden.
- **Ghost one window** - make just the last-focused window semi-transparent.
- **Stack windows** - tile up to 9 windows in a 3x3 grid; run it again to put every window back exactly where it was.
- **Resize the last window** - the **Size** button cycles 7x7 in -> 3x3 in -> full screen. The grip on the right end of the pill resizes by hand (drag right/down to grow, left/up to shrink).

## The menu

Right-click the pill and every action is one click away:

- **Fade all windows / Restore all windows** - the same as clicking the pill.
- **Cycle window size** - the same as the Size button: 7x7 in -> 3x3 in -> full screen.
- **Step opacity down** - 100% -> 75% -> 50% -> 25% -> hidden, then wraps.
- **Step opacity up** - hidden -> 25% -> 50% -> 75% -> 100% (stops there).
- **Hide / show all** - jump straight between fully visible and fully hidden.
- **Unfade all windows** - every window straight back to full opacity, whatever the current level.
- **Ghost last window** - make the last-focused window semi-transparent; run it again to undo.
- **Stack windows in a 3x3 grid** - tile up to 9 windows; run it again to unstack.
- **How it works (full details)** - the full in-app help.
- **Exit** - restores everything and quits.

The menu also shows the other controls: hover the pill to peek while windows are hidden, drag it to move it, and use the grip on the right end to size it by hand.

## Get FadeAll

- **Just run it:** download [`FadeAll/FadeAll.exe`](FadeAll/FadeAll.exe) and double-click it. Nothing to install - it uses the .NET Framework that ships with Windows 10 / 11.
- **Build from source:** run `build.cmd` inside the `FadeAll` folder. It uses the C# compiler that comes with Windows - no Visual Studio or SDK needed.

## How it works

FadeAll uses layered windows: it captures each window's original style and opacity, then animates the alpha with `SetLayeredWindowAttributes`. Hidden windows also become click-through, so you cannot accidentally click something you cannot see. Every original value is restored when you exit.

## Good to know

- **No keyboard hooks at all.** FadeAll never touches your keyboard - every action is a click on the pill or its menu, so typing and every other app's shortcuts are untouched.
- **Exit from the pill's right-click menu.** That is what restores your windows to their original state. A force-kill (`taskkill /F`) skips the cleanup and can leave windows faded.
- Some windows are skipped by design: UWP apps, tool windows, and elevated (administrator) windows - unless FadeAll itself is run as administrator.
- The `stacktest/` folder holds the small PowerShell harness used while developing the 3x3 stacking feature. It is not needed to use the app.

## License

MIT - see [LICENSE](LICENSE).
