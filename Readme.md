# ProSol.Html.TagsProvider

TagsProvider is a tool for extracting HTML tags from a string, in event-driven way.
Helps to extract text, structured data, from a specific site. 

## How to use?

Install the package:
```sh
dotnet add package ProSol.Html.TagsProvider --version 2.0.0-rc1.3

Fetch some html:
```csharp
var url = "https://en.wikipedia.org/wiki/Food_energy";
var html = HtmlSource.GetHtmlAsync(url).Result;
```

Process all `a` tag:
```csharp
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
```

```csharp
internal static class HtmlSource
{
    internal static async Task<string> GetHtmlAsync(string url)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}
```

That's it! 
The provider notifies about any tag met and its data: 
- name, 
- range of entire tag, 
- range of inner content.

More demos [here](https://git.disroot.org/alexenko/Demos/src/branch/master/ProSol.TagsProvider).

Happy coding!