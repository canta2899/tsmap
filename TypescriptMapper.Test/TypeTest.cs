using TypescriptMapper.Extensions;
using TypescriptMapper.Test.Model;

namespace TypescriptMapper.Test;

public class TypeTest
{
    private IEnumerable<TestType1> DummyEnumerble()
    {
        yield return new TestType1();
        yield return new TestType1();
    }

    [Fact]
    public void IsCollection_ShouldBeTrue_WhenTypeImplementsICollection()
    {
        // arrange 
        var l = new List<string>();
        var isCollection = false;

        // act
        isCollection = l.GetType().IsCollection();

        // assert
        Assert.True(isCollection);
    }
    
    [Fact]
    public void IsCollection_ShouldBeTrue_WhenTypeImplementsIEnumerable()
    {
        // arrange 
        var l = DummyEnumerble();
        var isCollection = false;

        // act
        isCollection = l.GetType().IsCollection();

        // assert
        Assert.True(isCollection);
    }
}