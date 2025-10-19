using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Extractor.Extractors
{
  public class BinaryDumper
  {
    public void Extract(string gameDataFolder, string outputFolderPath)
    {
      var allFiles = Directory.GetFiles(GetBinFilePath(gameDataFolder), "*.bin", SearchOption.AllDirectories);
      var outFiles = (string[])allFiles.Clone();
      for (var i = 0; i < outFiles.Length; i++)
      {
        var gameDataIndex = outFiles[i].LastIndexOf(Path.Combine("GameData", ""), StringComparison.OrdinalIgnoreCase);
        outFiles[i] = gameDataIndex >= 0 ? outFiles[i].Substring(gameDataIndex + 9) : Path.GetFileName(outFiles[i]);
      }

      for (var i = 0; i < allFiles.Length; i++)
      {
        DecryptBinFile(outputFolderPath, allFiles[i], outFiles[i]);
      }
    }

    private string GetBinFilePath(string gameDataFolder)
    {
      return gameDataFolder;
    }

    private string DecryptBinFile(string outputFolderPath, string binFile, string subdir)
    {
      var binFileWOE = Path.GetFileNameWithoutExtension(binFile);

      // Skip profanity as it has no value for us
      if (binFileWOE.StartsWith("profanity", StringComparison.OrdinalIgnoreCase))
      {
        return "";
      }

      var outSubdirs = Path.GetDirectoryName(Path.Combine(outputFolderPath, subdir));

      if (outSubdirs != "")
        Directory.CreateDirectory(outSubdirs);
      var finalOutPath = Path.Combine(outSubdirs, binFileWOE);
      var finalXmlPath = finalOutPath + ".xml";
      var finalJsonPath = finalOutPath + ".json";
      Console.Out.WriteLine("Extracting " + binFileWOE + ".bin... to:" + finalOutPath +".xml");

      using (var outputXmlFile = File.Create(finalXmlPath))
      {
        BinaryDecrypter.DecryptBinaryFile(binFile, outputXmlFile);
      }

      if (string.Equals("world", binFileWOE, StringComparison.OrdinalIgnoreCase) || (!subdir.StartsWith("cluster") && !subdir.StartsWith("templates")))
      {
        var xmlDocument = new XmlDocument();
        var xmlReaderSettings = new XmlReaderSettings
        {
          IgnoreComments = true
        };
        var xmlReader = XmlReader.Create(finalXmlPath, xmlReaderSettings);
        xmlDocument.Load(xmlReader);
        File.WriteAllText(finalJsonPath, JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, false));
      }

      return finalOutPath;
    }
  }
}
