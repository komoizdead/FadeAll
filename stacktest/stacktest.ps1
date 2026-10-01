param(
  [Parameter(Mandatory=$true)][string]$cmd,
  [int]$n = 6
)

$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

Add-Type -TypeDefinition @'
using System;
using System.Text;
using System.Runtime.InteropServices;
public class ST {
  [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
  [DllImport("user32.dll")] public static extern bool EnumWindows(EnumProc cb, IntPtr l);
  public delegate bool EnumProc(IntPtr h, IntPtr l);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetWindowText(IntPtr h, StringBuilder sb, int max);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetClassName(IntPtr h, StringBuilder sb, int max);
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
  [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr h);
  [DllImport("user32.dll")] public static extern bool IsIconic(IntPtr h);
  [DllImport("user32.dll")] public static extern bool IsZoomed(IntPtr h);
  [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr h, int i);
  [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
  [DllImport("user32.dll")] static extern IntPtr MonitorFromWindow(IntPtr h, uint flags);
  [DllImport("user32.dll")] static extern bool GetMonitorInfo(IntPtr mon, ref MONITORINFO mi);
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT p);
  [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")] public static extern IntPtr WindowFromPoint(POINT p);
  [DllImport("user32.dll")] public static extern bool GetLayeredWindowAttributes(IntPtr h, out uint key, out byte alpha, out uint flags);
  [DllImport("user32.dll")] static extern void mouse_event(uint f, uint dx, uint dy, uint d, UIntPtr e);
  [DllImport("user32.dll")] static extern void keybd_event(byte vk, byte scan, uint f, UIntPtr e);
  [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr h, IntPtr after, int x, int y, int cx, int cy, uint f);
  [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);
  [DllImport("user32.dll")] public static extern uint GetDpiForWindow(IntPtr h);
  [DllImport("kernel32.dll")] public static extern void Sleep(int ms);
  [DllImport("dwmapi.dll")] static extern int DwmGetWindowAttribute(IntPtr h, int a, out int v, int s);
  [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L; public int T; public int R; public int B; }
  [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X; public int Y; }
  [StructLayout(LayoutKind.Sequential)] public struct MONITORINFO { public int cbSize; public RECT mon; public RECT work; public uint flags; }

  public static string Title(IntPtr h) { StringBuilder sb = new StringBuilder(520); GetWindowText(h, sb, 520); return sb.ToString(); }
  public static string Cls(IntPtr h) { StringBuilder sb = new StringBuilder(300); GetClassName(h, sb, 300); return sb.ToString(); }
  public static uint Pid(IntPtr h) { uint p; GetWindowThreadProcessId(h, out p); return p; }
  public static int Ex(IntPtr h) { return GetWindowLong(h, -20); }
  public static bool Cloaked(IntPtr h) { int v; return DwmGetWindowAttribute(h, 14, out v, 4) == 0 && v != 0; }
  public static string RectStr(IntPtr h) { RECT r; if (GetWindowRect(h, out r)) return r.L + "," + r.T + "," + r.R + "," + r.B; return "none"; }
  public static string LayeredStr(IntPtr h) { uint k; byte a; uint f; if (GetLayeredWindowAttributes(h, out k, out a, out f)) return "Lay=" + a; return "Lay=-"; }
  public static string CursorStr() { POINT p; if (GetCursorPos(out p)) return p.X + "," + p.Y; return "none"; }
  public static string FgStr() { return GetForegroundWindow().ToInt64().ToString("X"); }
  public static string WFP(int x, int y) { POINT p; p.X = x; p.Y = y; return WindowFromPoint(p).ToInt64().ToString("X"); }
  public static string WorkStr(IntPtr h) { IntPtr m = MonitorFromWindow(h, 2); MONITORINFO mi = new MONITORINFO(); mi.cbSize = Marshal.SizeOf(typeof(MONITORINFO)); if (GetMonitorInfo(m, ref mi)) return mi.work.L + "," + mi.work.T + "," + mi.work.R + "," + mi.work.B; return "none"; }

  public static string[] GetAll() {
    System.Collections.Generic.List<string> res = new System.Collections.Generic.List<string>();
    EnumWindows(delegate(IntPtr h, IntPtr l) {
      if (IsWindowVisible(h)) {
        string t = Title(h);
        res.Add(h.ToInt64().ToString("X") + "|" + Cls(h) + "|" + RectStr(h) + "|" + Pid(h) + "|" +
          (IsIconic(h) ? 1 : 0) + "|" + (IsZoomed(h) ? 1 : 0) + "|" + Ex(h) + "|" +
          (Cloaked(h) ? 1 : 0) + "|" + t.Length + "|" + t);
      }
      return true;
    }, IntPtr.Zero);
    return res.ToArray();
  }

  public static void MoveClick(int x, int y, bool right) {
    SetCursorPos(x, y); Sleep(160);
    mouse_event(right ? 8u : 2u, 0, 0, 0, UIntPtr.Zero); Sleep(40);
    mouse_event(right ? 16u : 4u, 0, 0, 0, UIntPtr.Zero); Sleep(60);
  }
  public static void Press(int vk) {
    keybd_event((byte)vk, 0, 0, UIntPtr.Zero); Sleep(40);
    keybd_event((byte)vk, 0, 2, UIntPtr.Zero); Sleep(90);
  }
  public static void CloseWin(IntPtr h) { PostMessage(h, 0x10, IntPtr.Zero, IntPtr.Zero); }

  [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);

  public static void ClickAtClient(IntPtr h, int x, int y) {
    IntPtr lp = (IntPtr)((y << 16) | (x & 0xFFFF));
    SendMessage(h, 0x200, IntPtr.Zero, lp); Sleep(80);
    SendMessage(h, 0x201, (IntPtr)1, lp); Sleep(60);
    SendMessage(h, 0x202, IntPtr.Zero, lp); Sleep(60);
  }

  public static string[] GetAll2() {
    System.Collections.Generic.List<string> res = new System.Collections.Generic.List<string>();
    EnumWindows(delegate(IntPtr h, IntPtr l) {
      string t = Title(h);
      res.Add(h.ToInt64().ToString("X") + "|" + Cls(h) + "|" + RectStr(h) + "|" + Pid(h) + "|" +
        (IsIconic(h) ? 1 : 0) + "|" + (IsZoomed(h) ? 1 : 0) + "|" + Ex(h) + "|" +
        (Cloaked(h) ? 1 : 0) + "|" + (IsWindowVisible(h) ? 1 : 0) + "|" + t.Length + "|" + t);
      return true;
    }, IntPtr.Zero);
    return res.ToArray();
  }
}

[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct VARIANT {
  [FieldOffset(0)] public ushort vt;
  [FieldOffset(8)] public int intVal;
  [FieldOffset(8)] public IntPtr ptr;
}

[ComImport, Guid("618736E0-3C3D-11CF-810C-00AA00389B71"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAccessible {
  [PreserveSig] int GetTypeInfoCount(out uint pctinfo);
  [PreserveSig] int GetTypeInfo(uint iTInfo, uint lcid, out IntPtr ppTInfo);
  [PreserveSig] int GetIDsOfNames(ref Guid riid, string[] rgszNames, uint cNames, uint lcid, int[] rgDispId);
  [PreserveSig] int Invoke(int dispIdMember, ref Guid riid, uint lcid, ushort wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
  [PreserveSig] int get_accParent(out IntPtr ppdispParent);
  [PreserveSig] int get_accChildCount(out int pcountChildren);
  [PreserveSig] int get_accChild(ref VARIANT varChild, out IntPtr ppdispChild);
  [PreserveSig] int get_accName(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszName);
  [PreserveSig] int get_accValue(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszValue);
  [PreserveSig] int get_accDescription(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszDescription);
  [PreserveSig] int get_accRole(ref VARIANT varChild, out VARIANT pvarRole);
  [PreserveSig] int get_accState(ref VARIANT varChild, out VARIANT pvarState);
  [PreserveSig] int get_accHelp(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszHelp);
  [PreserveSig] int get_accHelpTopic([MarshalAs(UnmanagedType.BStr)] out string pszHelpFile, ref VARIANT varChild, out int pidTopic);
  [PreserveSig] int get_accKeyboardShortcut(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszKeyboardShortcut);
  [PreserveSig] int get_accFocus(out VARIANT pvarChild);
  [PreserveSig] int get_accSelection(out VARIANT pvarChild);
  [PreserveSig] int get_accDefaultAction(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] out string pszDefaultAction);
  [PreserveSig] int accSelect(int flagsSelect, ref VARIANT varChild);
  [PreserveSig] int accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, ref VARIANT varChild);
  [PreserveSig] int accNavigate(int navDir, ref VARIANT varStart, out VARIANT pvarEndUpAt);
  [PreserveSig] int accHitTest(int xLeft, int yTop, out VARIANT pvarChild);
  [PreserveSig] int accDoDefaultAction(ref VARIANT varChild);
  [PreserveSig] int put_accName(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] string szName);
  [PreserveSig] int put_accValue(ref VARIANT varChild, [MarshalAs(UnmanagedType.BStr)] string szValue);
}

public class MS {
  [DllImport("oleacc.dll")]
  static extern int AccessibleObjectFromWindow(IntPtr hwnd, uint dwId, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

  static IAccessible GetAcc(IntPtr hwnd) {
    Guid iid = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
    object o = null;
    int hr = AccessibleObjectFromWindow(hwnd, 0xFFFFFFFCu, ref iid, out o);
    if (hr != 0 || o == null) throw new Exception("AccessibleObjectFromWindow hr=" + hr);
    return (IAccessible)o;
  }

  public static string Dump(IntPtr hwnd) {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    try {
      IAccessible acc = GetAcc(hwnd);
      int cnt; acc.get_accChildCount(out cnt);
      sb.Append("ACC_COUNT=" + cnt);
      for (int i = 1; i <= cnt; i++) {
        VARIANT c = new VARIANT(); c.vt = 3; c.intVal = i;
        string nm = ""; acc.get_accName(ref c, out nm);
        VARIANT role; acc.get_accRole(ref c, out role);
        VARIANT st; acc.get_accState(ref c, out st);
        int l, t, w2, h2; acc.accLocation(out l, out t, out w2, out h2, ref c);
        sb.Append("\nMI[" + i + "] ROLE=" + role.intVal + " STATE=" + st.intVal + " LOC=" + l + "," + t + "," + (l + w2) + "," + (t + h2) + " NAME=[" + nm + "]");
      }
    } catch (Exception ex) {
      sb.Append("ACCEX: " + ex.Message);
    }
    return sb.ToString();
  }

  public static string Find(IntPtr hwnd, string sub) {
    try {
      IAccessible acc = GetAcc(hwnd);
      int cnt; acc.get_accChildCount(out cnt);
      for (int i = 1; i <= cnt; i++) {
        VARIANT c = new VARIANT(); c.vt = 3; c.intVal = i;
        string nm = ""; acc.get_accName(ref c, out nm);
        if (nm != null && nm.ToLower().IndexOf(sub.ToLower()) >= 0) {
          VARIANT st; acc.get_accState(ref c, out st);
          int l, t, w2, h2; acc.accLocation(out l, out t, out w2, out h2, ref c);
          return "FOUND idx=" + i + " LOC=" + l + "," + t + "," + (l + w2) + "," + (t + h2) + " STATE=" + st.intVal + " NAME=[" + nm + "]";
        }
      }
      return "NOTFOUND count=" + cnt;
    } catch (Exception ex) {
      return "ACCEX: " + ex.Message;
    }
  }

  public static string Act(IntPtr hwnd, string sub) {
    try {
      IAccessible acc = GetAcc(hwnd);
      int cnt; acc.get_accChildCount(out cnt);
      for (int i = 1; i <= cnt; i++) {
        VARIANT c = new VARIANT(); c.vt = 3; c.intVal = i;
        string nm = ""; acc.get_accName(ref c, out nm);
        if (nm != null && nm.ToLower().IndexOf(sub.ToLower()) >= 0) {
          string da = ""; acc.get_accDefaultAction(ref c, out da);
          int hr = acc.accDoDefaultAction(ref c);
          return "ACT idx=" + i + " HR=0x" + hr.ToString("X8") + " DA=[" + da + "] NAME=[" + nm + "]";
        }
      }
      return "ACT_NOTFOUND count=" + cnt;
    } catch (Exception ex) {
      return "ACTEX: " + ex.Message;
    }
  }
}
'@

[void][ST]::SetProcessDPIAware()

$script:skip = @('Progman','WorkerW','Shell_TrayWnd','Shell_SecondaryTrayWnd','Windows.UI.Core.CoreWindow','ApplicationFrameWindow')

function Parse-Win([string]$line) {
  $p = $line.Split([char]'|', 10)
  $r = $p[2].Split(',')
  $o = New-Object psobject
  $o | Add-Member NoteProperty Hex $p[0]
  $o | Add-Member NoteProperty H ([IntPtr][Convert]::ToInt64($p[0],16))
  $o | Add-Member NoteProperty Cls $p[1]
  $o | Add-Member NoteProperty L ([int]$r[0])
  $o | Add-Member NoteProperty T ([int]$r[1])
  $o | Add-Member NoteProperty R ([int]$r[2])
  $o | Add-Member NoteProperty B ([int]$r[3])
  $o | Add-Member NoteProperty Pid ([int]$p[3])
  $o | Add-Member NoteProperty Iconic ($p[4] -eq '1')
  $o | Add-Member NoteProperty Zoomed ($p[5] -eq '1')
  $o | Add-Member NoteProperty Ex ([int]$p[6])
  $o | Add-Member NoteProperty Cloaked ($p[7] -eq '1')
  $o | Add-Member NoteProperty TitleLen ([int]$p[8])
  $o | Add-Member NoteProperty Title ($p[9].Replace("`r"," ").Replace("`n"," "))
  return $o
}

function Get-Wins() {
  $out = New-Object System.Collections.ArrayList
  foreach ($l in [ST]::GetAll()) { [void]$out.Add((Parse-Win $l)) }
  return $out.ToArray()
}

function Get-FadePid() {
  $pr = Get-Process FadeAll -ErrorAction SilentlyContinue
  if (-not $pr) { Write-Output 'NOFADEALL'; exit 2 }
  return [int]$pr.Id
}

function Find-Pill($wins, [int]$fadePid) {
  foreach ($w in $wins) { if ($w.Title -eq 'FadeAll' -and $w.Pid -eq $fadePid) { return $w } }
  return $null
}

function Find-Menu($wins, [int]$fadePid, [string]$pillHex) {
  foreach ($w in $wins) {
    if ($w.Pid -eq $fadePid -and $w.Hex -ne $pillHex -and $w.Cls -like 'WindowsForms10*' -and ($w.R - $w.L) -ge 220 -and ($w.B - $w.T) -ge 140) { return $w }
  }
  return $null
}

function Test-Elig($w, [int]$fadePid) {
  if ($w.Iconic) { return $false }
  if ($w.Pid -eq $fadePid) { return $false }
  if (($w.Ex -band 0x80) -ne 0) { return $false }
  if ($script:skip -contains $w.Cls) { return $false }
  if ($w.Cloaked) { return $false }
  if ($w.TitleLen -le 0) { return $false }
  return $true
}

if ($cmd -eq 'list') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND' } else {
    Write-Output ("PILL=" + $pill.Hex + " RECT=" + $pill.L + "," + $pill.T + "," + $pill.R + "," + $pill.B + " DPI=" + [ST]::GetDpiForWindow($pill.H))
    Write-Output ("WORKAREA=" + [ST]::WorkStr($pill.H))
  }
  $z = 0
  $elig = 0
  foreach ($w in $wins) {
    $z++
    if ($w.TitleLen -gt 0) {
      $e = 0
      if (Test-Elig $w $fadePid) { $e = 1; $elig++ }
      Write-Output ("Z=" + $z + " HWND=" + $w.Hex + " ELIG=" + $e + " ZOOM=" + [int]$w.Zoomed + " ICONIC=" + [int]$w.Iconic + " RECT=" + $w.L + "," + $w.T + "," + $w.R + "," + $w.B + " CLASS=" + $w.Cls + " PID=" + $w.Pid + " TITLE=" + $w.Title)
    } else {
      Write-Output ("Z=" + $z + " HWND=" + $w.Hex + " ELIG=0 NOTITLE CLASS=" + $w.Cls + " PID=" + $w.Pid + " RECT=" + $w.L + "," + $w.T + "," + $w.R + "," + $w.B)
    }
  }
  Write-Output ("ELIGCOUNT=" + $elig)
  Write-Output 'END'
  exit 0
}

if ($cmd -eq 'spawn') {
  $script:remaining = 0
  $script:forms = New-Object System.Collections.ArrayList
  $script:clog = Join-Path $PSScriptRoot 'spawn_close.log'
  try { Add-Content -Path $script:clog -Value ("=== SPAWN n=" + $n + " PID=" + $PID + " AT " + (Get-Date -Format 'HH:mm:ss.f')) } catch { }
  for ($i = 1; $i -le $n; $i++) {
    $f = New-Object System.Windows.Forms.Form
    $f.AutoScaleMode = [System.Windows.Forms.AutoScaleMode]::None
    $f.Text = "STACKTEST $i"
    $f.StartPosition = [System.Windows.Forms.FormStartPosition]::Manual
    $f.Location = New-Object System.Drawing.Point((80 + ($i-1)*90), (130 + ($i-1)*70))
    $f.Size = New-Object System.Drawing.Size(560, 420)
    $f.BackColor = [System.Drawing.Color]::FromArgb([Math]::Min(255, 40 + $i*25), 60, 90)
    $lbl = New-Object System.Windows.Forms.Label
    $lbl.Text = "STACKTEST $i"
    $lbl.Dock = [System.Windows.Forms.DockStyle]::Fill
    $lbl.TextAlign = [System.Drawing.ContentAlignment]::MiddleCenter
    $lbl.Font = New-Object System.Drawing.Font("Segoe UI", 28, [System.Drawing.FontStyle]::Bold)
    $lbl.ForeColor = [System.Drawing.Color]::White
    $f.Controls.Add($lbl)
    $script:remaining++
    $f.Add_FormClosed({
      try { Add-Content -Path $script:clog -Value ((Get-Date -Format 'HH:mm:ss.f') + " CLOSED " + $args[0].Text + " REASON=" + $args[1].CloseReason) } catch { }
      $script:remaining--
      if ($script:remaining -le 0) { [System.Windows.Forms.Application]::ExitThread() }
    })
    $f.Show()
    try { Add-Content -Path $script:clog -Value ((Get-Date -Format 'HH:mm:ss.f') + " CREATED " + $f.Text) } catch { }
    [void]$script:forms.Add($f)
  }
  $PID | Out-File -Encoding ascii (Join-Path $PSScriptRoot 'spawn.pid')
  Write-Output ("SPAWNED=" + $n + " PID=" + $PID)
  try { [System.Windows.Forms.Application]::Run() } catch { Write-Output ("RUNEX: " + $_.Exception.Message) }
  Write-Output ("ENDRUN remaining=" + $script:remaining)
  try { Add-Content -Path $script:clog -Value ("=== SPAWN END remaining=" + $script:remaining + " AT " + (Get-Date -Format 'HH:mm:ss.f')) } catch { }
  exit 0
}

if ($cmd -eq 'raise') {
  $wins = Get-Wins
  for ($i = 1; $i -le $n; $i++) {
    $t = "STACKTEST $i"
    $found = $null
    foreach ($w in $wins) { if ($w.Title -eq $t) { $found = $w; break } }
    if (-not $found) { Write-Output ("RAISEFAIL=" + $t); continue }
    [void][ST]::SetWindowPos($found.H, [IntPtr]::Zero, 0, 0, 0, 0, 0x13)
    [ST]::Sleep(120)
  }
  Start-Sleep -Milliseconds 300
  $ord = New-Object System.Collections.ArrayList
  foreach ($w in (Get-Wins)) { if ($w.Title -like 'STACKTEST *') { [void]$ord.Add($w.Title) } }
  Write-Output ("ORDER_TOP_DOWN=" + ($ord -join ','))
  exit 0
}

if ($cmd -eq 'trigger') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND'; exit 1 }
  $px = [int]($pill.L + 100)
  $py = [int]($pill.T + 20)

  $dd = $null
  $mode = 'NONE'

  # attempt 1: UIA click on the menu item
  [ST]::MoveClick($px, $py, $true)
  for ($i = 0; $i -lt 40; $i++) {
    [ST]::Sleep(100)
    $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
    if ($dd) { break }
  }
  if (-not $dd) { Write-Output 'MENU=NOTFOUND'; exit 1 }
  [ST]::Sleep(400)
  $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
  Write-Output ("DD=" + $dd.Hex + " RECT=" + $dd.L + "," + $dd.T + "," + $dd.R + "," + $dd.B)
  try {
    $info = [MS]::Find($dd.H, 'Stack')
    Write-Output ("MSAA_" + $info)
    if ($info.StartsWith('FOUND')) {
      $m = [regex]::Match($info, 'LOC=(\d+),(\d+),(\d+),(\d+)')
      $l = [int]$m.Groups[1].Value; $t = [int]$m.Groups[2].Value
      $r = [int]$m.Groups[3].Value; $b = [int]$m.Groups[4].Value
      $cx = [int](($l + $r) / 2); $cy = [int](($t + $b) / 2)
      Write-Output ("MSAA_CLICK=" + $cx + "," + $cy)
      [ST]::MoveClick($cx, $cy, $false)
      $mode = 'MSAA'
    } else {
      $mode = 'NOITEM'
    }
  } catch {
    $mode = 'MSAAERR'
    Write-Output ("MSAAEX: " + $_.Exception.Message)
  }
  [ST]::Sleep(600)

  $stillOpen = $false
  foreach ($w in (Get-Wins)) { if ($w.Hex -eq $dd.Hex) { $stillOpen = $true } }
  $stillOpen2 = $false

  if ($stillOpen) {
    Write-Output ("ATTEMPT1_LEFT_MENU_OPEN=1 mode=" + $mode)
    [ST]::Press(0x1B)
    [ST]::Sleep(400)
    [ST]::MoveClick($px, $py, $true)
    $dd2 = $null
    for ($i = 0; $i -lt 40; $i++) {
      [ST]::Sleep(100)
      $dd2 = Find-Menu (Get-Wins) $fadePid $pill.Hex
      if ($dd2) { break }
    }
    if ($dd2) {
      [ST]::Sleep(400)
      for ($k = 0; $k -lt 7; $k++) { [ST]::Press(0x28) }
      [ST]::Press(0x0D)
      $mode = 'KEYBOARD'
      [ST]::Sleep(600)
      foreach ($w in (Get-Wins)) { if ($w.Hex -eq $dd2.Hex) { $stillOpen2 = $true } }
      if ($stillOpen2) { [ST]::Press(0x1B); $mode = 'KEYBOARD_FAIL'; [ST]::Sleep(300) }
    } else {
      $mode = 'REOPEN_FAIL'
    }
  }

  $helpOpen = $false
  foreach ($w in (Get-Wins)) { if ($w.Title -eq 'How FadeAll works') { $helpOpen = $true } }
  if ($helpOpen) { Write-Output 'MISFIRE_HELP_DIALOG=1'; [ST]::Press(0x0D); [ST]::Sleep(400) }

  Write-Output ("TRIGGERMODE=" + $mode + " MENUOPEN_AT_END=" + [int]$stillOpen2)
  [void][ST]::SetCursorPos([int]($pill.L - 400), 760)
  Write-Output 'TRIGGERDONE=1'
  exit 0
}

if ($cmd -eq 'probe') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND'; exit 1 }
  Write-Output ("PILL=" + $pill.Hex + " RECT=" + $pill.L + "," + $pill.T + "," + $pill.R + "," + $pill.B)
  $px = [int]($pill.L + 100)
  $py = [int]($pill.T + 20)
  [void][ST]::SetCursorPos($px, $py)
  [ST]::Sleep(250)
  Write-Output ("CURSOR_BEFORE_PRESS=" + [ST]::CursorStr() + " WFP_AT_CLICK=" + [ST]::WFP($px, $py))
  [ST]::MoveClick($px, $py, $true)
  Write-Output ("CURSOR_AFTER_PRESS=" + [ST]::CursorStr() + " FG=" + [ST]::FgStr())
  for ($k = 1; $k -le 8; $k++) {
    [ST]::Sleep(250)
    Write-Output ("--- t=" + ($k*250) + "ms FG=" + [ST]::FgStr() + " CURSOR=" + [ST]::CursorStr())
    foreach ($w in (Get-Wins)) {
      if ($w.Pid -eq $fadePid) {
        Write-Output ("    HWND=" + $w.Hex + " CLS=" + $w.Cls + " RECT=" + $w.L + "," + $w.T + "," + $w.R + "," + $w.B + " EX=" + $w.Ex + " TL=" + $w.TitleLen + " TITLE=" + $w.Title)
      }
    }
  }
  [ST]::Press(0x1B)
  [ST]::Sleep(200)
  exit 0
}

