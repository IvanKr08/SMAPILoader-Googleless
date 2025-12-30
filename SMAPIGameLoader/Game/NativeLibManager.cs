using System;
using System.IO;

using LWJGL;

using MonoGame.Framework.Utilities;

namespace SMAPIGameLoader.Game;

internal static class NativeLibManager
{
    static nint Load_libLZ4()
    {
        nint num = FuncLoader.LoadLibrary("liblwjgl_lz4.so");
        if (num == IntPtr.Zero)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string directoryName = Path.GetDirectoryName(folderPath);
            string libname = Path.Combine(directoryName, "lib", "liblwjgl_lz4.so");
            num = FuncLoader.LoadLibrary(libname);
        }
        return num;
    }
    public static void Loads()
    {
        try
        {
            int b = LZ4.CompressBound(10);
            Console.WriteLine("done setup native libs");
        }
        catch (Exception ex)
        {
            ErrorDialogTool.Show(ex);
        }
    }
}
