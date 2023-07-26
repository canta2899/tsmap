using TypescriptMapper.Attributes;

namespace TypeScriptMapper.Cli;

[TsMap]
public class Article 
{
    public string Description { get; set; }    
    public bool IsAvailable { get; set; }
    public IEnumerable<Price> Prices { get; set; }
}

[TsMap]
public class Price 
{
    public decimal Value { get; set; }
    public decimal Tax { get; set; } 
}

[TsMap]
public class Shop
{
    public string Name { get; set; }
    
    [TsExclude]
    public string SecretKey { get; set; }
    
    public Manager Manager { get; set; }
}

[TsMap]
public class Manager
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}

