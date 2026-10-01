using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinTimer = System.Windows.Forms.Timer;

namespace FadeAll
{
    internal static class Native
    {
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int index, int value);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int index, IntPtr value);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint colorKey, byte alpha, uint flags);

        [DllImport("user32.dll")]
        public static extern bool GetLayeredWindowAttributes(IntPtr hWnd, out uint colorKey, out byte alpha, out uint flags);

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hWnd, int attribute, out int value, int size);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint virtualKey);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public delegate IntPtr LowLevelKeyboardProc(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr module, uint threadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int virtualKey);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int command);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr insertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint flags);

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr monitor, ref MONITORINFO info);

        [DllImport("user32.dll")]
        public static extern uint GetDpiForWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern void SetLastError(uint errorCode);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr extraInfo;
        }

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_LAYERED = 0x00080000;
        public const uint LWA_COLORKEY = 0x00000001u;
        public const uint LWA_ALPHA = 0x00000002u;
        public const int DWMWA_CLOAKED = 14;
        public const int WM_HOTKEY = 0x0312;
        public const int SW_MAXIMIZE = 3;
        public const int SW_RESTORE = 9;
        public const uint SWP_NOZORDER = 0x0004u;
        public const uint SWP_NOACTIVATE = 0x0010u;
        public const uint SWP_NOSENDCHANGING = 0x0400u;
        public const uint MONITOR_DEFAULTTONEAREST = 0x00000002u;
        public const uint MOD_ALT = 0x0001u;
        public const uint MOD_CONTROL = 0x0002u;
        public const uint MOD_NOREPEAT = 0x4000u;
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const uint LLKHF_INJECTED = 0x00000010u;
        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_MENU = 0x12;
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_G = 0x47;
        public const int VK_INSERT = 0x2D;
        public const int VK_Q = 0x51;

        public static int GetExStyle(IntPtr hWnd)
        {
            if (IntPtr.Size == 8)
                return unchecked((int)GetWindowLongPtr64(hWnd, GWL_EXSTYLE).ToInt64());
            return GetWindowLong32(hWnd, GWL_EXSTYLE);
        }

        public static bool TrySetExStyle(IntPtr hWnd, int value)
        {
            SetLastError(0);
            if (IntPtr.Size == 8)
                SetWindowLongPtr64(hWnd, GWL_EXSTYLE, new IntPtr(value));
            else
                SetWindowLong32(hWnd, GWL_EXSTYLE, value);
            return Marshal.GetLastWin32Error() == 0;
        }

        public static string GetWindowClass(IntPtr hWnd)
        {
            var buffer = new StringBuilder(256);
            GetClassName(hWnd, buffer, buffer.Capacity);
            return buffer.ToString();
        }
    }

    internal sealed class FadeSession
    {
        private sealed class WindowState
        {
            public IntPtr Hwnd;
            public int OriginalExStyle;
            public bool HadLayered;
            public uint OriginalKey;
            public byte OriginalAlpha;
            public uint OriginalFlags;
            public int RestoreAlpha;
            public int CurrentAlpha = 255;
            public int LastExStyle = -1;
            public int LastAlpha = -1;
            public bool Failed;
        }

        private const int FadeStep = 24;
        private const int SweepTicks = 30;

        private static readonly int[] Levels = new int[] { 100, 75, 50, 25, 0 };

        public static readonly string[] SkipClasses = new string[]
        {
            "Progman", "WorkerW", "Shell_TrayWnd", "Shell_SecondaryTrayWnd",
            "Windows.UI.Core.CoreWindow", "ApplicationFrameWindow"
        };

        private readonly Dictionary<IntPtr, WindowState> _states = new Dictionary<IntPtr, WindowState>();
        private readonly WinTimer _timer = new WinTimer();
        private readonly Action _onChanged;
        private readonly uint _ownProcessId;
        private int _sweepCounter;

        public bool IsActive { get; private set; }
        public bool IsPeeking { get; private set; }
        public int Level { get; private set; }

        public FadeSession(Action onChanged)
        {
            _onChanged = onChanged;
            using (var self = Process.GetCurrentProcess())
                _ownProcessId = (uint)self.Id;
            _timer.Interval = 16;
            _timer.Tick += delegate { Tick(); };
            Level = 100;
        }

        public void Toggle()
        {
            if (IsActive) End();
            else Begin();
        }

        public void Begin()
        {
            SetLevel(0);
        }

        public void End()
        {
            SetLevel(100);
        }

        public void StepLevel()
        {
            int index = 0;
            for (int i = 0; i < Levels.Length; i++)
            {
                if (Levels[i] == Level)
                {
                    index = i;
                    break;
                }
            }
            SetLevel(Levels[(index + 1) % Levels.Length]);
        }

        public void StepUpLevel()
        {
            int index = 0;
            for (int i = 0; i < Levels.Length; i++)
            {
                if (Levels[i] == Level)
                {
                    index = i;
                    break;
                }
            }
            // Stops at 100% instead of wrapping, so a stray press can never hide everything.
            if (index == 0) return;
            SetLevel(Levels[index - 1]);
        }

        public void JumpLevel()
        {
            if (IsActive && Level == 0) SetLevel(100);
            else SetLevel(0);
        }

        public void SetLevel(int level)
        {
            Level = level;
            if (level < 100)
            {
                if (!IsActive)
                {
                    IsPeeking = false;
                    _sweepCounter = 0;
                    CaptureNewWindows();
                }
                IsActive = true;
            }
            else
            {
                IsActive = false;
                IsPeeking = false;
            }
            _timer.Start();
            _onChanged();
        }

        public void SetPeek(bool peeking)
        {
            if (!IsActive || IsPeeking == peeking) return;
            IsPeeking = peeking;
            _onChanged();
        }

        public void RestoreImmediately()
        {
            _timer.Stop();
            IsActive = false;
            IsPeeking = false;
            foreach (var state in _states.Values)
                RestoreOne(state);
            _states.Clear();
        }

        private int TargetFor(WindowState state)
        {
            if (!IsActive) return state.RestoreAlpha;
            if (IsPeeking) return state.RestoreAlpha;
            return state.RestoreAlpha * Level / 100;
        }

        private void Tick()
        {
            if (IsActive && ++_sweepCounter >= SweepTicks)
            {
                _sweepCounter = 0;
                Sweep();
            }

            var snapshot = new List<WindowState>(_states.Values);
            foreach (var state in snapshot)
            {
                if (state.Failed || !_states.ContainsKey(state.Hwnd)) continue;
                int target = TargetFor(state);
                if (state.CurrentAlpha == target) continue;
                if (state.CurrentAlpha < target)
                    state.CurrentAlpha = Math.Min(target, state.CurrentAlpha + FadeStep);
                else
                    state.CurrentAlpha = Math.Max(target, state.CurrentAlpha - FadeStep);
                Apply(state, state.CurrentAlpha);
            }

            if (!IsActive && AllRestored())
                Cleanup();
        }

        private bool AllRestored()
        {
            foreach (var state in _states.Values)
                if (!state.Failed && state.CurrentAlpha != state.RestoreAlpha) return false;
            return true;
        }

        private void Sweep()
        {
            var gone = new List<IntPtr>();
            foreach (var pair in _states)
            {
                if (!Native.IsWindow(pair.Key)) gone.Add(pair.Key);
                else if (Native.IsIconic(pair.Key)) gone.Add(pair.Key);
            }
            foreach (var hwnd in gone)
            {
                var state = _states[hwnd];
                if (Native.IsWindow(hwnd)) RestoreOne(state);
                _states.Remove(hwnd);
            }
            if (IsActive) CaptureNewWindows();
        }

        private void Cleanup()
        {
            foreach (var state in _states.Values)
                RestoreOne(state);
            _states.Clear();
            _timer.Stop();
            _onChanged();
        }

        private void CaptureNewWindows()
        {
            Native.EnumWindows(CaptureCallback, IntPtr.Zero);
        }

        private bool CaptureCallback(IntPtr hWnd, IntPtr lParam)
        {
            if (ShouldCapture(hWnd)) CaptureWindow(hWnd);
            return true;
        }

        private bool ShouldCapture(IntPtr hWnd)
        {
            if (_states.ContainsKey(hWnd)) return false;
            if (!Native.IsWindowVisible(hWnd) || Native.IsIconic(hWnd)) return false;

            uint pid;
            Native.GetWindowThreadProcessId(hWnd, out pid);
            if (pid == _ownProcessId) return false;

            int exStyle = Native.GetExStyle(hWnd);
            if ((exStyle & Native.WS_EX_TOOLWINDOW) != 0) return false;

            string cls = Native.GetWindowClass(hWnd);
            for (int i = 0; i < SkipClasses.Length; i++)
                if (cls == SkipClasses[i]) return false;

            int cloaked;
            if (Native.DwmGetWindowAttribute(hWnd, Native.DWMWA_CLOAKED, out cloaked, sizeof(int)) == 0 && cloaked != 0)
                return false;

            return Native.GetWindowTextLength(hWnd) > 0;
        }

        private void CaptureWindow(IntPtr hWnd)
        {
            var state = new WindowState();
            state.Hwnd = hWnd;
            state.OriginalExStyle = Native.GetExStyle(hWnd);
            state.HadLayered = (state.OriginalExStyle & Native.WS_EX_LAYERED) != 0;

            if (state.HadLayered)
            {
                uint key;
                byte alpha;
                uint flags;
                if (!Native.GetLayeredWindowAttributes(hWnd, out key, out alpha, out flags))
                    return; // per-pixel layered window; leave it untouched
                state.OriginalKey = key;
                state.OriginalAlpha = alpha;
                state.OriginalFlags = flags;
                state.RestoreAlpha = (flags & Native.LWA_ALPHA) != 0 ? alpha : 255;
            }
            else
            {
                state.OriginalKey = 0;
                state.OriginalAlpha = 255;
                state.OriginalFlags = Native.LWA_ALPHA;
                state.RestoreAlpha = 255;
            }

            state.CurrentAlpha = state.RestoreAlpha;
            _states.Add(hWnd, state);
        }

        private void Apply(WindowState state, int alpha)
        {
            int exStyle = state.OriginalExStyle;
            if (alpha < 255 || state.HadLayered) exStyle |= Native.WS_EX_LAYERED;
            if (alpha == 0) exStyle |= Native.WS_EX_TRANSPARENT;

            if (exStyle != state.LastExStyle)
            {
                if (!Native.TrySetExStyle(state.Hwnd, exStyle))
                {
                    state.Failed = true;
                    return;
                }
                state.LastExStyle = exStyle;
            }

            if (alpha != state.LastAlpha)
            {
                uint flags = Native.LWA_ALPHA;
                uint key = 0;
                if ((state.OriginalFlags & Native.LWA_COLORKEY) != 0)
                {
                    flags |= Native.LWA_COLORKEY;
                    key = state.OriginalKey;
                }

                Native.SetLastError(0);
                if (!Native.SetLayeredWindowAttributes(state.Hwnd, key, (byte)alpha, flags))
                {
                    Native.TrySetExStyle(state.Hwnd, state.OriginalExStyle);
                    state.Failed = true;
                    return;
                }
                state.LastAlpha = alpha;
            }
        }

        private void RestoreOne(WindowState state)
        {
            if (state.Failed) return;
            Native.TrySetExStyle(state.Hwnd, state.OriginalExStyle);
            if (state.HadLayered)
                Native.SetLayeredWindowAttributes(state.Hwnd, state.OriginalKey, state.OriginalAlpha, state.OriginalFlags);
        }
    }

    internal sealed class PillForm : Form
    {
        private const int HotkeyId = 0x0A11;
        private const int GhostAlpha = 128;
        private const int StatusWidth = 232;
        private const int ButtonWidth = 48;
        private const int GripWidth = 16;
        private const int MinSize = 80;
        private const int PillHeight = 40;
        private const int StackGap = 4;

        private static readonly string[] TrackSkipClasses = new string[]
        {
            "Progman", "WorkerW", "Shell_TrayWnd", "Shell_SecondaryTrayWnd"
        };

        private sealed class GhostState
        {
            public int OriginalExStyle;
            public bool HadLayered;
            public uint OriginalKey;
            public byte OriginalAlpha;
            public uint OriginalFlags;
        }

        private sealed class StackSnap
        {
            public Native.RECT Rect;
            public bool WasZoomed;
        }

        private readonly FadeSession _session;
        private readonly ToolTip _tip = new ToolTip();
        private readonly ContextMenuStrip _menu = new ContextMenuStrip();
        private readonly ToolStripMenuItem _toggleItem;
        private readonly WinTimer _tracker = new WinTimer();
        private readonly uint _myProcessId;
        private readonly Dictionary<IntPtr, int> _sizeSteps = new Dictionary<IntPtr, int>();
        private readonly Dictionary<IntPtr, GhostState> _ghosts = new Dictionary<IntPtr, GhostState>();
        private readonly Dictionary<IntPtr, StackSnap> _stacked = new Dictionary<IntPtr, StackSnap>();
        private readonly Native.LowLevelKeyboardProc _keyProc;
        private readonly HashSet<int> _keyDown = new HashSet<int>();
        private readonly HashSet<int> _swallowed = new HashSet<int>();
        private IntPtr _kbHook = IntPtr.Zero;
        private IntPtr _lastWindow = IntPtr.Zero;
        private bool _overButton;
        private bool _overGrip;
        private bool _dragging;
        private bool _moved;
        private bool _hover;
        private bool _sizing;
        private IntPtr _sizeTarget = IntPtr.Zero;
        private Rectangle _sizeStartRect;
        private int _sizeLiveW;
        private int _sizeLiveH;
        private int _sizeDpi = 96;
        private Point _dragCursorStart;
        private Point _dragFormStart;

        public PillForm()
        {
            Text = "FadeAll";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            ShowInTaskbar = false;
            BackColor = Color.FromArgb(32, 32, 38);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            Size = new Size(StatusWidth + ButtonWidth + GripWidth, PillHeight);
            Opacity = 0.93;

            using (var self = Process.GetCurrentProcess())
                _myProcessId = (uint)self.Id;

            _keyProc = KeyboardHookCallback;
            _session = new FadeSession(OnSessionChanged);

            Rectangle work = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(work.Left + (work.Width - Width) / 2, work.Top + 6);

            _tracker.Interval = 150;
            _tracker.Tick += delegate { TrackForeground(); };
            _tracker.Start();

            _toggleItem = new ToolStripMenuItem("Fade all windows (click pill / Ctrl+Alt+H)", null, delegate { Toggle(); });
            var shrinkItem = new ToolStripMenuItem("Cycle window size (Size button): 7in / 3in / full", null, delegate { CycleSizeCurrentWindow(); });
            var stepItem = new ToolStripMenuItem("Step opacity down (PageUp)", null, delegate { _session.StepLevel(); });
            var stepUpItem = new ToolStripMenuItem("Step opacity up (Q)", null, delegate { _session.StepUpLevel(); });
            var jumpItem = new ToolStripMenuItem("Hide / show all (PageDown)", null, delegate { _session.JumpLevel(); });
            var ghostItem = new ToolStripMenuItem("Ghost last window (G)", null, delegate { GhostCurrentWindow(); });
            var stackItem = new ToolStripMenuItem("Stack windows in a 3x3 grid (Insert)", null, delegate { ToggleStack(); });
            var peekInfo = new ToolStripMenuItem("Hover the pill: peek while faded");
            peekInfo.Enabled = false;
            var moveInfo = new ToolStripMenuItem("Drag the pill: move it");
            moveInfo.Enabled = false;
            var gripInfo = new ToolStripMenuItem("Drag the grip: size the last window by hand");
            gripInfo.Enabled = false;
            var helpItem = new ToolStripMenuItem("How it works (full details)", null, delegate { ShowHelp(); });
            var exitItem = new ToolStripMenuItem("Exit", null, delegate { Close(); });
            _menu.Items.Add(_toggleItem);
            _menu.Items.Add(shrinkItem);
            _menu.Items.Add(stepItem);
            _menu.Items.Add(stepUpItem);
            _menu.Items.Add(jumpItem);
            _menu.Items.Add(ghostItem);
            _menu.Items.Add(stackItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(peekInfo);
            _menu.Items.Add(moveInfo);
            _menu.Items.Add(gripInfo);
            _menu.Items.Add(helpItem);
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(exitItem);

            _tip.InitialDelay = 300;
            _tip.SetToolTip(this, "Left: fade / restore all  |  PageUp / Q: step opacity down / up  |  PageDown: hide / show  |  G: ghost last window  |  Insert: stack 3x3  |  Size: 7in / 3in / full  |  Grip: drag to size by hand");
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= Native.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!Native.RegisterHotKey(Handle, HotkeyId,
                Native.MOD_CONTROL | Native.MOD_ALT | Native.MOD_NOREPEAT, (uint)Keys.H))
                _tip.SetToolTip(this, "Ctrl+Alt+H is taken by another app. Click the pill instead. Everything else still works.");

            _kbHook = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, _keyProc, Native.GetModuleHandle(null), 0);
            if (_kbHook == IntPtr.Zero)
                _tip.SetToolTip(this, "The PageUp / PageDown / Q / Insert / G keys could not be watched. Click the pill and use the right-click menu instead.");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Native.UnregisterHotKey(Handle, HotkeyId);
            if (_kbHook != IntPtr.Zero)
            {
                Native.UnhookWindowsHookEx(_kbHook);
                _kbHook = IntPtr.Zero;
            }
            _tracker.Stop();
            RestoreGhosts();
            if (_stacked.Count > 0) UnstackWindows();
            _session.RestoreImmediately();
            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_HOTKEY && m.WParam.ToInt32() == HotkeyId)
            {
                Toggle();
                return;
            }
            base.WndProc(ref m);
        }

        private IntPtr KeyboardHookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                int message = wParam.ToInt32();
                var data = (Native.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.KBDLLHOOKSTRUCT));
                if ((data.flags & Native.LLKHF_INJECTED) == 0)
                {
                    int vk = (int)data.vkCode;
                    if (message == Native.WM_KEYDOWN || message == Native.WM_SYSKEYDOWN)
                    {
                        if ((vk == Native.VK_PRIOR || vk == Native.VK_NEXT || vk == Native.VK_G || vk == Native.VK_Q || vk == Native.VK_INSERT)
                            && !_keyDown.Contains(vk))
                        {
                            _keyDown.Add(vk);
                            if (!ModifierHeld()) HandleBareKey(vk);
                        }
                        if (vk == Native.VK_INSERT && !ModifierHeld())
                        {
                            // Ours while bare: swallowing keeps overwrite mode from ever toggling.
                            _swallowed.Add(vk);
                            return (IntPtr)1;
                        }
                    }
                    else if (message == Native.WM_KEYUP || message == Native.WM_SYSKEYUP)
                    {
                        _keyDown.Remove(vk);
                        if (_swallowed.Remove(vk)) return (IntPtr)1;
                    }
                }
            }
            return Native.CallNextHookEx(_kbHook, code, wParam, lParam);
        }

        private void HandleBareKey(int vk)
        {
            if (vk == Native.VK_PRIOR) _session.StepLevel();
            else if (vk == Native.VK_Q) _session.StepUpLevel();
            else if (vk == Native.VK_NEXT) _session.JumpLevel();
            else if (vk == Native.VK_G) GhostCurrentWindow();
            else if (vk == Native.VK_INSERT) ToggleStack();
        }

        private static bool ModifierHeld()
        {
            return (Native.GetAsyncKeyState(Native.VK_SHIFT) & 0x8000) != 0
                || (Native.GetAsyncKeyState(Native.VK_CONTROL) & 0x8000) != 0
                || (Native.GetAsyncKeyState(Native.VK_MENU) & 0x8000) != 0
                || (Native.GetAsyncKeyState(Native.VK_LWIN) & 0x8000) != 0
                || (Native.GetAsyncKeyState(Native.VK_RWIN) & 0x8000) != 0;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            using (GraphicsPath path = RoundedPath(new Rectangle(0, 0, Width, Height), 12))
                Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color background;
            Color border;
            string text;
            if (_sizing)
            {
                background = Color.FromArgb(45, 55, 75);
                border = Color.FromArgb(140, 160, 200);
                text = "Sizing " + (_sizeLiveW / (double)_sizeDpi).ToString("0.0") + " x " +
                    (_sizeLiveH / (double)_sizeDpi).ToString("0.0") + " in";
            }
            else if (_session.IsActive)
            {
                if (_session.IsPeeking)
                {
                    background = Color.FromArgb(14, 122, 87);
                    border = Color.FromArgb(130, 225, 185);
                    text = "Showing (peek)";
                }
                else if (_session.Level == 0)
                {
                    background = Color.FromArgb(13, 58, 109);
                    border = Color.FromArgb(105, 170, 235);
                    text = "Hidden - hover to peek";
                }
                else
                {
                    background = Color.FromArgb(120, 82, 16);
                    border = Color.FromArgb(228, 178, 92);
                    text = "Dimmed - " + _session.Level + "%";
                }
            }
            else
            {
                background = _hover ? Color.FromArgb(52, 52, 60) : Color.FromArgb(32, 32, 38);
                border = Color.FromArgb(96, 96, 112);
                text = "Fade all windows";
            }

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = RoundedPath(bounds, 12))
            {
                using (var brush = new SolidBrush(background)) g.FillPath(brush, path);
                using (var pen = new Pen(border)) g.DrawPath(pen, path);
            }

            var buttonBounds = new Rectangle(StatusWidth, 0, ButtonWidth, Height);
            var gripBounds = new Rectangle(StatusWidth + ButtonWidth, 0, GripWidth, Height);
            using (GraphicsPath buttonPath = RoundedPath(bounds, 12))
            {
                Region clip = g.Clip;
                g.SetClip(buttonBounds);
                using (var brush = new SolidBrush(Color.FromArgb(_overButton ? 46 : 18, 255, 255, 255)))
                    g.FillPath(brush, buttonPath);
                using (var pen = new Pen(border)) g.DrawPath(pen, buttonPath);
                g.Clip = clip;
                clip.Dispose();
            }
            using (GraphicsPath gripPath = RoundedPath(bounds, 12))
            {
                Region clip = g.Clip;
                g.SetClip(gripBounds);
                using (var brush = new SolidBrush(Color.FromArgb(_overGrip || _sizing ? 46 : 18, 255, 255, 255)))
                    g.FillPath(brush, gripPath);
                using (var pen = new Pen(border)) g.DrawPath(pen, gripPath);
                g.Clip = clip;
                clip.Dispose();
            }
            using (var pen = new Pen(Color.FromArgb(90, 90, 104)))
            {
                g.DrawLine(pen, StatusWidth, 7, StatusWidth, Height - 8);
                g.DrawLine(pen, StatusWidth + ButtonWidth, 7, StatusWidth + ButtonWidth, Height - 8);
            }
            using (var pen = new Pen(Color.FromArgb(160, 160, 176), 2))
            {
                for (int i = 0; i < 3; i++)
                {
                    int gx = gripBounds.Left + 1 + i * 3;
                    int gy = gripBounds.Top + Height / 2 + i * 3 - 6;
                    g.DrawLine(pen, gx, gy + 5, gx + 5, gy);
                }
            }

            var statusBounds = new Rectangle(0, 0, StatusWidth, Height);
            TextRenderer.DrawText(g, text, Font, statusBounds, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, "Size", Font, buttonBounds, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPrefix);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Cursor = Cursors.Hand;
            _session.SetPeek(true);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            _overButton = false;
            _overGrip = false;
            if (!_dragging && !_sizing)
            {
                Cursor = Cursors.Default;
                _session.SetPeek(false);
            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (e.X >= StatusWidth + ButtonWidth)
                {
                    StartSizing();
                    return;
                }
                _dragging = true;
                _moved = false;
                _dragCursorStart = Cursor.Position;
                _dragFormStart = Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_sizing)
            {
                DoSizing();
                return;
            }

            bool overButton = e.X >= StatusWidth && e.X < StatusWidth + ButtonWidth;
            bool overGrip = e.X >= StatusWidth + ButtonWidth;
            if (overButton != _overButton || overGrip != _overGrip)
            {
                _overButton = overButton;
                _overGrip = overGrip;
                Cursor = overGrip ? Cursors.SizeNWSE : Cursors.Hand;
                Invalidate();
            }

            if (!_dragging) return;

            Point now = Cursor.Position;
            int dx = now.X - _dragCursorStart.X;
            int dy = now.Y - _dragCursorStart.Y;
            if (!_moved && Math.Abs(dx) + Math.Abs(dy) > 5) _moved = true;
            if (!_moved) return;

            Rectangle screen = SystemInformation.VirtualScreen;
            var target = new Point(_dragFormStart.X + dx, _dragFormStart.Y + dy);
            target.X = Math.Max(screen.Left, Math.Min(screen.Right - Width, target.X));
            target.Y = Math.Max(screen.Top, Math.Min(screen.Bottom - Height, target.Y));
            Location = target;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (_sizing)
                {
                    _sizing = false;
                    _sizeTarget = IntPtr.Zero;
                }
                else if (_dragging && !_moved)
                {
                    if (e.X >= StatusWidth + ButtonWidth)
                        _tip.Show("Drag this grip to size the last window by hand.", this, StatusWidth + ButtonWidth, Height, 2500);
                    else if (e.X >= StatusWidth) CycleSizeCurrentWindow();
                    else Toggle();
                }
                _dragging = false;
                SyncMouseState();
            }
            else if (e.Button == MouseButtons.Right)
            {
                UpdateToggleText();
                _menu.Show(this, new Point(e.X, e.Y));
            }
        }

        private void StartSizing()
        {
            if (_lastWindow == IntPtr.Zero || !Native.IsWindow(_lastWindow))
            {
                _tip.Show("Click a window first, then drag the grip.", this, StatusWidth + ButtonWidth, Height, 2500);
                return;
            }

            if (Native.IsIconic(_lastWindow)) Native.ShowWindow(_lastWindow, Native.SW_RESTORE);
            if (Native.IsZoomed(_lastWindow)) Native.ShowWindow(_lastWindow, Native.SW_RESTORE);

            Native.RECT rect;
            if (!Native.GetWindowRect(_lastWindow, out rect)) return;

            _sizing = true;
            _sizeTarget = _lastWindow;
            _sizeStartRect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            _sizeLiveW = _sizeStartRect.Width;
            _sizeLiveH = _sizeStartRect.Height;
            uint dpi = Native.GetDpiForWindow(_lastWindow);
            _sizeDpi = dpi == 0 ? 96 : (int)dpi;
            _dragCursorStart = Cursor.Position;
            Cursor = Cursors.SizeNWSE;
            Invalidate();
        }

        private void DoSizing()
        {
            if (_sizeTarget == IntPtr.Zero || !Native.IsWindow(_sizeTarget))
            {
                _sizing = false;
                _sizeTarget = IntPtr.Zero;
                Invalidate();
                return;
            }

            Cursor = Cursors.SizeNWSE;

            Point now = Cursor.Position;
            int dx = now.X - _dragCursorStart.X;
            int dy = now.Y - _dragCursorStart.Y;

            Native.RECT work;
            if (!TryGetWorkArea(_sizeTarget, out work)) return;

            int maxW = work.Right - work.Left;
            int maxH = work.Bottom - work.Top;
            int w = Math.Max(MinSize, Math.Min(_sizeStartRect.Width + dx, maxW));
            int h = Math.Max(MinSize, Math.Min(_sizeStartRect.Height + dy, maxH));
            int x = Math.Max(work.Left, Math.Min(work.Right - w, _sizeStartRect.X));
            int y = Math.Max(work.Top, Math.Min(work.Bottom - h, _sizeStartRect.Y));

            SetSizeExactWH(_sizeTarget, x, y, w, h);

            Native.RECT live;
            if (Native.GetWindowRect(_sizeTarget, out live))
            {
                _sizeLiveW = live.Right - live.Left;
                _sizeLiveH = live.Bottom - live.Top;
            }
            Invalidate();
        }

        private void SyncMouseState()
        {
            Point client = PointToClient(Cursor.Position);
            bool inside = ClientRectangle.Contains(client);
            _hover = inside;
            _overGrip = inside && client.X >= StatusWidth + ButtonWidth;
            _overButton = inside && !_overGrip && client.X >= StatusWidth;
            Cursor = _overGrip ? Cursors.SizeNWSE : (inside ? Cursors.Hand : Cursors.Default);
            if (!inside) _session.SetPeek(false);
            Invalidate();
        }

        private void Toggle()
        {
            _session.Toggle();
            UpdateToggleText();
            Invalidate();
        }

        private void TrackForeground()
        {
            IntPtr hWnd = Native.GetForegroundWindow();
            if (hWnd == IntPtr.Zero || !Native.IsWindow(hWnd)) return;

            uint pid;
            Native.GetWindowThreadProcessId(hWnd, out pid);
            if (pid == _myProcessId) return;

            string cls = Native.GetWindowClass(hWnd);
            for (int i = 0; i < TrackSkipClasses.Length; i++)
                if (cls == TrackSkipClasses[i]) return;

            _lastWindow = hWnd;
        }

        private void GhostCurrentWindow()
        {
            PruneGhosts();

            if (_lastWindow == IntPtr.Zero || !Native.IsWindow(_lastWindow))
            {
                _tip.Show("Click a window first, then press G.", this, StatusWidth, Height, 2500);
                return;
            }

            IntPtr hWnd = _lastWindow;
            GhostState prior;
            if (_ghosts.TryGetValue(hWnd, out prior))
            {
                RestoreGhost(hWnd, prior);
                _ghosts.Remove(hWnd);
                return;
            }

            var ghost = new GhostState();
            ghost.OriginalExStyle = Native.GetExStyle(hWnd);
            ghost.HadLayered = (ghost.OriginalExStyle & Native.WS_EX_LAYERED) != 0;
            ghost.OriginalKey = 0;
            ghost.OriginalAlpha = 255;
            ghost.OriginalFlags = Native.LWA_ALPHA;

            if (ghost.HadLayered)
            {
                uint key;
                byte alpha;
                uint flags;
                if (!Native.GetLayeredWindowAttributes(hWnd, out key, out alpha, out flags))
                {
                    _tip.Show("That window can't be ghosted (it uses per-pixel transparency).", this, StatusWidth, Height, 2500);
                    return;
                }
                ghost.OriginalKey = key;
                ghost.OriginalAlpha = alpha;
                ghost.OriginalFlags = flags;
            }

            if (!Native.TrySetExStyle(hWnd, ghost.OriginalExStyle | Native.WS_EX_LAYERED))
            {
                _tip.Show("That window can't be ghosted (it may run as administrator).", this, StatusWidth, Height, 2500);
                return;
            }

            uint useFlags = Native.LWA_ALPHA;
            uint useKey = 0;
            if ((ghost.OriginalFlags & Native.LWA_COLORKEY) != 0)
            {
                useFlags |= Native.LWA_COLORKEY;
                useKey = ghost.OriginalKey;
            }

            if (!Native.SetLayeredWindowAttributes(hWnd, useKey, (byte)GhostAlpha, useFlags))
            {
                Native.TrySetExStyle(hWnd, ghost.OriginalExStyle);
                _tip.Show("That window can't be ghosted (it may run as administrator).", this, StatusWidth, Height, 2500);
                return;
            }

            _ghosts.Add(hWnd, ghost);
        }

        private void PruneGhosts()
        {
            if (_ghosts.Count == 0) return;
            var dead = new List<IntPtr>();
            foreach (IntPtr key in _ghosts.Keys)
                if (!Native.IsWindow(key)) dead.Add(key);
            for (int i = 0; i < dead.Count; i++)
                _ghosts.Remove(dead[i]);
        }

        private void RestoreGhosts()
        {
            foreach (var pair in _ghosts)
                RestoreGhost(pair.Key, pair.Value);
            _ghosts.Clear();
        }

        private static void RestoreGhost(IntPtr hWnd, GhostState ghost)
        {
            Native.TrySetExStyle(hWnd, ghost.OriginalExStyle);
            if (ghost.HadLayered)
                Native.SetLayeredWindowAttributes(hWnd, ghost.OriginalKey, ghost.OriginalAlpha, ghost.OriginalFlags);
        }

        private void ToggleStack()
        {
            if (_stacked.Count > 0)
            {
                UnstackWindows();
                _tip.Show("Unstacked - windows are back where they were.", this, StatusWidth, Height, 2500);
                return;
            }

            var targets = CollectStackTargets();
            if (targets.Count == 0)
            {
                _tip.Show("No windows to stack.", this, StatusWidth, Height, 2500);
                return;
            }

            Native.RECT work;
            if (!TryGetWorkArea(Handle, out work))
            {
                _tip.Show("Could not measure the screen area.", this, StatusWidth, Height, 2500);
                return;
            }

            int cellW = ((work.Right - work.Left) - StackGap * 4) / 3;
            int cellH = ((work.Bottom - work.Top) - StackGap * 4) / 3;

            int placed = 0;
            for (int i = 0; i < targets.Count && placed < 9; i++)
            {
                IntPtr hWnd = targets[i];
                if (!Native.IsWindow(hWnd)) continue;

                bool wasZoomed = Native.IsZoomed(hWnd);
                if (Native.IsIconic(hWnd)) Native.ShowWindow(hWnd, Native.SW_RESTORE);
                if (wasZoomed) Native.ShowWindow(hWnd, Native.SW_RESTORE);

                Native.RECT before;
                if (!Native.GetWindowRect(hWnd, out before)) continue;

                int x = work.Left + StackGap + (placed % 3) * (cellW + StackGap);
                int y = work.Top + StackGap + (placed / 3) * (cellH + StackGap);
                if (!SetSizeExactWH(hWnd, x, y, cellW, cellH)) continue;

                var snap = new StackSnap();
                snap.Rect = before;
                snap.WasZoomed = wasZoomed;
                _stacked[hWnd] = snap;
                placed++;
            }

            if (placed == 0)
            {
                _tip.Show("No window accepted a new position (they may run as administrator).", this, StatusWidth, Height, 2500);
                return;
            }

            if (targets.Count > 9)
                _tip.Show("Stacked " + placed + " of " + targets.Count + " windows. Press Insert again to unstack.", this, StatusWidth, Height, 3000);
            else
                _tip.Show("Stacked " + placed + " windows. Press Insert again to unstack.", this, StatusWidth, Height, 3000);
        }

        private void UnstackWindows()
        {
            foreach (var pair in _stacked)
            {
                IntPtr hWnd = pair.Key;
                if (!Native.IsWindow(hWnd)) continue;
                if (Native.IsIconic(hWnd)) Native.ShowWindow(hWnd, Native.SW_RESTORE);
                if (pair.Value.WasZoomed)
                {
                    Native.ShowWindow(hWnd, Native.SW_MAXIMIZE);
                }
                else
                {
                    Native.RECT r = pair.Value.Rect;
                    SetSizeExactWH(hWnd, r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
                }
            }
            _stacked.Clear();
        }

        private List<IntPtr> CollectStackTargets()
        {
            var list = new List<IntPtr>();
            Native.EnumWindows(delegate(IntPtr hWnd, IntPtr lParam)
            {
                if (IsStackable(hWnd)) list.Add(hWnd);
                return true;
            }, IntPtr.Zero);
            return list;
        }

        private bool IsStackable(IntPtr hWnd)
        {
            if (!Native.IsWindowVisible(hWnd) || Native.IsIconic(hWnd)) return false;

            uint pid;
            Native.GetWindowThreadProcessId(hWnd, out pid);
            if (pid == _myProcessId) return false;

            int exStyle = Native.GetExStyle(hWnd);
            if ((exStyle & Native.WS_EX_TOOLWINDOW) != 0) return false;

            string cls = Native.GetWindowClass(hWnd);
            for (int i = 0; i < FadeSession.SkipClasses.Length; i++)
                if (cls == FadeSession.SkipClasses[i]) return false;

            int cloaked;
            if (Native.DwmGetWindowAttribute(hWnd, Native.DWMWA_CLOAKED, out cloaked, sizeof(int)) == 0 && cloaked != 0)
                return false;

            return Native.GetWindowTextLength(hWnd) > 0;
        }

        private void CycleSizeCurrentWindow()
        {
            if (_lastWindow == IntPtr.Zero || !Native.IsWindow(_lastWindow))
            {
                _tip.Show("Click a window first, then click Size.", this, StatusWidth, Height, 2500);
                return;
            }

            PruneSizeSteps();

            int step;
            if (!_sizeSteps.TryGetValue(_lastWindow, out step)) step = 0;
            if (ApplySizeStep(_lastWindow, step))
                _sizeSteps[_lastWindow] = (step + 1) % 3;

            Native.SetForegroundWindow(_lastWindow);
        }

        private void PruneSizeSteps()
        {
            if (_sizeSteps.Count == 0) return;
            var dead = new List<IntPtr>();
            foreach (IntPtr key in _sizeSteps.Keys)
                if (!Native.IsWindow(key)) dead.Add(key);
            for (int i = 0; i < dead.Count; i++)
                _sizeSteps.Remove(dead[i]);
        }

        private static bool ApplySizeStep(IntPtr hWnd, int step)
        {
            if (Native.IsIconic(hWnd)) Native.ShowWindow(hWnd, Native.SW_RESTORE);
            if (Native.IsZoomed(hWnd)) Native.ShowWindow(hWnd, Native.SW_RESTORE);

            Native.RECT work;
            if (!TryGetWorkArea(hWnd, out work)) return false;

            if (step == 2)
                return Native.SetWindowPos(hWnd, IntPtr.Zero, work.Left, work.Top,
                    work.Right - work.Left, work.Bottom - work.Top,
                    Native.SWP_NOZORDER | Native.SWP_NOACTIVATE);

            uint dpi = Native.GetDpiForWindow(hWnd);
            if (dpi == 0) dpi = 96;
            int size = (int)Math.Round((step == 0 ? 7.0 : 3.0) * dpi);

            Native.RECT rect;
            if (!Native.GetWindowRect(hWnd, out rect)) return false;

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            int x = rect.Left + (width - size) / 2;
            int y = rect.Top + (height - size) / 2;
            x = Math.Max(work.Left, Math.Min(work.Right - size, x));
            y = Math.Max(work.Top, Math.Min(work.Bottom - size, y));

            return SetSizeExact(hWnd, x, y, size);
        }

        // Apps clamp a plain resize to their own minimum size. Retrying with
        // SWP_NOSENDCHANGING denies them that chance, so the size sticks.
        private static bool SetSizeExact(IntPtr hWnd, int x, int y, int size)
        {
            return SetSizeExactWH(hWnd, x, y, size, size);
        }

        private static bool SetSizeExactWH(IntPtr hWnd, int x, int y, int w, int h)
        {
            if (Native.SetWindowPos(hWnd, IntPtr.Zero, x, y, w, h,
                Native.SWP_NOZORDER | Native.SWP_NOACTIVATE)
                && ReachedSizeWH(hWnd, w, h))
                return true;

            return Native.SetWindowPos(hWnd, IntPtr.Zero, x, y, w, h,
                Native.SWP_NOZORDER | Native.SWP_NOACTIVATE | Native.SWP_NOSENDCHANGING);
        }

        private static bool ReachedSizeWH(IntPtr hWnd, int w, int h)
        {
            Native.RECT rect;
            if (!Native.GetWindowRect(hWnd, out rect)) return false;
            return Math.Abs((rect.Right - rect.Left) - w) <= 8
                && Math.Abs((rect.Bottom - rect.Top) - h) <= 8;
        }

        private static bool TryGetWorkArea(IntPtr hWnd, out Native.RECT work)
        {
            work = new Native.RECT();
            IntPtr monitor = Native.MonitorFromWindow(hWnd, Native.MONITOR_DEFAULTTONEAREST);
            if (monitor == IntPtr.Zero) return false;
            var info = new Native.MONITORINFO();
            info.cbSize = Marshal.SizeOf(typeof(Native.MONITORINFO));
            if (!Native.GetMonitorInfo(monitor, ref info)) return false;
            work = info.rcWork;
            return true;
        }

        private void OnSessionChanged()
        {
            UpdateToggleText();
            Invalidate();
        }

        private void UpdateToggleText()
        {
            _toggleItem.Text = (_session.IsActive ? "Restore all windows" : "Fade all windows")
                + " (click pill / Ctrl+Alt+H)";
        }

        private void ShowHelp()
        {
            string text =
                "Click the left part of the pill to fade every open window to invisible.\r\n\r\n" +
                "Keyboard shortcuts (active while FadeAll runs, in every app):\r\n" +
                "  -  PageUp: step down one level - 100% to 75% to 50% to 25% to hidden, then back to 100%.\r\n" +
                "  -  PageDown: jump straight between full (100%) and hidden.\r\n" +
                "  -  Q: step up one level - hidden to 25% to 50% to 75% to 100% - and stops at full.\r\n" +
                "  -  G: ghost the window you last used - it becomes see-through so you can read what is behind it. Press G again to undo.\r\n" +
                "  -  Insert: stack your open windows into a 3 x 3 grid - up to 9, most recently used first. Press Insert again to put them back where they were. The pill floats on top of the grid; drag it aside if it covers a window.\r\n" +
                "  -  FadeAll only listens in the background, so PageUp / PageDown still reach other apps and g / q still type there. The flip side: the action fires at the same time - typing the letter g also ghosts / un-ghosts your last window, and typing q also steps the opacity up (it stops at 100%, so stray q presses cannot hide anything).\r\n" +
                "  -  Insert is taken over while FadeAll runs - its normal function is off (no overwrite mode), and only the bare key is captured: Ctrl+Insert and Shift+Insert still work in other apps.\r\n" +
                "  -  Holding Ctrl, Alt, Shift or Windows suppresses the action, so shortcuts like Ctrl+PageUp are left alone.\r\n\r\n" +
                "Click the Size button on the right to cycle the window you were last using:\r\n" +
                "  -  First click: 7 x 7 inches.\r\n" +
                "  -  Second click: 3 x 3 inches (smallest).\r\n" +
                "  -  Third click: back to full screen.\r\n" +
                "Then the cycle repeats. Sizes are centered where the window already was.\r\n\r\n" +
                "Drag the grip (the dotted strip on the far right of the pill) to size that window by hand:\r\n" +
                "  -  Drag right / down to grow it, left / up to shrink it. The top-left corner stays put.\r\n" +
                "  -  Any width and height you like - about an inch minimum, up to your screen size.\r\n" +
                "  -  The pill shows the live size while you drag. A plain click on the grip shows this tip.\r\n\r\n" +
                "While windows are hidden or dimmed:\r\n" +
                "  -  Hover over the pill to peek - the windows come back while your mouse is on it.\r\n" +
                "  -  Click the pill or press Ctrl+Alt+H to restore everything (PageDown also toggles hidden / full).\r\n\r\n" +
                "Drag the pill to move it. Right-click it for this menu.\r\n\r\n" +
                "Notes:\r\n" +
                "  -  Browser tabs live inside one window, so the whole browser window fades together.\r\n" +
                "  -  The Size button, the grip and G act on the last window you clicked outside the pill (when shrinking, FadeAll pushes past an app's own minimum size; a rare app may still snap itself back).\r\n" +
                "  -  Windows from apps running 'as administrator' cannot be faded unless FadeAll also runs as administrator.\r\n" +
                "  -  Some special windows (Store/UWP apps, full-screen games) may not fade.\r\n" +
                "  -  Quit with Exit so windows get restored. If the tool is force-killed while windows are hidden, restarting the affected app (or Windows) brings them back.";
            MessageBox.Show(this, text, "How FadeAll works", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            bool createdNew;
            using (var mutex = new Mutex(true, "FadeAll_SingleInstance", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("FadeAll is already running. Look for the small pill at the top of the screen.", "FadeAll");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new PillForm());
            }
        }
    }
}
