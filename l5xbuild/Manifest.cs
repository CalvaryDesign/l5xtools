namespace l5xbuild;

class Component
{
    public string FilePath { get; set; } = "";
    public string XPath { get; set; } = "";
}
class Manifest
{
    public uint MajorRevision { get; set; }
    public string ProcessorTypeName { get; set; } = "1756-L85E";
    public string ControllerName { get; set; } = "PLC";
    public Component[]? Import { get; set; }
    public Component[]? Export { get; set; }
    public override string ToString()
    {
        return MajorRevision.ToString();
    }
}