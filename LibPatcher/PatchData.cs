namespace LibPatcher;

record PatchData(string ExportFunctionName, int Offset, byte[] PatchBytes);
