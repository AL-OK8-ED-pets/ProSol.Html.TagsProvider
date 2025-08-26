using System.Collections.Immutable;

namespace ProSol.Html.Contracts.Data;

/// <summary>
/// Represents a push-notification from <see cref="TagsProvider"/>
/// </summary>
/// <param name="TagsHistory">Current branch of tags tree.</param>
/// <param name="CurrentTag">Current tag.</param>
public record class TagsProviderMessage(
    ImmutableArray<TagInfo> TagsHistory,
    ProcessedTag CurrentTag);