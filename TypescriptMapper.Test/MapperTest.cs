using System.Reflection;
using TypescriptMapper.Test.Model;

namespace TypescriptMapper.Test;

public class MapperTest 
{
    [Fact]
    public void Mapper_ShouldMapProperties_WhenNotExcluded()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            "export interface TestType1 {\n\tentryOne?: string;\n\tentryTwo?: number;\n\tentryThree?: string[];\n\tentryFour?: any;\n}";
        
        // act
        var mappedType = mapper.MapInterface<TestType1>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapClassName_WhenPropertyOfTypeClassExists()
    {
        
        // arrange
        Mapper mapper = new();
        var expected =
            "export interface TestType2 {\n\tentryOne?: string;\n\tentryTwo?: TestType1;\n}";
        
        // act
        var mappedType = mapper.MapInterface<TestType2>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapGeneric_WhenTypeHasGeneric()
    {
       // arrange 
       Mapper mapper = new();
       var expected =
           "export interface TestType3 {\n\tvalue?: string;\n\tgenericType?: TestTypeGeneric1<TestType2>;\n}";
       
       // act
       var mappedType = mapper.MapInterface<TestType3>();
       
       // assert
       Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapMultipleGeneric_WhenTypeHasMultipleGeneric()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            "export interface TestType4 {\n\tvalue?: string;\n\tgenericType?: TestTypeGeneric2<TestType1, TestType2>;\n}";

        // act
        var mappedType = mapper.MapInterface<TestType4>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }
    
    [Fact]
    public void Mapper_ShouldMapToNullableField_WhenValueTypeIsNullable()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            "export interface TestType5 {\n\tnullableString?: string;\n\tnonNullableInt?: number;" +
            "\n\tnullableDouble?: number | null;\n\tnullableDate?: Date | null;" +
            "\n\ttest1?: TestType1;\n\ttest2?: TestType1;\n\ttestList1?: []TestType1;\n\ttestlist2?: []TestType1;}";

        // act
        var mappedType = mapper.MapInterface<TestType5>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldCreateType_WhenTypeIsRequested()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            "export type TestType5 = {\n\tnullableString?: string;\n\tnonNullableInt?: number;" +
            "\n\tnullableDouble?: number | null;\n\tnullableDate?: Date | null;" +
            "\n\ttest1?: TestType1;\n\ttest2?: TestType1;\n\ttestList1?: []TestType1;\n\ttestlist2?: []TestType1;}";
        
        // act
        var mappedType = mapper.MapType<TestType5>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldReturnMarkedTypes_WhenScanningAssembly()
    {
        // arrange
        Mapper mapper = new();
        var assembly = Assembly.GetAssembly(typeof(MapperTest));
        IEnumerable<string> foundTypes = new List<string>();
        
        // act
        foundTypes = mapper.GetMappableTypes(assembly).Select(x => 
            x.IsGenericType ? RemoveGenericName(x.Name) : x.Name);
        
        // assert
        Assert.Contains("TestType1", foundTypes);
        Assert.Contains("TestType2", foundTypes);
        Assert.Contains("TestType3", foundTypes);
        Assert.Contains("TestType4", foundTypes);
        Assert.Contains("TestTypeGeneric1", foundTypes);
        Assert.Contains("TestTypeGeneric2", foundTypes);
    }

    [Fact]
    public void Mapper_ShouldSkipType_WhenNotMaked()
    {
        // arrange
        Mapper mapper = new();
        var assembly = Assembly.GetAssembly(typeof(MapperTest));
        var hasFoundType = true;
        var notMappedTypeName = typeof(NotMappedType).Name;
        
        // act
        var types = mapper.GetMappableTypes(assembly);
        hasFoundType = types.Any(x => x.Name == notMappedTypeName);

        // assert
        Assert.False(hasFoundType);
    }
    
    private string RemoveGenericName(string name) => name.Split("`")[0];
    
}