using LibPatcher;

class LibPatcherX86 : LibPatcherBase<uint> {
    protected override string ArchName { get => "x86"; }

    public LibPatcherX86(string dotnetHome) : base(dotnetHome) { }

    protected override PatchData Patch_MethodAccessException() {
        return new("mono_method_can_access_method", 0x128, [
            // (different from all archs)
            // HACK V2: I don't need to manually write 1 (5 bytes) when I can just jump (2 bytes) to place that will do this
            // <end of mono_method_can_access_method_full - cleanup>
            // ...
            // mov eax, 1  <----------------------------------x
            // jmp lCleanup                                   |
            0xEB, 0xF7, // (xor eax, eax) => (jmp short) -----x
            // lCleanup:
            // <cleanup>
            // retn
        ]);
    }
    protected override PatchData Patch_FieldAccessException() {
        return new("mono_method_can_access_field", 0x161, [
            // <func end - <some bytes>>
            0x90, 0x90, // (xor ecx, ecx) => (nop) * 2
            0x90, 0x90, // (jmp short)    => (nop) * 2
            // mov ecx, 1
            // mov eax, ecx
            // <cleanup>
            // retn
        ]);
    }
}
