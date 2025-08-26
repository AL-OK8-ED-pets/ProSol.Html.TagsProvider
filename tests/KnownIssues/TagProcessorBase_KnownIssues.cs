using ProSol.Html.Contracts.Data;
using ProSol.Messaging;
using ProSol.Messaging.Translating;

namespace ProSol.Html.Tests.KnownIssues;

public class TagsProvider_KnownIssues
{
    private TagsProvider tagsProvider;
    private DataSubscriber<ProcessedTag> processedTagsListener;

    [SetUp]
    public void Setup()
    {
        tagsProvider = new();
        processedTagsListener = new();

        tagsProvider
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Subscribe(processedTagsListener);
    }

    [Test]
    /// <summary>
    /// It could be a frequent case later, 
    /// but now the engine is not ready.
    /// </summary>
    public void Process_Ignores_SelfClosingTag()
    {
        var html = "<br />";
        tagsProvider.Process(html);
        Assert.That(processedTagsListener.Messages, Has.Length.EqualTo(0));
    }
}