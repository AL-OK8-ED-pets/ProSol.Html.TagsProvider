namespace ProSol.Html.Tools.Extracting;

internal static class AttributeExtractor
{
    internal static ILookup<string, string> EmptyAttributes 
        => Array.Empty<byte>()
            .ToLookup(x => string.Empty, x => string.Empty);

    internal static ILookup<string, string> GetAttributes(
        ReadOnlySpan<char> tagContent)
    {
        tagContent = tagContent.TrimEnd('/');

        var keyValues = new List<KeyValuePair<string, string>>();
        while (tagContent.IsEmpty is false)
        {
            tagContent = tagContent.TrimStart(' ')
                .GetKey(out var key)
                .GetValues(out var values);
            keyValues.AddRange(ToKeyValues(key, values));
        }

        return keyValues.ToLookup(x => x.Key, x => x.Value);
    }

    private static ReadOnlySpan<char> GetKey(
        this ReadOnlySpan<char> tagContent, 
        out ReadOnlySpan<char> key)
    {
        var result = new TagNameExtractor().Extract(tagContent, out var innerKey);
        key = innerKey;
        return result;
    }

    private static ReadOnlySpan<char> GetValues(
        this ReadOnlySpan<char> tagContent, 
        out string[] values)
    {
        var result = new AttributeValuesExtractor()
            .Extract(tagContent, out var innerValues);
        values = innerValues;
        return result;
    }

    private static KeyValuePair<string, string>[] ToKeyValues(
        ReadOnlySpan<char> key,
        string[] values)
    {
        var result = new List<KeyValuePair<string, string>>();
        foreach (var value in values)
        {
            result.Add(new(key.ToString(), value));
        }
        return [.. result];
    }
}