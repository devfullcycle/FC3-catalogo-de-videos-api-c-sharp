namespace FC.Codeflix.Catalog.Domain.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
