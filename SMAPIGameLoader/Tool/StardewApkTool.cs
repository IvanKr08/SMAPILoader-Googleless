using Android.App;
using Android.Content.PM;

using System;
using System.Linq;

namespace SMAPIGameLoader;

internal static class StardewApkTool
{
    public const string GamePlayStorePackageName = "com.chucklefish.stardewvalley";
    public const string GameGalaxyStorePackageName = "com.chucklefish.stardewvalleysamsung";
    public static bool IsSplitContent { get; private set; }
    static PackageInfo _currentPackageInfo;

    //init at first SDK
    static StardewApkTool()
    {
        Console.WriteLine("Initialize Stardew Apk Tool");
        var playStore = ApkTool.GetPackageInfo(GamePlayStorePackageName);
        var samsung = ApkTool.GetPackageInfo(GameGalaxyStorePackageName);

        //select samsung first, better for debug, test app
        if (samsung != null)
        {
            _currentPackageInfo = samsung;
            Console.WriteLine("Game Install From Galaxy Store");
        }
        else if (playStore != null)
        {
            _currentPackageInfo = playStore;
            Console.WriteLine("Game Install From Play Store");

			//из-за священной войны с пиратами страдают обычные люди!!!
            var splitApks = CurrentPackageInfo.ApplicationInfo?.SplitSourceDirs;
            IsSplitContent = splitApks?.Count == 2;
        }
    }

    public static PackageInfo CurrentPackageInfo => _currentPackageInfo;

    public static bool IsInstalled
    {
        get
        {
            return CurrentPackageInfo != null;
        }
    }

    public static Android.Content.Context GetContext => Application.Context;
    public static string? BaseApkPath => CurrentPackageInfo.ApplicationInfo.PublicSourceDir;
    public static string? ContentApkPath
    {
        get
        {
            try
            {
                if (IsSplitContent) return CurrentPackageInfo.ApplicationInfo.SplitSourceDirs?.First(path => path.Contains("split_content"));

                return BaseApkPath;
            }
            catch (Exception ex)
            {
                ErrorDialogTool.Show(ex, "Error try to get ContentApkPath");
                return null;
            }
        }
    }

    public static Version GameVersionSupport
    {
        get
        {
            if (CurrentPackageInfo == null)
                return null;

            switch (CurrentPackageInfo.PackageName)
            {
                case GamePlayStorePackageName:
                    return new(1, 6, 15, 0);
                case GameGalaxyStorePackageName:
                    return new(1, 6, 14, 8);
                default:
                    return null;
            }
        }
    }
    public static Version CurrentGameVersion 
    {
        get
        {
            try
            {
                return new Version(CurrentPackageInfo?.VersionName);
            }
            catch(Exception ex)
            {
                return new Version(0,0,0,0);
            }
        }
    }
    public static bool IsGameVersionSupport => CurrentGameVersion >= GameVersionSupport;
}
