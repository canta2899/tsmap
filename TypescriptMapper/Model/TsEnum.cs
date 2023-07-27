namespace TypescriptMapper;

public class TsEnum
{
    public string Name { get; set; }
    public IEnumerable<TsEnumField> Fields { get; set; }
}