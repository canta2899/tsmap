namespace TypescriptMapper.Test.Model;

public class TestType5
{
    public string? Nullable_String { get; set; }
    public int NonNullableInt { get; set; }
    public double? NullableDouble { get; set; }
    public DateTime? NullableDate { get; set; }
    public TestType1 Test1 { get; set; }
    public TestType1? Test2 { get; set; }
    public ICollection<TestType1> TestList1 { get; set; }
    public List<TestType1> TestList2 { get; set; }
}