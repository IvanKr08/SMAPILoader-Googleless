using HarmonyLib;

using Microsoft.Xna.Framework.Audio;

namespace SMAPIGameLoader.Game.Rewriter;

public static class AudioEngineWrapperMethods
{
    public static int GetCategoryIndex(object audioEngine, string name)
    {
        var _categories = AccessTools.Field(typeof(AudioEngine), "_categories").GetValue(audioEngine) as AudioCategory[];

        for (int i = 0; i < _categories.Length; i++)
        {
            if (_categories[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }
}
