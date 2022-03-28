namespace Geev.Api.Framrwork.Recovery;


[Flags]
public enum RestartRestrictions
{
    None = 0,
    NotOnCrash = 1,
    NotOnHang = 2,
    NotOnPatch = 4,
    NotOnReboot = 8,
}