if ($cmd -eq 'state') {
  foreach ($w in (Get-Wins)) {
    if ($w.TitleLen -gt 0) {
      Write-Output ("HWND=" + $w.Hex + " EX=0x" + ("{0:X}" -f $w.Ex) + " " + [ST]::LayeredStr($w.H) + " PID=" + $w.Pid + " TITLE=" + $w.Title)
    }
  }
  Write-Output 'STATEEND'
  exit 0
}

if ($cmd -eq 'menudump') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND'; exit 1 }
  $px = [int]($pill.L + 100)
  $py = [int]($pill.T + 20)
  [ST]::MoveClick($px, $py, $true)
  $dd = $null
  for ($i = 0; $i -lt 40; $i++) {
    [ST]::Sleep(100)
    $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
    if ($dd) { break }
  }
  if (-not $dd) { Write-Output 'MENU=NOTFOUND'; exit 1 }
  [ST]::Sleep(400)
  Write-Output ("DD=" + $dd.Hex + " RECT=" + $dd.L + "," + $dd.T + "," + $dd.R + "," + $dd.B)
  try {
    Add-Type -AssemblyName UIAutomationClient
    $root = [System.Windows.Automation.AutomationElement]::FromHandle($dd.H)
    $items = $root.FindAll([System.Windows.Automation.TreeScope]::Descendants, [System.Windows.Automation.Condition]::TrueCondition)
    Write-Output ("UIA_COUNT=" + $items.Count)
    $k = 0
    foreach ($it in $items) {
      $k++
      if ($k -gt 40) { break }
      try {
        $rc = $it.Current.BoundingRectangle
        $nm = $it.Current.Name
        Write-Output ("UIA_ITEM[" + $k + "] CT=" + $it.Current.ControlType.ProgrammaticName + " RECT=" + [int]$rc.X + "," + [int]$rc.Y + "," + [int]($rc.X+$rc.Width) + "," + [int]($rc.Y+$rc.Height) + " ENABLED=" + [int]$it.Current.IsEnabled + " NAME=[" + $nm + "]")
      } catch {
        Write-Output ("UIA_ITEM[" + $k + "] ERR " + $_.Exception.Message)
      }
    }
  } catch {
    Write-Output ("UIAEX: " + $_.Exception.Message)
  }
  [ST]::Press(0x1B)
  [ST]::Sleep(300)
  Write-Output 'MENUDUMP_DONE=1'
  exit 0
}

