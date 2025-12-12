using HarmonyLib;

using StardewValley;

using System;

namespace SMAPIGameLoader;

internal class MainActivityPatcher
{
    public static bool PrefixCheckStorageMigration(ref bool __result)
    {
        Console.WriteLine("bypass CheckStorageMigration");
        __result = false;
        return false;
    }

    internal static void Apply()
    {
        var harmony = new Harmony("SMAPIGameLoader");
        var PrefixCheckStorageMigration = AccessTools.Method(
            typeof(MainActivityPatcher), nameof(MainActivityPatcher.PrefixCheckStorageMigration));
        var CheckStorageMigration = AccessTools.Method(
            typeof(MainActivity), nameof(MainActivity.CheckStorageMigration));
        harmony.Patch(CheckStorageMigration, prefix: PrefixCheckStorageMigration);
        Console.WriteLine("Done MainActivityPatcher.Apply()");
    }
}
