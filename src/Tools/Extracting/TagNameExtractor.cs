namespace ProSol.Html.Tools.Extracting;

internal sealed class TagNameExtractor
{
    internal ReadOnlySpan<char> Extract(
        ReadOnlySpan<char> tagContent, 
        out string key) 
        {
            const string nameDelimeters = " =";
            key = tagContent.IndexOfAny(nameDelimeters) switch
            {
                -1 => tagContent.ToString(),
                var index => tagContent[..index].ToString()
            };
            return tagContent[key.Length..];
        } 
}