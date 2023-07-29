using TypescriptMapper.Annotations;

namespace TypescriptMapper.Test.Model;

public class TestType11
{
    [TsName("customName")]
    public string Name { get; set; }
    
    public string Lastname { get; set; }
}