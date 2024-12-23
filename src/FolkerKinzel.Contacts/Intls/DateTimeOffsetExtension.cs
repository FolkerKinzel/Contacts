namespace FolkerKinzel.Contacts.Intls;

internal static class DateTimeOffsetExtension
{
    internal static bool IsEmptyTimeStamp(this DateTimeOffset timeStamp) => timeStamp < new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero);
}
