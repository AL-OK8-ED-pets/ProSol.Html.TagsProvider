using ProSol.Html.Messaging;
using ProSol.Messaging;
using ProSol.Html.Contracts.Data;
using ProSol.Messaging.Translating;

namespace ProSol.Html.Tests;

public class TagsProvider_MultipleTags_Tests
{
    [Test]
    public void Process_MultipleTags()
    {
        var html = $"<main> <div> <p>LoremIpsum</p> </div> </main>";
        
        var tagsProvider = new TagsProvider();
        var data = new DataSubscriber<ProcessedTag>();
        tagsProvider
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Subscribe(data);

        tagsProvider.Process(html);

        var result = data.Messages;

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[^1].TagLength, Is.EqualTo(html.Length));
            Assert.That(result[0].TagInfo.Name, Is.EqualTo("p"));
            Assert.That(result[1].TagInfo.Name, Is.EqualTo("div"));
            Assert.That(result[2].TagInfo.Name, Is.EqualTo("main"));
        });
    }

    [Test]
    public void Process_ShouldReturn_DeepestTag_Text()
    {
        var html = $"<main> <div> <p>LoremIpsum</p> </div> </main>";

        var tagsProvider = new TagsProvider();
        var data = new DataSubscriber<ProcessedTag>();
        tagsProvider
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Subscribe(data);

        tagsProvider.Process(html);

        var result = data.Messages;

        Assert.Multiple(() => 
        {
            Assert.That(result[0].TagInfo.Name, 
                Is.EqualTo("p"));

            Assert.That(result[0].TextLength, 
                Is.EqualTo("LoremIpsum".Length));
                
            Assert.That(html[result[0].InnerTextRange], 
                Is.EqualTo("LoremIpsum"));
        });
    }

    [Test]
    public void Process_Rootless_ShouldReturn_Tags()
    {
        var html = """
            <p> Foo <b>Bar</b> </p>
            <p> Foo <b>Bar</b> </p>
        """;

        var tagsProvider = new TagsProvider();
        var data = new DataSubscriber<ProcessedTag>();
        tagsProvider
            .Filter("b")
            .Translate<TagsProviderMessage, ProcessedTag>(x => x.CurrentTag)
            .Subscribe(data);

        tagsProvider.Process(html);

        var result = data.Messages;

        Assert.Multiple(() => 
        {
            Assert.That(result[0].TagInfo.Name, 
                Is.EqualTo("b"));

            Assert.That(html[result[0].InnerTextRange], 
                Is.EqualTo("Bar"));                

            Assert.That(result[1].TagInfo.Name, 
                Is.EqualTo("b"));

            Assert.That(html[result[1].InnerTextRange], 
                Is.EqualTo("Bar"));                
            
        });
    }
}