param(
  [Parameter(Mandatory=$true)][int]$targetPid,
  [string]$log = 'watch.log'
)
Add-Type -TypeDefinition @'
using System; using System.Text; using System.Runtime.InteropServices;
public class W {
  [DllImport("user32.dll")] static extern bool EnumWindows(EnumProc cb, IntPtr l);
  public delegate bool EnumProc(IntPtr h, IntPtr l);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetWindowText(IntPtr h, StringBuilder sb, int max);
  [DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
  public static int Count(uint target) {
    int n = 0;
    EnumWindows(delegate(IntPtr h, IntPtr l) {
      uint p; GetWindowThreadProcessId(h, out p);
      if (p == target) {
        StringBuilder sb = new StringBuilder(300);
        GetWindowText(h, sb, 300);
        if (sb.ToString().StartsWith("STACKTEST")) n++;
      }
      return true;
    }, IntPtr.Zero);
    return n;
  }
}
'@
$first = $true
while ($true) {
  $n = [W]::Count([uint32]$targetPid)
  $line = (Get-Date -Format 'HH:mm:ss.f') + " STCOUNT=" + $n
  if ($first) { $line = $line + " START"; $first = $false }
  Add-Content -Path $log -Value $line
  if ($n -eq 0) { break }
  Start-Sleep -Milliseconds 500
}
