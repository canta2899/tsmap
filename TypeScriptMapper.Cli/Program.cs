// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Text;
using TypescriptMapper;
using TypescriptMapper.Annotations;

var arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
    Console.WriteLine("Specify assembly path");
    Environment.Exit(1);
}

var config = new DefaultMappingConfiguration();

if (arguments.Contains("ignore-case"))
{
    config.CasingPolicy = CasingPolicy.Invariant;
}

var assemblyPath = Path.GetFullPath(arguments[1]);

var mapper = new Mapper(config);
using var sw = new StringWriter();
var assembly = Assembly.LoadFrom(assemblyPath);
mapper.MapAssembly(assembly, sw);
Console.WriteLine(sw.ToString());


