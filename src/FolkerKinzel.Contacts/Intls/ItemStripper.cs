namespace FolkerKinzel.Contacts.Intls;

internal ref struct ItemStripper
{
    private const char END_OF_STRING = '\0';
    private const int INITIAL_INDEX = -1;

    private readonly string _s = "";
    private int? _length = null;
    private int _currentIndex = INITIAL_INDEX;


    internal ItemStripper(string? s) => _s = s ?? "";


    public bool Equals(ref ItemStripper other, bool ignoreCase)
    {
        ResetCurrentIndex();
        other.ResetCurrentIndex();

        char thisChar;
        do
        {
            thisChar = GetNextChar();

            if (!AreCharsEqual(thisChar, other.GetNextChar(), ignoreCase))
            {
                return false;
            }

        } while (thisChar != END_OF_STRING);

        return true;
    }


    public override int GetHashCode()
    {
        ResetCurrentIndex();
        int hashCode = string.Empty.GetHashCode();

        char c = GetNextChar();

        while (c != END_OF_STRING)
        {
            hashCode ^= c.GetHashCode();
        }

        return hashCode;
    }


    public int GetLength()
    {
        if (_length.HasValue)
        {
            return _length.Value;
        }

        _length = 0;
        int idx = -1;
        while (++idx < _s.Length)
        {
            char c = _s[idx];

            if (char.IsLetterOrDigit(c))
            {
                _length++;
            }
        }

        return _length.Value;
    }


    public bool StartsWith(ref ItemStripper other, bool ignoreCase)
    {
        ResetCurrentIndex();
        other.ResetCurrentIndex();

        while (true)
        {
            char otherChar = other.GetNextChar();
            if (otherChar == END_OF_STRING)
            {
                break;
            }

            char thisChar = GetNextChar();

            if(!AreCharsEqual(thisChar, otherChar, ignoreCase))
            {
                return false;
            }
        }

        return true;
    }


    private char GetNextChar()
    {
        while (++_currentIndex < _s.Length)
        {
            char c = _s[_currentIndex];

            if (char.IsLetterOrDigit(c))
            {
                return c;
            }
        }

        return END_OF_STRING;
    }


#if !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private void ResetCurrentIndex() => _currentIndex = INITIAL_INDEX;


    private static bool AreCharsEqual(char c1, char c2, bool ignoreCase) 
        => (ignoreCase && char.ToUpperInvariant(c1) == char.ToUpperInvariant(c2)) || c1.Equals(c2);

}
