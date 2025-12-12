extern alias MonoCecilAlias;

using Mono.Cecil;

namespace SMAPIGameLoader;

public class StardewAssembliesResolver : DefaultAssemblyResolver
{
    public static StardewAssembliesResolver Instance { get; } = new();

    public StardewAssembliesResolver() : base()
    {
        AddSearchDirectory(GameAssemblyManager.AssembliesDirPath);
    }
}
