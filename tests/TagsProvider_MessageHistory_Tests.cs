using System.Collections.Immutable;
using ProSol.Html;
using ProSol.Html.Contracts.Data;
using ProSol.Messaging;
using ProSol.Messaging.Translating;

namespace ProSol.Html.Tests;

public class TagsProvider_MessageHistory_Tests
{
    private TagsProvider tagsProvider;
    private DataSubscriber<string> tagNamesListener;

    [SetUp]
    public void Setup()
    {
        tagsProvider = new();
        tagNamesListener = new();
        tagsProvider
            .Translate<TagsProviderMessage, string>(x => x.CurrentTag.TagInfo.Name)
            .Subscribe(tagNamesListener);
    }

    [Test]
    public void Process_ShouldReturn_CorrectMessageOrder()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";
        tagsProvider.Process(html);

        var names = tagNamesListener.Messages;
        Assert.Multiple( () => {
            
            Assert.That(names[0], Is.EqualTo("p"));
            Assert.That(names[1], Is.EqualTo("div"));
            Assert.That(names[2], Is.EqualTo("main"));
        });
    }

    [Test]
    public void Process_FirstDeepestMessage_ShouldContain_CorrectHistory()
    {
        var text = "LoremIpsum";
        var html = $"<main> <div> <p>{text}</p> </div> </main>";

        var tagsHistoryListener = new DataSubscriber<ImmutableArray<TagInfo>>();
        tagsProvider
            .Translate<TagsProviderMessage, ImmutableArray<TagInfo>>(x => x.TagsHistory)
            .Subscribe(tagsHistoryListener);

        tagsProvider.Process(html);

        var names = tagNamesListener.Messages;

        Assert.That(names[0], Is.EqualTo("p"));

        Assert.Multiple( () => {
            var deepestHistory = tagsHistoryListener.Messages[0]
                .Select(x => x.Name)
                .ToArray();
            Assert.That(deepestHistory[0], Is.EqualTo("main"));
            Assert.That(deepestHistory[1], Is.EqualTo("div"));
            Assert.That(deepestHistory[2], Is.EqualTo("p"));
        });
    }
}