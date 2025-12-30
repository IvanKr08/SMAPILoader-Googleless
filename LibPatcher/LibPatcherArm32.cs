using LibPatcher;

class LibPatcherArm32 : LibPatcherBase<uint> { // Uses thumb mode!
    protected override string ArchName { get => "arm"; }

    public LibPatcherArm32(string dotnetHome) : base(dotnetHome) { }

    protected override PatchData Patch_MethodAccessException() {
        return new("mono_method_can_access_method", 0x2C, [
            // <start of mono_method_can_access_method_full>
            // ...
            // ldrbeq r1, [r4, #0x14]
            0x00, 0xBF, 0x00, 0xBF, // (tsteq.w r1, #0x7C)         => (nop) * 2
            0x00, 0xBF,             // (beq <jmp to continuation>) => (nop)
            // movs r0, #1
            // pop.w {r8, r9, r11}
            // pop {r4 - r7, pc}
        ]);
    }
    protected override PatchData Patch_FieldAccessException() {
        return new("mono_method_can_access_field", 0xD6, [
            // <func end - 2>
            0x01, 0x20, // (movs r0, #0) => (mov r0, #1)
            // <jump to func returner>
        ]);
    }
}
