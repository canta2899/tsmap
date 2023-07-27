using TypescriptMapper.Annotations;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestTypeGeneric1<T>
{
    public T Entry { get; set; }
}