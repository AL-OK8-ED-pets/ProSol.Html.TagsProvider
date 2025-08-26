using ProSol.Html.Contracts.Data;

namespace ProSol.Html.Data;

/// <summary>
/// Represents an opened tag, when there is incomplete data about it.
/// </summary>
internal record class UnprocessedTag(
    TagInfo TagInfo,
    int TagOffset, 
    int? InnerOffset);