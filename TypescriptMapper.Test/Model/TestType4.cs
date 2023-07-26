using TypescriptMapper.Attributes;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestType4
{
    public string Value { get; set; }
    public TestTypeGeneric2<TestType1, TestType2> GenericType { get; set; }
}