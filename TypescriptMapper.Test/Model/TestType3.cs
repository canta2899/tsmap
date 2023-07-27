using TypescriptMapper.Annotations;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestType3
{
    public string Value { get; set; }
    public TestTypeGeneric1<TestType2> GenericType { get; set; }
}