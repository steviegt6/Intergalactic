using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Intergalactic
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            string root;

            if (args.Length > 0)
                Console.WriteLine(root = args[0]);
            else
            {
                Console.WriteLine("Enter root folder:");
                root = Console.ReadLine()!;
            }

            if (root is null) // somehow??
                throw new Exception("how is this man");

            string command;
            string mode;

            if (args.Length >= 2)
                Console.WriteLine(command = args[1].ToLower());
            else
            {
                Console.WriteLine("Available commands: diff, patch, decompile, setup");
                command = Console.ReadLine()!.ToLower();
            }

            if (args.Length >= 3)
                Console.WriteLine(mode = args[2].ToLower());
            else
            {
                Console.WriteLine("Select mode: vanilla, modded");
                mode = Console.ReadLine()!.ToLower();

                if (mode is not "vanilla" or "modded")
                    throw new Exception("Invalid mode");
            }

            switch (command)
            {
                case "setup":
                    switch (mode)
                    {
                        case "vanilla":
                            string locationS;

                            if (args.Length >= 4)
                                Console.WriteLine(locationS = args[3]);
                            else
                            {
                                Console.WriteLine("Enter Outworlder location:");
                                locationS = Console.ReadLine();
                            }

                            // decompile
                            await Main(new[] {root, "decompile", "vanilla", locationS});

                            // copy decompiled files to "vanilla" folder
                            Directory.CreateDirectory(Path.Combine(root, "Vanilla"));
                            string[] dFiles = Directory.GetFiles(Path.Combine(root, "Decompiled"), "*.*", SearchOption.AllDirectories);
                            foreach (string file in dFiles)
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(file.Replace(Path.Combine(root, "Decompiled"), Path.Combine(root, "Vanilla")))!);
                                File.Copy(file, file.Replace(Path.Combine(root, "Decompiled"), Path.Combine(root, "Vanilla")), true);
                            }

                            // patch vanilla files
                            await Main(new[] {root, "patch", "vanilla", locationS});
                            Console.WriteLine("Finished vanilla set-up.");
                            break;

                        case "modded":
                            string locationSs;

                            if (args.Length >= 4)
                                Console.WriteLine(locationSs = args[3]);
                            else
                            {
                                Console.WriteLine("Enter Outworlder location:");
                                locationSs = Console.ReadLine();
                            }

                            // run vanilla set-up
                            await Main(new[] {root, "setup", "vanilla", locationSs});

                            // copy patched vanilla files to "modded" folder
                            Directory.CreateDirectory(Path.Combine(root, "Modded"));
                            string[] vFiles = Directory.GetFiles(Path.Combine(root, "Vanilla"), "*.*", SearchOption.AllDirectories);
                            foreach (string file in vFiles)
                                File.Copy(file, file.Replace(Path.GetDirectoryName(file)!, Path.Combine(root, "Modded")), true);

                            // patch modded files
                            await Main(new[] { root, "patch", "modded", locationSs });
                            Console.WriteLine("Finished modded set-up.");
                            break;
                    }

                    break;

                case "diff":
                    string sourceDiff = "";
                    string outputDiff = "";
                    string pathLoc = "";

                    switch (mode)
                    {
                        case "vanilla":
                            sourceDiff = "Decompiled";
                            outputDiff = "Vanilla";
                            pathLoc = Path.Combine("Patches", "Vanilla");
                            break;

                        case "modded":
                            sourceDiff = "Vanilla";
                            outputDiff = "Modded";
                            pathLoc = Path.Combine("Patches", "Modded");
                            break;
                    }

                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    static DirectoryInfo ToDi(string str) => new(str);

                    await new UnityDiffer().DiffFolders(ToDi(Path.Combine(root, sourceDiff)), ToDi(Path.Combine(root, outputDiff)), ToDi(Path.Combine(root, pathLoc)));
                    break;

                case "patch":
                    string destination = "";
                    string patches = "";

                    switch (mode)
                    {
                        case "vanilla":
                            destination = "Vanilla";
                            patches = Path.Combine("Patches", "Vanilla");
                            break;

                        case "modded":
                            destination = "Modded";
                            patches = Path.Combine("Patches", "Modded");
                            break;
                    }
                    
                    await ManagedPatcher.Program.Main(new[] {"patch", Path.Combine(root, destination), Path.Combine(root, patches)});
                    break;

                case "decompile":
                    string location;

                    if (args.Length >= 4)
                        Console.WriteLine(location = args[3]);
                    else
                    {
                        Console.WriteLine("Enter Outworlder location:");
                        location = Console.ReadLine();
                    }

                    await new UnityDecompiler().DecompileFile(location, Path.Combine(root, "Decompiled"));
                    break;

                default:
                    throw new Exception();
            }
        }
    }
}