using TypescriptMapper.Attributes;

namespace TypescriptMapper;

public enum TsType
{
    [TsTypeName("Date")]
    Date,
    [TsTypeName("string")]
    String,
    [TsTypeName("number")]
    Number,
    [TsTypeName("bigint")]
    BigInteger,
    [TsTypeName("any")]
    Any,
    [TsTypeName("boolean")]
    Boolean,
    Object,
    Nullable,
    Generic,
    Array,
    Collection,
}