using System.Runtime.InteropServices;

namespace Geev.Api.Framrwork.Recovery;

public static class Recovery
{
    public delegate int RecoveryDelegate(IntPtr parameterData);

    [DllImport("kernel32.dll")]
    public static extern int RegisterApplicationRecoveryCallback(RecoveryDelegate recoveryCallback, IntPtr parameter, uint pingInterval, uint flags);

    [DllImport("kernel32.dll")]
    public static extern int ApplicationRecoveryInProgress(out bool canceled);

    [DllImport("kernel32.dll")]
    public static extern void ApplicationRecoveryFinished(bool success);

    [DllImport("kernel32.dll")]
    public static extern int UnregisterApplicationRecoveryCallback();

    [DllImport("kernel32.dll")]
    public static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.LPWStr)] string commandLineArgs, RestartRestrictions flags);

    [DllImport("kernel32.dll")]
    public static extern int UnregisterApplicationRestart();

   
}
