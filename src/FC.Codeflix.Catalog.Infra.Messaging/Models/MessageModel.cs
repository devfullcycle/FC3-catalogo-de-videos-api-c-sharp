namespace FC.Codeflix.Catalog.Infra.Messaging.Models;

public class MessageModel<T>
    where T: class
{
    public MessageModelPayload<T> Payload { get; set; } = null!;
}

public class MessageModelPayload<T>
    where T : class
{
    public T Before { get; set; } = null!;
    public T After { get; set; } = null!;
    public string Op { get; set; } = null!;
    
    public MessageModelOperation? Operation
        => Op switch
        {
            "c" => MessageModelOperation.Create,
            "u" => MessageModelOperation.Update,
            "d" => MessageModelOperation.Delete,
            "r" => MessageModelOperation.Read,
            _ => null
        };
}

public enum MessageModelOperation
{
    Create = 0,
    Update = 1,
    Delete = 2,
    Read = 3
}