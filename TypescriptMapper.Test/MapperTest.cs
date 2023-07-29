using System.Reflection;
using TypescriptMapper.Test.Model;

namespace TypescriptMapper.Test;

public class MapperTest 
{
    private const string _t = "  ";
    
    [Fact]
    public void Mapper_ShouldMapProperties_WhenNotExcluded()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            $"export interface TestType1 {{\n{_t}entryOne?: string;\n{_t}entryTwo?: number;\n{_t}entryThree?: string[];\n{_t}entryFour?: any;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType1>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapToAny_WhenCannotExpandToType()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            $"export interface TestType2 {{\n{_t}entryOne?: string;\n{_t}entryTwo?: any;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType2>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapGeneric_WhenTypeHasGeneric()
    {
       // arrange 
       Mapper mapper = new();
       var expected =
           $"export interface TestType3 {{\n{_t}value?: string;\n{_t}genericType?: TestTypeGeneric1<TestType2>;\n}}";
       
       // act
       var mappedType = mapper.Map<TestType3>();
       
       // assert
       Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapMultipleGeneric_WhenTypeHasMultipleGeneric()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            $"export interface TestType4 {{\n{_t}value?: string;\n{_t}genericType?: TestTypeGeneric2<TestType1, TestType2>;\n}}";

        // act
        var mappedType = mapper.Map<TestType4>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }
    
    [Fact]
    public void Mapper_ShouldMapToNullableField_WhenValueTypeIsNullable()
    {
        // arrange
        Mapper mapper = new();
        var expected =
            $"export interface TestType5 {{\n{_t}nullableString?: string;\n{_t}nonNullableInt?: number;" +
            $"\n{_t}nullableDouble?: number | null;\n{_t}nullableDate?: Date | null;" +
            $"\n{_t}test1?: TestType1;\n{_t}test2?: TestType1;\n{_t}testList1?: TestType1[];\n{_t}testList2?: TestType1[];\n}}";

        // act
        var mappedType = mapper.Map<TestType5>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapGeneric_WhenTheyHaveBaseType()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType6 {{\n{_t}boolGeneric?: TestTypeGeneric1<boolean>;\n}}";

        // act
        var mappedType = mapper.Map<TestType6>();

        // assert
        Assert.Equal(expected, mappedType);
    }
    
    [Fact]
    public void Mapper_ShouldMapPropertyType_WhenTypeIsExpandable()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType6 {{\n{_t}boolGeneric?: TestTypeGeneric1<boolean>;\n}}";

        // act
        var type = typeof(TestTypeGeneric1<bool>);
        var mappedType = mapper.Map<TestType6>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldRespectGenerics_WhenGenericTypeIsMapped()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType6 {{\n{_t}boolGeneric?: TestTypeGeneric1<boolean>;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType6>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldUseAny_WhenGenericTypeIsNotMapped()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType9 {{\n{_t}notMappedProp?: any;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType9>();
        
        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapGenericParameters_WhenSupplied()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestTypeGeneric1<T> {{\n{_t}entry?: T;\n}}";
        
        // act
        var mappedType = mapper.Map(typeof(TestTypeGeneric1<>));

        // assert
        Assert.Equal(expected, mappedType);
    }
    
    [Fact]
    public void Mapper_ShouldAddExtends_WhenClassInheritsFromAllowedModel()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType8 extends TestType7 {{\n{_t}newField?: string;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType8>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldMapEnum_WhenEnumIsProvided()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export enum TestEnum {{\n{_t}CaseOne,\n{_t}CaseTwo,\n}}";
        
        // act
        var mappedType = mapper.Map<TestEnum>();

        // assert
        Assert.Equal(expected, mappedType);
    }

    [Fact]
    public void Mapper_ShouldIgnoreInterfaces_WhenTypeImplementsAny()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType10 {{\n{_t}name?: string;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType10>();

        // assert
        Assert.Equal(expected, mappedType);
    }

     [Fact]
    public void Mapper_ShouldUseOverriddenNames_WhenTsNameIsSupplied()
    {
        // arrange
        Mapper mapper = new();
        var expected = $"export interface TestType11 {{\n{_t}customName?: string;\n{_t}lastname?: string;\n}}";
        
        // act
        var mappedType = mapper.Map<TestType11>();

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
        Assert.Contains("TestType6", foundTypes);
        Assert.Contains("TestTypeGeneric1", foundTypes);
        Assert.Contains("TestTypeGeneric2", foundTypes);
    }


    [Fact]
    public void Mapper_ShouldSkipType_WhenNotMarked()
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