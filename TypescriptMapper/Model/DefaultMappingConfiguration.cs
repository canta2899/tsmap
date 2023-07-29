using TypescriptMapper.Annotations;

namespace TypescriptMapper;

public class DefaultMappingConfiguration : IMappingConfiguration
{
    public CasingPolicy CasingPolicy { get; set; } = CasingPolicy.CamelCase;
}