using ProSol.Html;
using ProSol.Messaging;
using ProSol.Messaging.Filtering;
using ProSol.Messaging.Translating;

// Get all links from the page:
var url = "https://en.wikipedia.org/wiki/Food_energy";
var html = HtmlSource.GetHtmlAsync(url).Result;

var provider = new TagsProvider();
var data = new DataSubscriber<string>();

provider
    .Endpoint(x => x.CurrentTag.TagInfo.Name == "a")
    .Translate(x => html[x.CurrentTag.InnerTextRange])
    .Subscribe(data);

provider.Process(html);

foreach (var item in data.Messages)
{
    Console.WriteLine(item);
}

Console.ReadKey();

internal static class HtmlSource
{
    internal static async Task<string> GetHtmlAsync(string url)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}