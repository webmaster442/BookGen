//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace BookGen.Framework.Scripts
{
    public class Compiler
    {
        private readonly List<PortableExecutableReference> _references;
        private readonly ILog _log;

        public void AddTypeReference<TType>()
        {
            string location = typeof(TType).GetTypeInfo().Assembly.Location;
            _references.Add(MetadataReference.CreateFromFile(location));
        }

        public Compiler(ILog log)
        {
            _references = new List<PortableExecutableReference>();
            _log = log;
            AddTypeReference<object>();
        }

        public Assembly? CompileToAssembly(IEnumerable<FsPath> files)
        {
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();

            foreach (var file in files)
            {
                string content = file.ReadFile(_log);
                SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(content);
                syntaxTrees.Add(tree);
            }


            CSharpCompilation compiler = CSharpCompilation.Create("scripts.dll")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
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
            }

            return null;
        }
    }
}