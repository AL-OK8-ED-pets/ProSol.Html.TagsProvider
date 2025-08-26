using ProSol.Html.Contracts.Data;
using ProSol.Html.Data;
using ProSol.Messaging;

namespace ProSol.Html;

/// <summary>
/// Processes the html input, provides push-notifications when html tag met for <see cref="ISubscriber<TagsProviderMessage"/>.
/// </summary>
/// <remarks>
/// Push-notification happens only when the closing tag met, so it contains the full data on tag.
/// </remarks>
public class TagsProvider : PipelineMessagePublisher<TagsProviderMessage>
{
    readonly HistoryTracker historyTracker = new();

    public void Process(ReadOnlySpan<char> html)
    {
        var charsProcessed = TagsNavigator.GetNextTagIndex(html);
        do
        {
            var currentHtml = html[charsProcessed..];
            Process(currentHtml, charsProcessed);
            historyTracker.Update(currentHtml, charsProcessed);
            charsProcessed += Proceed(currentHtml);
        } while (charsProcessed < html.Length);

        base.Complete();
    }

    void Process(ReadOnlySpan<char> currentHtml, int charsProcessed)
    {
        if (TagDetector.Detect(currentHtml) != TagKind.Closing)
        {
            return;
        }

        var latestTag = historyTracker.History.LastOrDefault();
        if (latestTag == null)
        {
            // TODO: log a warning here!
            return;
        }

        var tag = currentHtml.Clip("<", ">");
        var tagLength = charsProcessed + tag.Length;
        var range = latestTag.TagOffset..tagLength;
        var innerRange = latestTag.InnerOffset switch 
            {
                null => 0..0,
                var o => o.Value..charsProcessed
            };
        var processedTag = new ProcessedTag(latestTag.TagInfo, range, innerRange);

        OnProcessedTagMet(processedTag);
    }

    void OnProcessedTagMet(ProcessedTag value)
    {
        var history = historyTracker
            .History
            .Select(x => x.TagInfo);

        var message = new TagsProviderMessage(
            [..history], 
            value);

        base.Publish(message);
    }

    static int Proceed(ReadOnlySpan<char> currentHtml)
        => TagDetector.Detect(currentHtml) switch
        {
            TagKind.Comment => TagsNavigator.SkipComment(currentHtml),
            _ => 1 + TagsNavigator.GetNextTagIndex(currentHtml[1..])
        };
}
