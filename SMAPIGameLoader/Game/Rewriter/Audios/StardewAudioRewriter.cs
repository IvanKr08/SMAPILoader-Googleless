using Mono.Cecil;

using System;

namespace SMAPIGameLoader.Game.Rewriter;

internal static class StardewAudioRewriter
{
    internal static void Rewrite(AssemblyDefinition stardewAssemblyDef)
    {
        try
        {
        }
        catch (Exception e)
        {
            ErrorDialogTool.Show(e, nameof(StardewAudioRewriter));
            throw;
        }
    }
}
