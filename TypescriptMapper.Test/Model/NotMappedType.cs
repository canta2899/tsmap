using TypescriptMapper.Attributes;

namespace TypescriptMapper.Test.Model;

// misses the attribute so mapper won't include it
// [TsMap]
public class NotMappedType  
{
    public string Name { get; set; }
    
    [TsExclude]
    public IEnumerable<int> Entries { get; set; }
}