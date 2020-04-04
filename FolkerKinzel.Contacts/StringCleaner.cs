using System.Text.RegularExpressions;

namespace FolkerKinzel.Contacts
{
    internal static class StringCleaner
    {

        internal static string? CleanComment(string? val)
        {
            return string.IsNullOrWhiteSpace(val) ? null : val.Trim();
        }

        internal static string? CleanDataEntry(string? val)
        {
            return string.IsNullOrWhiteSpace(val) ? null : Regex.Replace(val.Trim(), @"\s+", " ", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
        }
    }
}
