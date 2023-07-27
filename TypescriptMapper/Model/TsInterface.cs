namespace TypescriptMapper;

public class TsInterface
{
    public string Name { get; set; }
    public string Extends { get; set; }
    public IEnumerable<TsInterfaceField> Fields { get; set; }
}