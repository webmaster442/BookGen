//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace BookGen.Framework.Scripts
{
    /// <summary>
    /// Compiles C# script files into Assembly
    /// Compilation uses Roslyn API & the generated assembly is only in memory
    /// </summary>
    internal class Compiler
    {
        private readonly HashSet<PortableExecutableReference> _references;
        private readonly ILog _log;
        private readonly CSharpCompilationOptions _compilerOptions;

        public Compiler(ILog log)
        {
            _references = new HashSet<PortableExecutableReference>();
            _log = log;
            ReferenceNetStandard();
            AddTypeReference<object>();
            _compilerOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithPlatform(Platform.AnyCpu)
                .WithNullableContextOptions(NullableContextOptions.Enable)
                .WithOverflowChecks(true)
                .WithOptimizationLevel(OptimizationLevel.Release);
        }

        private void ReferenceNetStandard()
        {
            if (!(AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string trusted))
                throw new DependencyException("Can't locate Trusted platform assemblies");

            string[]? trustedAssembliesPaths = trusted.Split(Path.PathSeparator);

            string[]? neededAssemblies = new[]
            {
                "System.Runtime",
                "netstandard",
            };
            IEnumerable<PortableExecutableReference>? references = trustedAssembliesPaths
                .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p));

            foreach (PortableExecutableReference? reference in references)
            {
                _references.Add(reference);
            }
        }

        public void AddTypeReference<TType>()
        {
            string location = typeof(TType).GetTypeInfo().Assembly.Location;
            _references.Add(MetadataReference.CreateFromFile(location));
        }

        public IEnumerable<SyntaxTree> ParseToSyntaxTree(IEnumerable<FsPath> files)
        {
            foreach (FsPath? file in files)
            {
                string content = file.ReadFile(_log);
                SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
                yield return tree;
            }
        }

        public IEnumerable<SyntaxTree> ParseToSyntaxTree(string source)
        {
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(source, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
            yield return tree;
        }

        public Assembly? CompileToAssembly(IEnumerable<SyntaxTree> syntaxTrees)
        {
            string? timestamp = $"{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";
            CSharpCompilation compiler = CSharpCompilation.Create($"scripts_{timestamp}.dll")
                .WithOptions(_compilerOptions)
                .AddReferences(_references.ToArray())
                .AddSyntaxTrees(syntaxTrees);

            using (var compiled = new MemoryStream(32 * 1024))
            {
                EmitResult emitResult = compiler.Emit(compiled);
                if (emitResult.Success)
                {
                    compiled.Seek(0, SeekOrigin.Begin);
                    return AssemblyLoadContext.Default.LoadFromStream(compiled);
                }
                else
                {
                    _log.Warning("Error Compiling scripts. Use verbose log to see details");
                    string? details = string.Join('\n', emitResult.Diagnostics);
                    _log.Detail(details);
                }
            }

            return null;
        }
    }
}