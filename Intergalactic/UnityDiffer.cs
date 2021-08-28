#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using Webmilio.Commons.Console;
using Webmilio.Commons.Extensions;

namespace Intergalactic
{
    public class UnityDiffer : IDiffer
    {
        public const string
            PatchExtension = ".patch",
            DeleteExtension = ".d",
            CreateExtension = ".c";

        public async Task DiffFolders(DirectoryInfo origin, DirectoryInfo updated, DirectoryInfo patches)
        {
            // ensure all folders exist
            origin.Create();
            updated.Create();
            patches.Create();

            Console.WriteLine($"Diffing {origin} to {updated} using {patches}.");

            string[] originalFiles = Directory.GetFiles(origin.FullName, "*.*", SearchOption.AllDirectories);
            string[] updatedFiles = Directory.GetFiles(updated.FullName, "*.*", SearchOption.AllDirectories);

            IList<string> toCreate, toDiff, toDelete;

            {
                List<string> strippedOriginal = SelectFilter(originalFiles, origin);
                List<string> strippedUpdated = SelectFilter(updatedFiles, updated);

                toDiff = strippedOriginal;
                toCreate = strippedUpdated.Where(su => !strippedOriginal.Any(so => so.Equals(su, StringComparison.OrdinalIgnoreCase))).ToArray();
                toDelete = strippedOriginal.Where(so => !strippedUpdated.Any(su => su.Equals(so, StringComparison.OrdinalIgnoreCase))).ToArray();
            }

            patches.Recreate(true);

            // List<Thread> threads = new(3);

            LineMatchedDiffer differ = new() { MaxMatchOffset = 10 };
            await toDiff.DoEnumerableAsync(p => Diff(differ, origin.FullName, updated.FullName, patches.FullName, p));
            await toCreate.DoAsync(p => WriteCreatePatch(updated.FullName, patches.FullName, p));
            await toDelete.DoAsync(p => WriteDeletePatch(patches.FullName, p));

            // threads.Do(t => t.Start());
            // threads.Do(t => t.Join());

            await Task.CompletedTask;
        }

        private List<string> SelectFilter(string[] collection, DirectoryInfo root)
        {
            List<string> items = new(collection.Length);

            collection.Do(i =>
            {
                string n = StripPath(i, root.FullName);

                if (n.StartsWith('.') || n.StartsWith("bin") || n.StartsWith("obj"))
                    return;

                items.Add(n);
            });

            return items;
        }

        private string StripPath(string path, string root)
        {
            return path.Remove(0, root.Length + 1);
        }


        private async Task Diff(Differ differ, string originalRoot, string destinationRoot, string patchRoot, string shortName)
        {
            try
            {
                Console.WriteLine($"Diff data: {originalRoot}, {destinationRoot}. {patchRoot}, {shortName}");

                string destinationPath = Path.Combine(destinationRoot, shortName);

                if (!File.Exists(destinationPath))
                    return;

                PatchFile diff = differ.DiffFile(Path.Combine(originalRoot, shortName), destinationPath, 3,
                    includePaths: false);

                if (!diff.IsEmpty)
                {
                    shortName += PatchExtension;
                    await WriteDiffPatch(patchRoot, shortName, diff.ToString());
                }
            }
            catch (Exception e) {
                Console.WriteLine($"Diff failed due to exception with {shortName}: {e}");
            }
        }


        private async Task WriteDiffPatch(string destRoot, string file, string content)
        {
            await WritePatch(Path.Combine(destRoot, file), file,
                async p => await File.WriteAllTextAsync(p, content));
        }

        private async Task WriteCreatePatch(string destRoot, string patchesRoot, string file)
        {
            await WritePatch(Path.Combine(destRoot, file), file,
                p => Task.Run(() => {
                    string newFilePath = Path.Combine(patchesRoot, $"{file}{PatchExtension}{CreateExtension}");
                    Directory.CreateDirectory(Path.GetDirectoryName(newFilePath)!);
                    File.Copy(p, newFilePath);
                }));
        }

        private async Task WriteDeletePatch(string patchesRoot, string file)
        {
            await WritePatch(Path.Combine(patchesRoot, file), file,
                _ => Task.Run(() => File.Create(Path.Combine(patchesRoot, $"{file}{PatchExtension}{DeleteExtension}")).Close()));
        }

        private static async Task WritePatch(string file, string displayPath, Func<string, Task> action)
        {
            Console.WriteLine("Creating patch {0}... ", displayPath);

            try
            {
                DirectoryInfo patch = new(Path.GetDirectoryName(file)!);
                patch.Create();

                await action(file);
            }
            catch (Exception e)
            {
                ConsoleHelper.WriteLineError("Failed creating patch for {0}:\n{1}.", displayPath, e);
            }
        }
    }
}