if ($cmd -eq 'msaa') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND'; exit 1 }
  $px = [int]($pill.L + 100)
  $py = [int]($pill.T + 20)
  [ST]::MoveClick($px, $py, $true)
  $dd = $null
  for ($i = 0; $i -lt 40; $i++) {
    [ST]::Sleep(100)
    $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
    if ($dd) { break }
  }
  if (-not $dd) { Write-Output 'MENU=NOTFOUND'; exit 1 }
  [ST]::Sleep(400)
  Write-Output ("DD=" + $dd.Hex + " RECT=" + $dd.L + "," + $dd.T + "," + $dd.R + "," + $dd.B)
  Write-Output ([MS]::Dump($dd.H))
  [ST]::Press(0x1B)
  [ST]::Sleep(300)
  Write-Output 'MSAA_DONE=1'
  exit 0
}

if ($cmd -eq 'allwins') {
  foreach ($l in [ST]::GetAll2()) {
    $p = $l.Split([char]'|', 11)
    if ([int]$p[9] -gt 0) {
      Write-Output ("VIS=" + $p[8] + " HWND=" + $p[0] + " ICONIC=" + $p[4] + " ZOOM=" + $p[5] + " RECT=" + $p[2] + " CLASS=" + $p[1] + " PID=" + $p[3] + " TITLE=" + $p[10])
    }
  }
  Write-Output 'ALLWINS_DONE=1'
  exit 0
}

