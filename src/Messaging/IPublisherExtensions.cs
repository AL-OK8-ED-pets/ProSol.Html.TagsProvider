using ProSol.Html.Contracts.Data;
using ProSol.Messaging;
using ProSol.Messaging.Filtering;

namespace ProSol.Html.Messaging;

public static class IPublisherExtensions
{
    public static IPublisher<TagsProviderMessage> Filter(
        this IPublisher<TagsProviderMessage> publisher,
        string tagName)
        => publisher.Filter([tagName]);

    public static IPublisher<TagsProviderMessage> Filter(
        this IPublisher<TagsProviderMessage> publisher,
        params string[] tagNames)
         => publisher.Filter(x => tagNames.Contains(x.CurrentTag.TagInfo.Name));
}