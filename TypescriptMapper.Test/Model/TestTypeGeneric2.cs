using TypescriptMapper.Attributes;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestTypeGeneric2<T1, T2>
{
    public T1 Entry1 { get; set; } 
    public T2 Entry2 { get; set; }
}