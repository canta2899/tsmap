namespace TypescriptMapper;

internal class TypeMapException : Exception
{
    public TypeMapException()
    {
    }

    public TypeMapException(string message)
        : base(message)
    {
    }

    public TypeMapException(string message, Exception inner)
        : base(message, inner)
    {
    }
}