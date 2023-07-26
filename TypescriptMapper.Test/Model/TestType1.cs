using TypescriptMapper.Attributes;

namespace TypescriptMapper.Test.Model;

[TsMap]
public class TestType1
{
    public string EntryOne { get; set; }
    
    public int EntryTwo { get; set; } 
    
    public string[] EntryThree { get; set; }
    
    public dynamic EntryFour { get; set; }
    
    [TsExclude]
    public string[] EntryFive { get; set; }
    
}