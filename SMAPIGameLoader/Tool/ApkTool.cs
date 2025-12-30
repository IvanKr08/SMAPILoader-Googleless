using Android.App;
using Android.Content.PM;
using Android.OS;

using System;

using Xamarin.Essentials;

namespace SMAPIGameLoader;

internal static class ApkTool
{
    public static int LauncherBuildCode => int.Parse(AppInfo.BuildString);
    public static Version AppVersion => AppInfo.Version;
    public static string PackageName => AppInfo.PackageName;

    public static PackageInfo GetPackageInfo(string PackageName)
    {
        try
        {
            var ctx = Application.Context;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
#pragma warning disable CA1416
                return ctx.PackageManager.GetPackageInfo(PackageName, PackageManager.PackageInfoFlags.Of(PackageInfoFlagsLong.None));
#pragma warning restore
            else
                return ctx.PackageManager.GetPackageInfo(PackageName, 0);
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static bool IsInstalled(string packageName) => GetPackageInfo(packageName) != null;
}
