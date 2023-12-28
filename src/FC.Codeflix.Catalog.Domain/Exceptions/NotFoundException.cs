namespace FC.Codeflix.Catalog.Domain.Exceptions;
public class NotFoundException : BusinessRuleException
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
