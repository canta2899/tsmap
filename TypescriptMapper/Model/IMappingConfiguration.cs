using TypescriptMapper.Annotations;

namespace TypescriptMapper;

public interface IMappingConfiguration
{
    CasingPolicy CasingPolicy { get; }
}