namespace FolkerKinzel.Contacts;

internal struct PhoneStripper
{
    private readonly string _s;
    private int _currentIndex;

    internal PhoneStripper(string s)
    {
        this._s = s;
        _currentIndex = -1;
    }

    internal char GetNextChar()
    {
        while (++_currentIndex < _s.Length)
        {
            char c = _s[_currentIndex];

            if (char.IsLetterOrDigit(c))
            {
                return c;
            }
        }

        return '\0';
    }
}
