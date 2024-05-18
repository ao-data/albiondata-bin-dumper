using Extractor;
using Extractor.Extractors;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace CommandLine
{
  internal class Program
  {
    [Option(Description = "Export Type", ShortName = "t")]
    private ExportType ExportType { get; } = ExportType.Both;

    [Option(Description = "Export Mode", ShortName = "m")]
    private ExportMode ExportMode { get; } = ExportMode.Everything;

    [Option(Description = "Server Type", ShortName = "s")]
    private ServerType ServerType { get; } = ServerType.Live;

    [Required]
    [Option(Description = "Game Folder", ShortName = "d")]
    private string MainGameFolder { get; }

    [Required]
    [Option(Description = "Output Folder", ShortName = "o")]
    private string OutputFolderPath { get; }

    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called with reflection")]
    private void OnExecute()
    {
      RunExtractions();
    }

    public void RunExtractions()
    {
      Console.Out.WriteLine("#---- Starting Extraction Operation ----#");

      string exportTypeString;
      if (ExportType == ExportType.TextList)
      {
        exportTypeString = "Text List";
      }
      else if (ExportType == ExportType.Json)
      {
        exportTypeString = "JSON";
      }
      else
      {
        exportTypeString = "Text List and JSON";
      }

      string serverTypeString = "game"; // called game_x64 on my machine (linux)
      if (ServerType == ServerType.Staging)
      {
        serverTypeString = "staging";
      }
      if (ServerType == ServerType.Playground)
      {
        serverTypeString = "playground";
      }
      string mainGameFolderString = MainGameFolder + serverTypeString;
      mainGameFolderString = mainGameFolderString.Replace("'", "");

      var localizationData = new LocalizationData(mainGameFolderString, OutputFolderPath);

      switch (ExportMode)
      {
        case ExportMode.ItemExtraction:
          ExtractItems(mainGameFolderString, localizationData, exportTypeString);
          break;
        case ExportMode.LocationExtraction:
          ExtractLocations(mainGameFolderString, exportTypeString);
          break;
        case ExportMode.DumpAllXML:
          DumpAllXml(mainGameFolderString);
          break;
        case ExportMode.Everything:
          ExtractItems(mainGameFolderString, localizationData, exportTypeString);
          ExtractLocations(mainGameFolderString, exportTypeString);
          DumpAllXml(mainGameFolderString);
          break;
      }

      Console.Out.WriteLine("#---- Finished Extraction Operation ----#");
    }

    public void ExtractItems(string mainGameFolderString, LocalizationData localizationData, string exportTypeString)
    {
      Console.Out.WriteLine("--- Starting Extraction of Items (" + mainGameFolderString + ") as " + exportTypeString + " ---");
      new ItemExtractor(mainGameFolderString, OutputFolderPath, ExportMode, ExportType).Extract(localizationData);
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }

    public void ExtractLocations(string mainGameFolderString, string exportTypeString)
    {
      Console.Out.WriteLine("--- Starting Extraction of Locations (" + mainGameFolderString + ") as " + exportTypeString + " ---");
      new LocationExtractor(mainGameFolderString, OutputFolderPath, ExportMode, ExportType).Extract();
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }

    public void DumpAllXml(string mainGameFolderString)
    {
      Console.Out.WriteLine("--- Starting Extraction of All Files (" + mainGameFolderString + ") as XML from  ---");
      new BinaryDumper().Extract(mainGameFolderString, OutputFolderPath);
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }
  }
}
