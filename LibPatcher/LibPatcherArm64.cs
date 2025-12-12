using LibPatcher;

class LibPatcherArm64 : LibPatcherBase<ulong> {
    protected override string ArchName { get => "arm64"; }

    public LibPatcherArm64(string dotnetHome) : base(dotnetHome) { }

    protected override PatchData Patch_MethodAccessException() {
        return new("mono_method_can_access_method", 0x28, [
            // <start of mono_method_can_access_method_full>
            // ldrb w8, [x0, #0x20]
            0x1F, 0x20, 0x03, 0xD5, // (tst W8, #0x7C)              => (nop)
            0x1F, 0x20, 0x03, 0xD5, // (b.eq <jmp to continuation>) => (nop)
            // mov w0, #1
            // ret
        ]);
    }

    protected override PatchData Patch_FieldAccessException() {
        return new("mono_method_can_access_field", 0x130, [
            // <func end - 2>
            0x20, 0x00, 0x80, 0x52, // (mov w0, wzr) => (mov w0, #1)
            // ret
        ]);
    }
}