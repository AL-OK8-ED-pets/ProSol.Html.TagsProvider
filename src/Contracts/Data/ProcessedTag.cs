namespace ProSol.Html.Contracts.Data;

/// <summary>
/// Represents the completed tag with metadata and offsets from begin to end.
/// </summary>
public record class ProcessedTag(
    TagInfo TagInfo, 
    Range TagRange, 
    Range InnerTextRange)
{
    public int TagLength => TagRange.End.Value - TagRange.Start.Value;

    public int TextLength => InnerTextRange.End.Value - InnerTextRange.Start.Value;
}