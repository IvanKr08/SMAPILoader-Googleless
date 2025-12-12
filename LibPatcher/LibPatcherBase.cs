using System.Numerics;

using ELFSharp.ELF;
using ELFSharp.ELF.Sections;

namespace LibPatcher;

abstract class LibPatcherBase<T> where T : struct, IUnsignedNumber<T> {
    public Dictionary<string, SymbolEntry<T>> MonoMethodMap { get; } = new();
    public string LibFile { get; }

    internal LibPatcherBase(string dotnetHome) {
        var ver = "8.0.22";
        LibFile = Path.Combine(dotnetHome, @$"packs\Microsoft.NETCore.App.Runtime.Mono.android-{ArchName}\{ver}\runtimes\android-{ArchName}\native\libmonosgen-2.0.so");
    }

    public void Patch() {
        var bakFile = LibFile + ".bak";
        if (File.Exists(bakFile)) {
            Console.WriteLine("Backup file exists, skipping...");
            return;
        }
        File.Copy(LibFile, bakFile);

        try {
            using var libReader = ELFReader.Load(LibFile);

            var sect = (SymbolTable<T>)libReader.GetSection(".dynsym");
            foreach (var item in sect.Entries) {
                if (item.Type == SymbolType.Function && item.Name.StartsWith("mono_")) {
                    MonoMethodMap[item.Name] = item;
                }
            }

            libReader.Dispose();

            PatchData[] patches = [
                Patch_FieldAccessException(),
                Patch_MethodAccessException(),
                //Patch_mono_class_from_mono_type_internalCrashFix(),
            ];

            using var libWriter = File.Open(LibFile, FileMode.Open, FileAccess.ReadWrite);
            foreach (var patchData in patches) {
                var funcVAFile = GetFunctionOffsetVAFile(patchData.ExportFunctionName);
                var patchFileOffset = long.CreateChecked(funcVAFile) + patchData.Offset;

                Console.WriteLine($$"""
                Patch: {{patchData.ExportFunctionName}}
                    File offset      : 0x{{patchData.Offset:X}}
                    Byte length      : {{patchData.PatchBytes.Length}}
                    Patch file offset: 0x{{patchFileOffset:X}}
                """);

                WriteByteArray(libWriter, patchFileOffset, patchData.PatchBytes);
            }

            Console.WriteLine($"Successfully patched {ArchName} runtime");
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Revert();
        }
    }
    public void Revert() {
        var bakFile = LibFile + ".bak";

        if (File.Exists(bakFile)) {
            File.Copy(bakFile, LibFile, true);
            File.Delete(bakFile);
            Console.WriteLine($"Reverted {bakFile}");
        }
        else {
            Console.WriteLine($"No backup file exists: {bakFile}");
        }
    }

    T GetFunctionOffsetVASection(SymbolEntry<T> func) {
        return func.Value - func.PointedSection.Offset;
    }
    SymbolEntry<T> GetFunction(string name) {
        return MonoMethodMap[name];
    }
    T GetFunctionOffsetVAFile(string name) {
        var func            = GetFunction(name);
        var offsetOnSection = GetFunctionOffsetVASection(func);
        var section         = func.PointedSection;
        var headerOffset    = section.Offset;

        return headerOffset + offsetOnSection;
    }
    void WriteByteArray(FileStream file, long start, byte[] bytes) {
        file.Seek(start, SeekOrigin.Begin);
        file.Write(bytes);
    }

    protected abstract string ArchName { get; }
    protected abstract PatchData Patch_FieldAccessException();
    protected abstract PatchData Patch_MethodAccessException();
}

