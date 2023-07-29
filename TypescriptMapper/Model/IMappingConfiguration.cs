using TypescriptMapper.Annotations;
using TypescriptMapper.Model;

namespace TypescriptMapper;

public interface IMappingConfiguration
{
    CasingPolicy CasingPolicy { get; }
}