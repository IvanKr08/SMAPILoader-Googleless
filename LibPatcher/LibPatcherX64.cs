using LibPatcher;

class LibPatcherX64 : LibPatcherBase<ulong> {
    protected override string ArchName { get => "x64"; }

    public LibPatcherX64(string dotnetHome) : base(dotnetHome) { }

    protected override PatchData Patch_MethodAccessException() {
        return new("mono_method_can_access_method", 0x35, [
            // <start of mono_method_can_access_method_full>
            // ...
            // mov eax, 1
            0x90, 0x90, 0x90, 0x90, // (test byte ptr [rdi+20h], 7Ch)   => (nop) * 4
            0x90, 0x90,             // (jz short <jmp to continuation>) => (nop) * 2
            // retn
        ]);
    }
    protected override PatchData Patch_FieldAccessException() {
        return new("mono_method_can_access_field", 0x12D, [
            // <func end - cleanup>
            // HACK: (mov eax, 1) won't fit => 5 bytes. Zero and increment eax => 4 bytes. Assembly world is weird
            0x31, 0xC0, // (xor ebp, ebp) => (xor eax, eax)
            0xFF, 0xC0, // (mov eax, ebp) => (inc eax)
            // <cleanup>
            // retn
        ]);
    }
}
