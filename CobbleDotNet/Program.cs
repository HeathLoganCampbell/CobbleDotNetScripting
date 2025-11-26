using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

// Path to the .csx script file (in scripts folder at solution level)
string scriptPath = Path.Combine("..", "..", "..", "..", "scripts", "script.csx");

async Task ExecuteScript()
{
    if (File.Exists(scriptPath))
    {
        try
        {
            // Read the script content
            string scriptContent = await File.ReadAllTextAsync(scriptPath);

            // Execute the script with default imports
            var options = ScriptOptions.Default
                .WithImports("System", "System.IO", "System.Collections.Generic", "System.Linq", "System.Text")
                .WithReferences(
                    typeof(Console).Assembly,
                    typeof(System.Linq.Enumerable).Assembly
                );

            var result = await CSharpScript.EvaluateAsync(scriptContent, options);

            Console.WriteLine("Script executed successfully!");
            if (result != null)
            {
                Console.WriteLine($"Result: {result}");
            }
        }
        catch (CompilationErrorException ex)
        {
            Console.WriteLine("Script compilation error:");
            Console.WriteLine(string.Join(Environment.NewLine, ex.Diagnostics));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine($"Script file not found: {scriptPath}");
    }
}

// Execute script initially
await ExecuteScript();

// Set up file watcher
var fullPath = Path.GetFullPath(scriptPath);
var directory = Path.GetDirectoryName(fullPath)!;
var fileName = Path.GetFileName(fullPath);

using var watcher = new FileSystemWatcher(directory)
{
    Filter = fileName,
    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
};

watcher.Changed += async (sender, e) =>
{
    Console.WriteLine($"\n--- File modified, re-running script ---");
    await ExecuteScript();
};

watcher.EnableRaisingEvents = true;

Console.WriteLine($"\nWatching for changes to {scriptPath}... Press Ctrl+C to exit.");
await Task.Delay(Timeout.Infinite);
