using TypescriptMapper.Extensions;

namespace TypescriptMapper.Test;

public class CaseConversionTest
{
    [Fact]
    public void Converter_ShouldConvertToCamelCase_WhenPascaleCaseIsSupplied()
    {
        // arrange
        var pascaleCaseString = "MyEntryPoint";
        var camelCaseString = "";
        
        // act
        camelCaseString = pascaleCaseString.ToCamelCase();

        // assert
        Assert.Equal("myEntryPoint", camelCaseString);
    }

    [Fact]
    public void Convert_ShouldConvertToCamelCase_WhenStringHasUnderscores()
    {
        // arrange
        var inputString = "my_EntryPoint_Type";
        var camelCaseString = "";
        
        // act
        camelCaseString = inputString.ToCamelCase();
        
        // assert
        Assert.Equal("myEntryPointType", camelCaseString);
    }

    [Fact]
    public void Convert_ShouldConvertToCamelCase_WhenSnakeCaseIsProvided()
    {
        // arrange
        var inputString = "my_entry_point";
        var camelCaseString = "";
        
        // act 
        camelCaseString = inputString.ToCamelCase();
        
        // assert 
        Assert.Equal("myEntryPoint", camelCaseString);
    }
}