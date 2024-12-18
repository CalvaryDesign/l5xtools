# l5xbuild tool

This tool is used to build .l5x files from many individula .l5x files. It takes a manifest.yaml as input that defines all the components to import and then exports the specified components to generate .l5x files with all dependencies contained.

1. Create a project
2. import all components from the `Import` array defined in the yaml
3. export all components from the `Export` array defined in the yaml

# Building

To build:
`dotnet build`

Note: The logix designer sdk NuGet package is expected to be located at `C:\Users\Public\Documents\Studio 5000\Logix Designer SDK\dotnet`. If the file is not there then change the location in `NuGet.Config`.

# Testing
To test
`.\bin\Debug\net8.0\l5xbuild .\test\manifest.yaml`

# Issue
Currently the import command keeps failing and is reporting:
`Unhandled exception: RockwellAutomation.LogixDesigner.OperationFailedException: XPath passed as argument is not valid: Controller/DataTypes/DataType[@Name='UDT_Top']. (build/PLC.L5X)`

