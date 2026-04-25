using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using OsmiumNucleus;


namespace OsmiumEditor;


public static class ScriptCompiler
{
    public static string ProgramName = "0";

    public static bool Success = false;
    
    //returns a stream from a newly compiled script
    public static MemoryStream CompileScripts() {
        

        List<SyntaxTree> scriptTrees = [];
        
        //todo: clean up add docs
        foreach (string file in Directory.GetFiles(Project.SourcePath, "*.cs", SearchOption.AllDirectories)) {
            
            Console.WriteLine(file);
            
            scriptTrees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(file)));
        }
        
        //todo: make it easy to create modules add some editor option
        Compilation compiledProgram = CSharpCompilation.Create("OsmiumProgram", scriptTrees, DependencyResolver.GetDependencies(), options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        
        //todo: temp
        MemoryStream stream = new MemoryStream();
        EmitResult result = compiledProgram.Emit(stream);

        Success = result.Success;
        
        if (!result.Success)
        {
            foreach (Diagnostic diag in result.Diagnostics)
            {
                Console.WriteLine(diag.ToString());
            }
        }
        else
        {
            Console.WriteLine("DLL compiled successfully!");
        }

        stream.Position = 0;
        
        return stream;
    }
}