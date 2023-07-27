namespace TypescriptMapper;

public interface IMappingConfiguration
{
    CasingPolicy CasingPolicy { get; }
    bool ExportAll { get; }
}