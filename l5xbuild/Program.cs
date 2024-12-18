using System.CommandLine;
using System.CommandLine.Parsing;
using YamlDotNet.Serialization;
using RockwellAutomation.LogixDesigner;
using RockwellAutomation.LogixDesigner.Logging;

namespace l5xbuild;

class Program
{
    const string BUILD_PATH = "build/";
    static async Task<int> Main(string[] args)
    {
        var manifestArgument = new Argument<FileInfo?>(
            name: "MANIFEST",
            description: "The file that contains the build manifest.");

        var rootCommand = new RootCommand("Utility to build L5X from source.");
        rootCommand.AddArgument(manifestArgument);

        rootCommand.SetHandler(async (manifestFile) => 
            { 
                await ExecuteBuild(manifestFile!); 
            },
            manifestArgument);

        return await rootCommand.InvokeAsync(args);
    }

    static async Task ExecuteBuild(FileInfo manifestFile)
    {
        // read in manifest
        var deserializer = new DeserializerBuilder().Build();
        using var sr = File.OpenText(manifestFile.FullName);
        Manifest manifest = deserializer.Deserialize<Manifest>(sr);
        Console.WriteLine("Loaded Manifest...");

        // create build directory if needed
        if(!Directory.Exists(BUILD_PATH)) Directory.CreateDirectory(BUILD_PATH);

        // Create Project
        string _logixFile = BUILD_PATH + manifest.ControllerName + ".ACD";
        // if existing project exists then delete
        if(File.Exists(_logixFile)) File.Delete(_logixFile);
        using LogixProject project = await LogixProject.CreateNewProjectAsync(
            _logixFile,
            manifest.MajorRevision, manifest.ProcessorTypeName, manifest.ControllerName, [new StdOutEventLogger()]);

        // Import Components
        foreach (Component item in manifest.Import)
        {
            await project.PartialImportFromXmlFileAsync(item.XPath, item.FilePath, LogixProject.ImportCollisionOptions.OverwriteOnColl);
        }

        // Export Components
        foreach (Component item in manifest.Export)
        {
            await project.PartialExportToXmlFileAsync(item.XPath, item.FilePath);
        }
    }
}