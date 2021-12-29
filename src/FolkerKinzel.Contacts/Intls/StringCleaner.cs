using System.Text.RegularExpressions;

namespace FolkerKinzel.Contacts.Intls;

internal static class StringCleaner
{
    internal static string? CleanComment(string? val)
        => string.IsNullOrWhiteSpace(val) ? null : val.Trim();

    internal static string? CleanDataEntry(string? val)
        => Strip.IsEmpty(val)
                    ? null
                    : Regex.Replace(val.Trim(), @"\s+", " ", RegexOptions.Compiled
                                                           | RegexOptions.Singleline
                                                           | RegexOptions.CultureInvariant);

    internal static string PrepareForComparison(string? text) => text ?? string.Empty;

}
