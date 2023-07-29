using TypescriptMapper.Annotations;
using TypescriptMapper.Model;

namespace TypescriptMapper;

public class DefaultMappingConfiguration : IMappingConfiguration
{
    public CasingPolicy CasingPolicy { get; set; } = CasingPolicy.CamelCase;
}