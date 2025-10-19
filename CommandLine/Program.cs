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

    [Required]
    [Option(Description = "GameData Folder", ShortName = "d")]
    private string GameDataFolder { get; }

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

      string gameDataFolderString = GameDataFolder;
      gameDataFolderString = gameDataFolderString.Replace("'", "");

      var localizationData = new LocalizationData(gameDataFolderString, OutputFolderPath);

      switch (ExportMode)
      {
        case ExportMode.ItemExtraction:
          ExtractItems(gameDataFolderString, localizationData, exportTypeString);
          break;
        case ExportMode.LocationExtraction:
          ExtractLocations(gameDataFolderString, exportTypeString);
          break;
        case ExportMode.DumpAllXML:
          DumpAllXml(gameDataFolderString);
          break;
        case ExportMode.Everything:
          ExtractItems(gameDataFolderString, localizationData, exportTypeString);
          ExtractLocations(gameDataFolderString, exportTypeString);
          DumpAllXml(gameDataFolderString);
          break;
      }

      Console.Out.WriteLine("#---- Finished Extraction Operation ----#");
    }

    public void ExtractItems(string gameDataFolderString, LocalizationData localizationData, string exportTypeString)
    {
      Console.Out.WriteLine("--- Starting Extraction of Items (" + gameDataFolderString + ") as " + exportTypeString + " ---");
      new ItemExtractor(gameDataFolderString, OutputFolderPath, ExportMode, ExportType).Extract(localizationData);
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }

    public void ExtractLocations(string gameDataFolderString, string exportTypeString)
    {
      Console.Out.WriteLine("--- Starting Extraction of Locations (" + gameDataFolderString + ") as " + exportTypeString + " ---");
      new LocationExtractor(gameDataFolderString, OutputFolderPath, ExportMode, ExportType).Extract();
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }

    public void DumpAllXml(string gameDataFolderString)
    {
      Console.Out.WriteLine("--- Starting Extraction of All Files (" + gameDataFolderString + ") as XML from  ---");
      new BinaryDumper().Extract(gameDataFolderString, OutputFolderPath);
      Console.Out.WriteLine("--- Extraction Complete! ---");
    }
  }
}
