namespace ProSol.Html.Contracts.Data;

/// <summary>
/// Represents the html info about the tag.
/// </summary>
public record class TagInfo(string Name, ILookup<string, string> Attributes);