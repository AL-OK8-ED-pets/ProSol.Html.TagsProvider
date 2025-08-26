namespace ProSol.Html.Tools.Extracting;

internal sealed class AttributeValuesExtractor
{
    internal ReadOnlySpan<char> Extract(
        ReadOnlySpan<char> tagContent, 
        out string[] values)
    {
        values = [ string.Empty ];

        if (tagContent.IndexOf('=') is 0)
        {
            tagContent = GetValues(tagContent[1..], out values);
        }

        return tagContent;
    }

    private static ReadOnlySpan<char> GetValues(
        ReadOnlySpan<char> valuesString, 
        out string[] values)
    {
        var ranges = GetRanges(valuesString)
            .Where(x => x.End.Value != 0);

        values = [..GetStrings(valuesString, ranges)];
        return valuesString;
    }

    private static Range[] GetRanges(
        ReadOnlySpan<char> tagContent)
    {
        const int maxAttributeValuesCount = 100;
        Span<Range> rangesBuffer = stackalloc Range[maxAttributeValuesCount];
        return tagContent.Split(rangesBuffer, ' ') switch
        {
            0 => [0..tagContent.Length],
            _ => [..rangesBuffer]
        };
    }

    private static string[] GetStrings(
        ReadOnlySpan<char> tagContent, 
        IEnumerable<Range> ranges)
    {
        var result = new List<string>();
        foreach (var range in ranges)
        {
            var str = tagContent[range]
                .ToString()
                .Trim('\'','"');
            
            result.Add(str);
        }
        return [.. result];
    }
}