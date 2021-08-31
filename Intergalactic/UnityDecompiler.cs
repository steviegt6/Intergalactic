#region License
// Pending license, modified from Assaturers/Assatur (https://github.com/Assaturers/Assatur)
#endregion

using System;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.ProjectDecompiler;
using ICSharpCode.Decompiler.Metadata;
using ModdingToolkit.Magicka.Decompiling;

namespace Intergalactic
{
    public class UnityDecompiler : IDecompiler
    {
        public Task DecompileFile(string from, string to)
        {
            Directory.CreateDirectory(to);

            Console.WriteLine($"Decompiling from {from} to {to}.");

            using PEFile module = new(from);
            UniversalAssemblyResolver resolver = new(from, false, module.Reader.DetectTargetFrameworkId());

            WholeProjectDecompiler decompiler = new(GetSettings(module), resolver, resolver, null);
            decompiler.DecompileProject(module, to);

            return Task.CompletedTask;
        }

        internal static DecompilerSettings GetSettings(PEFile module)
        {
            return new(LanguageVersion.CSharp7_3)
            {
                RemoveDeadCode = true,
                RemoveDeadStores = true,

                Ranges = false,

                ThrowOnAssemblyResolveErrors = false,
                UseSdkStyleProjectFormat = WholeProjectDecompiler.CanUseSdkStyleProjectFormat(module),
            };
        }
    }
}