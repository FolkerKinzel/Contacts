namespace FolkerKinzel.Contacts.Intls;

internal ref struct ItemStripper
{
    private const char END_OF_STRING = '\0';
    private const int INITIAL_INDEX = -1;

    private readonly string _s = "";
    private readonly bool _caseSensitive;
    private int _currentIndex = INITIAL_INDEX;


    internal ItemStripper(string? s, bool caseSensitive = true)
    {
        _s = s ?? "";
        _caseSensitive = caseSensitive;
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => throw new InvalidOperationException();


    internal bool Equals(ItemStripper other)
    {
        ResetCurrentIndex();
        other.ResetCurrentIndex();

        char thisChar;
        do
        {
            thisChar = GetNextChar();

            if (!AreCharsEqual(thisChar, other.GetNextChar()))
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
            if (!_caseSensitive)
            {
                c = char.ToUpperInvariant(c);
            }

            hashCode ^= c.GetHashCode();

            c = GetNextChar();
        }

        return hashCode;
    }

    internal static int GetLength(string? input) => new ItemStripper(input).GetLength();
    

    internal static bool StartEqual(string? s1, string? s2, bool ignoreCase = false)
    {
        var strip1 = new ItemStripper(s1, !ignoreCase);
        var strip2 = new ItemStripper(s2, !ignoreCase);

        return strip1.GetLength() < strip2.GetLength() ? strip2.StartsWith(strip1)
                                                       : strip1.StartsWith(strip2);
    }

    internal static bool AreEqual(string? s1, string? s2, bool ignoreCase = false)
        => new ItemStripper(s1, !ignoreCase).Equals(new ItemStripper(s2, !ignoreCase));

    internal static bool IsEmpty(string? s) => new ItemStripper(s).IsEmpty();



    private int GetLength()
    {
        //ResetCurrentIndex();
        Debug.Assert(_currentIndex == INITIAL_INDEX);

        int length = 0;

        while (GetNextChar() != END_OF_STRING)
        {
            length++;
        }

        return length;
    }

    private bool IsEmpty()
    {
        //ResetCurrentIndex();
        Debug.Assert(_currentIndex == INITIAL_INDEX);
        return GetNextChar().Equals(END_OF_STRING);
    }


    private bool StartsWith(ItemStripper other)
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

            if (!AreCharsEqual(thisChar, otherChar))
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


    private bool AreCharsEqual(char c1, char c2) 
        => _caseSensitive ? c1.Equals(c2) : char.ToUpperInvariant(c1).Equals(char.ToUpperInvariant(c2));

}
