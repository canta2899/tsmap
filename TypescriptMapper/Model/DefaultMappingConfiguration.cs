namespace TypescriptMapper;

public class DefaultMappingConfiguration : IMappingConfiguration
{
    public CasingPolicy CasingPolicy { get; } = CasingPolicy.CamelCase;
    public bool ExportAll { get; } = true;
}