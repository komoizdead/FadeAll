# FadeAll

<p align="center"><img src="assets/pill.png" alt="The FadeAll pill: a slim dark bar with 'Fade all windows', a Size button and a drag grip" width="592"></p>

A tiny Windows pill that fades all your open windows out of sight - and brings them back when you need them.

Click the pill (or press `Ctrl+Alt+H`) and every window fades away. Hover the pill to peek at your screen; click again to bring everything back exactly as it was.

## Features

- **Fade / restore everything** - one click hides all windows, another click restores them.
- **Hover to peek** - while hidden, hovering the pill shows your screen; move the mouse away and it fades out again.
- **Opacity steps** - `PageUp` dims one step (100% down to 0%, then wraps), `Q` steps back up (stops at 100%), `PageDown` jumps straight between fully visible and fully hidden.
- **Ghost one window** - `G` makes just the last-focused window semi-transparent.
- **Stack windows** - `Insert` tiles up to 9 windows in a 3x3 grid; press it again to put every window back exactly where it was.
- **Resize the last window** - the **Size** button cycles 7x7 in -> 3x3 in -> full screen. The grip on the right end of the pill resizes by hand (drag right/down to grow, left/up to shrink).

## Hotkeys

| Key | Action |
| --- | --- |
| Pill click or `Ctrl+Alt+H` | Hide / restore all windows |
| `PageUp` | Opacity down one step (wraps at 0%) |
| `Q` | Opacity up one step (stops at 100%) |
| `PageDown` | Toggle fully visible / fully hidden |
| `G` | Ghost the last-focused window |
| `Insert` | Stack / unstack up to 9 windows in a 3x3 grid |

Right-click the pill for this same list, plus "Exit" and "How it works (full details)".

Key notes:

- `PageUp`, `PageDown`, `Q` and `G` **pass through** - scrolling and typing still work normally; FadeAll just acts alongside them. The flip side: typing a plain `g` also ghosts your last window, and typing a plain `q` also steps the opacity up (it stops at 100%, so a stray `q` cannot hide anything).
- `Insert` is **taken over** while FadeAll runs: overwrite mode never fires. Only the bare key is captured, so `Ctrl+Insert` / `Shift+Insert` (copy / paste) still work in other apps.
- F1-F12 are deliberately left alone.

## Get FadeAll

- **Just run it:** download [`FadeAll/FadeAll.exe`](FadeAll/FadeAll.exe) and double-click it. Nothing to install - it uses the .NET Framework that ships with Windows 10 / 11.
- **Build from source:** run `build.cmd` inside the `FadeAll` folder. It uses the C# compiler that comes with Windows - no Visual Studio or SDK needed.

## How it works

FadeAll uses layered windows: it captures each window's original style and opacity, then animates the alpha with `SetLayeredWindowAttributes`. Hidden windows also become click-through, so you cannot accidentally click something you cannot see. Every original value is restored when you exit.

## Good to know

- **Exit from the pill's right-click menu.** That is what restores your windows to their original state. A force-kill (`taskkill /F`) skips the cleanup and can leave windows faded.
- Some windows are skipped by design: UWP apps, tool windows, and elevated (administrator) windows - unless FadeAll itself is run as administrator.
- The `stacktest/` folder holds the small PowerShell harness used while developing the 3x3 stacking feature. It is not needed to use the app.

## License

MIT - see [LICENSE](LICENSE).
