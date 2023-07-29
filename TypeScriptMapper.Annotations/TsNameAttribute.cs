namespace TypescriptMapper.Annotations;
 
public class TsNameAttribute : Attribute
{
    public string Name { get; set; }
    
    public TsNameAttribute() : base()
    {
        Name = "";
    }

    public TsNameAttribute(string name) : base()
    {
        Name = name;
    }

}