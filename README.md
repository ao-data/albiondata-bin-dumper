# Albion Data Bin Dumper

A .NET 7.0 tool that extracts and converts Albion Online game data from encrypted `.bin` files into readable XML/JSON formats. It can extract item IDs, names, locations, and dump all game data files.

## Features

- **Item Extraction**: Extract item IDs, names, descriptions, and enchantment levels
- **Location Extraction**: Extract location IDs and display names  
- **Full Data Dump**: Convert all game data files to XML/JSON format
- **Localization Support**: Extract localized names and descriptions in multiple languages
- **Multiple Export Formats**: Support for text lists, JSON, or both formats
- **Cross-Platform**: Uses System.IO.Path for cross-platform compatibility

## Requirements

- .NET 7.0 Runtime (to run)
- .NET 7.0 SDK (to build from source)
- Albion Online game installation

## Installation

### Pre-built Binary
Download the latest release and run `CommandLine.exe` with the required parameters.

### Build from Source
```bash
git clone <repository-url>
cd albiondata-bin-dumper
dotnet build
```

## Usage

### Basic Usage
```bash
dotnet run -- -d "C:\Program Files\Albion Data Client" -o "C:\path\to\output"
```

### Command Line Options
```
Options:
  -t|--export-type <EXPORT_TYPE>                Export Type
                                                Allowed values are: TextList, Json, Both
  -m|--export-mode <EXPORT_MODE>                Export Mode
                                                Allowed values are: ItemExtraction, LocationExtraction, DumpAllXML,
                                                Everything
  -d|--game-data-folder <GAME_DATA_FOLDER>      GameData Folder
  -o|--output-folder-path <OUTPUT_FOLDER_PATH>  Output Folder
  -?|-h|--help                                  Show help information
```

### Examples

**Extract everything to JSON format on Windows for the Live server:**
```bash
dotnet run -- -d "C:\Program Files (x86)\AlbionOnline\game\Albion-Online_Data\StreamingAssets\GameData" -o "C:\output" -t Json -m Everything
```

**Extract everything to JSON format on Windows for the Staging server:**
```bash
dotnet run -- -d "C:\Program Files (x86)\AlbionOnline\staging\Albion-Online_Data\StreamingAssets\GameData" -o "C:\output" -t Json -m Everything
```

## Output Structure

The tool creates the following output structure:
```
output/
├── formatted/
│   ├── items.txt          # Item list (text format)
│   ├── items.json         # Item list (JSON format)
│   ├── world.txt          # Location list (text format)
│   └── world.json         # Location list (JSON format)
└── [game_data_files]/     # Raw XML/JSON dumps of all game data
    ├── items.xml
    ├── world.xml
    └── ...
```

## Notes

- All paths use cross-platform path separators for compatibility
- The tool requires access to the Albion Online game data files
