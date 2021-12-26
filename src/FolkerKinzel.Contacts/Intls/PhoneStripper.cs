using System.Diagnostics.CodeAnalysis;

namespace FolkerKinzel.Contacts.Intls;

internal struct PhoneStripper : IEquatable<PhoneStripper>
{
    private readonly string _s = "";
    private int _currentIndex;
    private const char END_OF_STRING = '\0';
    private const int INITAL_INDEX = -1;

    internal PhoneStripper(string? s)
    {
        this._s = s ?? "";
        _currentIndex = -1;
    }

    public bool Equals(PhoneStripper other)
    {
        char thisChar;
        do
        {
            thisChar = GetNextChar();

            if(thisChar != other.GetNextChar())
            {
                _currentIndex = INITAL_INDEX;
                return false;
            }

        } while (thisChar != END_OF_STRING);

        _currentIndex = INITAL_INDEX;
        return true;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if(obj is PhoneStripper ps)
        {
            return Equals(ps);
        }

        return false;
    }

    public override int GetHashCode()
    {
        int hashCode = string.Empty.GetHashCode();

        char c = GetNextChar();

        while (c != END_OF_STRING)
        {
            hashCode ^= c.GetHashCode();
        }

        _currentIndex = INITAL_INDEX;
        return hashCode;
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


}