if ($cmd -eq 'msaaact') {
  $fadePid = Get-FadePid
  $wins = Get-Wins
  $pill = Find-Pill $wins $fadePid
  if (-not $pill) { Write-Output 'PILL=NOTFOUND'; exit 1 }
  $px = [int]($pill.L + 100)
  $py = [int]($pill.T + 20)

  $open = Find-Menu $wins $fadePid $pill.Hex
  if ($open) {
    Write-Output ("STALE_MENU=" + $open.Hex + " CLOSING=1")
    [ST]::Press(0x1B)
    [ST]::Sleep(500)
  }

  Write-Output ("T=" + (Get-Date -Format 'HH:mm:ss.f') + " RIGHTCLICK_START")
  [ST]::MoveClick($px, $py, $true)
  $dd = $null
  for ($i = 0; $i -lt 40; $i++) {
    [ST]::Sleep(100)
    $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
    if ($dd) { break }
  }
  if (-not $dd) { Write-Output 'MENU=NOTFOUND'; exit 1 }
  Write-Output ("T=" + (Get-Date -Format 'HH:mm:ss.f') + " MENU_OPEN")
  [ST]::Sleep(400)
  $dd = Find-Menu (Get-Wins) $fadePid $pill.Hex
  Write-Output ("DD=" + $dd.Hex + " RECT=" + $dd.L + "," + $dd.T + "," + $dd.R + "," + $dd.B)

  Write-Output ("T=" + (Get-Date -Format 'HH:mm:ss.f') + " ACTING")
  $act = [MS]::Act($dd.H, 'Stack')
  Write-Output ("MSAA_" + $act)
  [ST]::Sleep(700)
  Write-Output ("T=" + (Get-Date -Format 'HH:mm:ss.f') + " ACT_DONE")

  $stillOpen = $false
  foreach ($w in (Get-Wins)) { if ($w.Hex -eq $dd.Hex) { $stillOpen = $true } }
  Write-Output ("MENU_AFTER_ACT=" + [int]$stillOpen)

  if ($stillOpen) {
    $info = [MS]::Find($dd.H, 'Stack')
    Write-Output ("MSAA2_" + $info)
    if ($info.StartsWith('FOUND')) {
      $m = [regex]::Match($info, 'LOC=(\d+),(\d+),(\d+),(\d+)')
      $l = [int]$m.Groups[1].Value; $t = [int]$m.Groups[2].Value
      $r = [int]$m.Groups[3].Value; $b = [int]$m.Groups[4].Value
      $cx = [int](($l + $r) / 2); $cy = [int](($t + $b) / 2)
      $ccx = $cx - $dd.L; $ccy = $cy - $dd.T
      Write-Output ("MSGCLICK_SCREEN=" + $cx + "," + $cy + " CLIENT=" + $ccx + "," + $ccy + " WFP=" + [ST]::WFP($cx, $cy))
      [ST]::ClickAtClient($dd.H, $ccx, $ccy)
      [ST]::Sleep(700)
      $stillOpen2 = $false
      foreach ($w in (Get-Wins)) { if ($w.Hex -eq $dd.Hex) { $stillOpen2 = $true } }
      Write-Output ("MENU_AFTER_MSGCLICK=" + [int]$stillOpen2)
      if ($stillOpen2) { [ST]::Press(0x1B); [ST]::Sleep(300) }
    }
  }

  $helpOpen = $false
  foreach ($w in (Get-Wins)) { if ($w.Title -eq 'How FadeAll works') { $helpOpen = $true } }
  if ($helpOpen) { Write-Output 'HELP_DIALOG=1'; [ST]::Press(0x0D); [ST]::Sleep(400) }

  foreach ($w in (Get-Wins)) {
    if ($w.Pid -eq $fadePid) {
      Write-Output ("FWND HWND=" + $w.Hex + " CLS=" + $w.Cls + " RECT=" + $w.L + "," + $w.T + "," + $w.R + "," + $w.B + " TL=" + $w.TitleLen + " TITLE=" + $w.Title)
    }
  }
  [void][ST]::SetCursorPos([int]($pill.L - 400), 760)
  Write-Output 'MSAAACT_DONE=1'
  exit 0
}

if ($cmd -eq 'close') {
  $closed = 0
  foreach ($w in (Get-Wins)) { if ($w.Title -like 'STACKTEST *') { [ST]::CloseWin($w.H); $closed++ } }
  Write-Output ("CLOSED=" + $closed)
  exit 0
}

Write-Output 'UNKNOWN_CMD'
exit 3
