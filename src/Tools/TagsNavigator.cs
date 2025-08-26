namespace ProSol.Html.Tools;

internal static class TagsNavigator
{
    internal static int GetNextTagIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            -1 => html.Length,
            var nextTagIndex => nextTagIndex
        };

    internal static int GetInnerTextIndex(ReadOnlySpan<char> html)
        => html.IndexOf('<') switch
        {
            0 => GetInnerTextIndex(html[1..]) + 1,
            _ => html.IndexOf('>') + 1
        };

    internal static int SkipComment(ReadOnlySpan<char> html) 
        => (html.IndexOf('-'), html.IndexOf('>')) switch
        {   
            // HACK: it actually skips usefull chars right after the comment. 
            // It should return "->".Length. 
            // It's the design issue, unfortunately.
            (0, 1) => GetNextTagIndex(html), 
            (0, _) => 1 + SkipComment(html[1..]),
            (var x, _) => x + SkipComment(html[x..])
        };
}