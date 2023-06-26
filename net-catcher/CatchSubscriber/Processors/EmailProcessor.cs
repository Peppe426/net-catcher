namespace CatchSubscriber.Processors;

public class EmailProcessor
{
    public List<(string EmailAddress, string? Name)> Copies { get; private set; } = new();

    internal EmailProcessor AddCopy((string emailAddress, string? name) copy)
    {
        Copies.Add(copy);

        return this;
    }
}