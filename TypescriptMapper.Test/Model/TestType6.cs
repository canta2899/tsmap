using TypescriptMapper.Annotations;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestType6
{
    public TestTypeGeneric1<bool> BoolGeneric { get; set; }
}