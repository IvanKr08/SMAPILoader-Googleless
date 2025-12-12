using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

partial class Program {
    static string[] args = Array.Empty<string>();
    static int argPos;

    public static bool Silent { get; private set; }

    public static void Main(string[] args) {
        Program.args = args;

        if (args.Length == 0) {
            Help();
            Console.Write("Enter action: ");

            var input = Console.ReadLine()!;
            Program.args = SplitString().Split(input);
            argPos = 0;
        }

        var act = GetArg("action");

        if (act == "silent") {
            Silent = true;
            act = GetArg("action");
        }

        try {
            switch (act) {
                case "revert": {
                    var home = GetArgOrDefault(@"C:\Program Files\dotnet\");

                    new LibPatcherArm64(home).Revert();
                    new LibPatcherArm32(home).Revert();
                    new LibPatcherX64(home).Revert();
                    new LibPatcherX86(home).Revert();

                    break;
                }
                case "patch": {
                    var home = GetArgOrDefault(@"C:\Program Files\dotnet\");

                    new LibPatcherArm64(home).Patch();
                    new LibPatcherArm32(home).Patch();
                    new LibPatcherX64(home).Patch();
                    new LibPatcherX86(home).Patch();

                    break;
                }
                case "help":
                    Help();
                    break;
                default:
                    Console.WriteLine("Unknown action");
                    Exit(-1);
                    break;
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Exit(-1);
        }

        Exit(0);
    }

    static void Help() {
        Console.Write("""
        Available actions:
            help                    : help
            silent <another action> : do not wait for user input
            patch  [dotnet home]    : patch all 8.0.22 runtime archs.
            revert [dotnet home]    : restore all 8.0.22 runtimes from backup.

        [dotnet home] is optional and defaults to "C:\Program Files\dotnet\"

        """);
    }

    [DoesNotReturn]
    static void Exit(int code) {
        if (!Silent) {
            Console.WriteLine($"Finished with code {code}. Press any key...");
            Console.ReadKey();
        }
        Environment.Exit(code);
    }

    static bool HasMoreArgs() {
        return args.Length > argPos;
    }
    static string GetArgOrDefault(string @default) {
        if (!HasMoreArgs()) return @default;
        return GetArg(null!);
    }
    static string GetArg(string name) {
        if (args.Length <= argPos) {
            Console.WriteLine($"No \"{name}\" argument provided");
            Exit(-1);
            return null;
        }
        return args[argPos++];
    }

    [GeneratedRegex(@"\s(?=(?:[^""]*""[^""]*"")*[^""]*$)")]
    private static partial Regex SplitString();
}

