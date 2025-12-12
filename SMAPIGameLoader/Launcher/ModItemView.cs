using Newtonsoft.Json.Linq;

using System;
using System.IO;

namespace SMAPIGameLoader.Launcher;

public class ModItemView
{
    public string NameText = "Unknow";
    public string VersionText = "Unknow";
    public string FolderPathText = "Unknow";

    public readonly string modName = "unknow";
    public readonly string modVersion = "unknow";
    public readonly string modFolderPath = "unknow";

    public ModItemView(string manifestFilePath, int modListIndex)
    {
        try
        {
            var manifestText = File.ReadAllText(manifestFilePath);
            var manifest = JObject.Parse(manifestText);

            modName = manifest["Name"].ToString();
            modVersion = manifest["Version"].ToString();

            NameText = $"[{modListIndex + 1}]: {modName}";
            VersionText = $"Version: {modVersion}";

            modFolderPath = Path.GetDirectoryName(manifestFilePath);
            var relativeModDir = modFolderPath.Substring(modFolderPath.IndexOf("/Mods") + 5);
            FolderPathText = $"Folder: {relativeModDir}";
        }
        catch (Exception ex)
        {
            modFolderPath = Path.GetDirectoryName(manifestFilePath);
            FolderPathText = modFolderPath;
            ErrorDialogTool.Show(ex, "Error try parser mod folder path: " + modFolderPath);
        }

        NameText = $"[{modListIndex + 1}]: {modName}";
        VersionText = $"Version: {modVersion}";
    }
}
