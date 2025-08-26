using ProSol.Html.Messaging;
using ProSol.Messaging.Filtering;
using ProSol.Messaging.Translating;
using ProSol.Html.Contracts.Data;
using ProSol.Messaging;

namespace ProSol.Html.Tests;

public class TagsProvider_FiltersSubscribers_Tests
{

    [Test]
    public void Process_Single_Named_Tag()
    {
        var html = """
                <ul> 
                    <li>Rock</li>
                    <li>Paper</li>
                    <li>Scissors</li>
                </ul> 
        """;
        var expected = new string[] { "Rock", "Paper", "Scissors" };

        var tagsProvider = new TagsProvider();
        var data = new DataSubscriber<string>();

        tagsProvider
            .Filter("li")
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Translate<ProcessedTag, string>(x => html[x.InnerTextRange])
            .Subscribe(data);

        tagsProvider.Process(html);
        
        Assert.That(data.Messages, Is.EquivalentTo(expected));
    }

    [Test]
    public void Process_Multiple_Named_Tag()
    {
        var html = """
            <b>Terminal</b> will be reused by <b>tasks</b>, press <i>any</i> key to <i>close</i> it. 
        """;

        var expected = new string[] { "Terminal", "tasks", "any", "close" };

        var tagsProvider = new TagsProvider();
        var data = new DataSubscriber<string>();

        tagsProvider
            .Filter("i", "b")
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Translate<ProcessedTag, string>(x => html[x.InnerTextRange])
            .Subscribe(data);

        tagsProvider.Process(html);

        Assert.That(data.Messages, Is.EquivalentTo(expected));
    }
}