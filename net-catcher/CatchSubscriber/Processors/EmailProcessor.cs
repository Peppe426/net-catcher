using FluentEmail.Core;

namespace CatchSubscriber.Processors;

public class EmailProcessor
{
    public Email Email { get; set; } = new Email();

    internal void AddCopys(List<(string emailAddress, string? name)> copys)
    {
        copys.ForEach(copy =>
        {
            Email.CC(copy.emailAddress, copy.name != null ? copy.name : "");
        });
    }
}