using TypescriptMapper.Annotations;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestType2
{
    public string EntryOne { get; set; }
    public TestType1 EntryTwo { get; set; } 
}