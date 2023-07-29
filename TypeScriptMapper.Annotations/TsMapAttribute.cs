namespace TypescriptMapper.Annotations;

public class TsMapAttribute : Attribute
{
    public CasingPolicy CasingPolicy { get; }
    public TsMapAttribute(CasingPolicy casingPolicy = CasingPolicy.CamelCase) : base()
    {
        CasingPolicy = casingPolicy;
    }
}