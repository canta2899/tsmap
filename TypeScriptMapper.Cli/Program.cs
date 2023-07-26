// See https://aka.ms/new-console-template for more information

using System.Reflection;
using TypescriptMapper;

var mapper = new Mapper();
var mappableTypes = mapper.GetMappableTypes(Assembly.GetAssembly(typeof(Program)));

foreach (var t in mappableTypes)
{
    Console.WriteLine(mapper.MapInterface(t));
    Console.WriteLine();
}

