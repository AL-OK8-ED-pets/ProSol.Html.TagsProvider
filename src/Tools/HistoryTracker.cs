using ProSol.Html.Contracts.Data;
using ProSol.Html.Data;
using ProSol.Html.Tools.Extracting;

namespace ProSol.Html.Tools;

internal class HistoryTracker
{
    private readonly Stack<UnprocessedTag> openedTags = new();

    public UnprocessedTag[] History => openedTags.Reverse().ToArray();

    public void Update(ReadOnlySpan<char> currentHtml, int offset)
    {
        switch (TagDetector.Detect(currentHtml))
        {
            case TagKind.Opening:
            {
                var tag = currentHtml.Clip("<", ">");
                openedTags.Push(new(
                    CreateTagInfo(tag),
                    TagOffset: offset,
                    InnerOffset: offset + tag.Length));
                break;
            }
            case TagKind.Closing:
            {
                if (openedTags.Any())
                {
                    // TODO: LOG: consider logging the case when the stack is empty, and tries to pop.
                    openedTags.Pop();
                }
                break;
            }
        }
    }

    static TagInfo CreateTagInfo(ReadOnlySpan<char> tag)
    {
        if (tag.IndexOf(' ') == -1)
        {
            var name = tag.Clip("<", ">", true);
            return new TagInfo(
                name.ToString(), 
                AttributeExtractor.EmptyAttributes);
        }
        else
        {
            var name = tag.Clip("<", " ", true);
            var attributes = tag.Clip(" ", ">", true);
            return new TagInfo(
                name.ToString(), 
                AttributeExtractor.GetAttributes(attributes));
        }
    }
}