using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using FolkerKinzel.Contacts.Intls;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts;

    /// <summary>Encapsulates information about a phone number.</summary>
public sealed class PhoneNumber : MergeableObject<PhoneNumber>, ICleanable, ICloneable, IEquatable<PhoneNumber?>, IEnumerable<PhoneNumber>
{
    #region Flags Enum
    [Flags]
    private enum Flags
    {
        IsWork = 1,
        IsMobile = 2,
        IsFax = 4
    }
    #endregion

    #region private Fields

    private Flags _flags;

    #endregion

    #region Ctors

    /// <summary>Initializes an empty instance of the <see cref="PhoneNumber" /> class.</summary>
    public PhoneNumber() { }

    /// <summary>Initializes a <see cref="PhoneNumber" /> object with the phone number
    /// to be encapsulated and optional flags, that describe it in more detail.</summary>
    /// <param name="value">The phone number.</param>
    /// <param name="isWork"> <c>true</c> indicates, that it is a business phone number.</param>
    /// <param name="isMobile"> <c>true</c> indicates, that it is a mobile phone number.</param>
    /// <param name="isFax"> <c>true</c> indicates, that it is a fax number.</param>
    public PhoneNumber(string? value, bool isWork = false, bool isMobile = false, bool isFax = false)
    {
        this.Value = value;
        this.IsWork = isWork;
        this.IsMobile = isMobile;
        this.IsFax = isFax;
    }

    private PhoneNumber(PhoneNumber other)
    {
        this.Value = other.Value;
        this._flags = other._flags;
    }

    #endregion

    #region public Properties and Methods

    /// <summary>Phone number</summary>
    public string? Value { get; set; }

    /// <summary> <c>true</c> gibt an, dass es sich bei <see cref="Value" /> um eine
    /// dienstliche Telefonnummer handelt. </summary>
    public bool IsWork
    {
        get => (_flags & Flags.IsWork) == Flags.IsWork;
        set => _flags = value ? _flags | Flags.IsWork : _flags & ~Flags.IsWork;
    }

    /// <summary> <c>true</c> gibt an, dass es sich bei <see cref="Value" /> um eine
    /// Mobilfunknummer handelt. </summary>
    public bool IsMobile
    {
        get => (_flags & Flags.IsMobile) == Flags.IsMobile;
        set => _flags = value ? _flags | Flags.IsMobile : _flags & ~Flags.IsMobile;
    }

    /// <summary> <c>true</c> gibt an, dass <see cref="Value" /> eine Telefonnummer
    /// ist, die für den Faxempfang geeignet ist. </summary>
    public bool IsFax
    {
        get => (_flags & Flags.IsFax) == Flags.IsFax;
        set => _flags = value ? _flags | Flags.IsFax : _flags & ~Flags.IsFax;
    }


    /// <summary>Creates a <see cref="string" /> representation of the object instance.</summary>
    /// <returns>The content of the object instance as <see cref="string" />.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();

    #endregion

    #region Operators

    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="PhoneNumber" /> objects to determine whether
    /// they are equal.</remarks>
    /// <param name="phone1">Left operand.</param>
    /// <param name="phone2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="phone1" /> and <paramref name="phone2"
    /// /> are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(PhoneNumber? phone1, PhoneNumber? phone2)
    {
        // If both are null, or both are same instance, return true.
        if (object.ReferenceEquals(phone1, phone2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (phone1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return phone2 is not null && phone1.CompareBoolean(phone2);
        }
    }


    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="PhoneNumber" /> objects to determine whether
    /// they are not equal.</remarks>
    /// <param name="phone1">Left operand.</param>
    /// <param name="phone2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="phone1" /> and <paramref name="phone2"
    /// /> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(PhoneNumber? phone1, PhoneNumber? phone2) => !(phone1 == phone2);

    #endregion

    #region MergeableObject<T>, ICleanable

    /// <inheritdoc />
    protected override bool DescribesForeignIdentity(PhoneNumber other) => !Strip.Equals(Value, other.Value);


    /// <inheritdoc />
    protected override void SupplementWith(PhoneNumber source)
    {
        if (IsEmpty)
        {
            Value = source.Value;
        }

        this._flags |= source._flags;
    }

    #region ICleanable

    /// <inheritdoc />
    public override bool IsEmpty => Strip.IsEmpty(this.Value);


    /// <inheritdoc />
    public override void Clean() => this.Value = StringCleaner.CleanDataEntry(this.Value);

    #endregion
    #endregion

    #region IEnumerable

    /// <summary>Returns an enumerator, which returns the executing instance of the
    /// <see cref="PhoneNumber" /> class.</summary>
    /// <returns>An enumerator, which returns the executing instance of the <see cref="PhoneNumber"
    /// /> class.</returns>
    /// <remarks>This implementation allows to pass a single <see cref="PhoneNumber"
    /// /> object as an argument to a method parameter of type <see cref="IEnumerable{T}">IEnumerable&lt;PhoneNumber&gt;</see>.</remarks>
    IEnumerator<PhoneNumber> IEnumerable<PhoneNumber>.GetEnumerator()
    {
        yield return this;
    }

    /// <summary>Returns an enumerator, which returns the executing instance of the
    /// <see cref="PhoneNumber" /> class.</summary>
    /// <returns>An enumerator, which returns the executing instance of the <see cref="PhoneNumber"
    /// /> class.</returns>
    /// <remarks>This implementation allows to pass a single <see cref="PhoneNumber"
    /// /> object as an argument to a method parameter of type <see cref="IEnumerable"
    /// />.</remarks>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PhoneNumber>)this).GetEnumerator();

    #endregion

    #region ICloneable

    /// <summary>Creates a deep copy of the object instance.</summary>
    /// <returns>Deep copy of the object instance.</returns>
    public object Clone() => new PhoneNumber(this);

    #endregion

    #region IEquatable

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not PhoneNumber p)
        {
            return false;
        }

        return Equals(p);
    }


    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] PhoneNumber? other)
    {
        if (other is null)
        {
            return false;
        }

        // Referenzgleichheit
        if (object.ReferenceEquals(this, other))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(other);
    }


    /// <summary> Vergleicht die Eigenschaften mit denen eines anderen <see cref="PhoneNumber"
    /// />-Objekts. </summary>
    /// <param name="other">Das <see cref="PhoneNumber" />-Objekt, mit dem verglichen
    /// wird.</param>
    /// <returns> <c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(PhoneNumber other)
        => (IsEmpty && other.IsEmpty)
        || (StringComparer.Ordinal.Equals(Value, other.Value) && _flags == other._flags);


    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Strip.GetHashCode(Value), _flags);

    #endregion

    #region internal

    internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
    {
        _ = sb.Append(indent);
        if (string.IsNullOrWhiteSpace(Value))
        {
            _ = sb.Append('_');
            return sb;
        }
        else
        {
            _ = sb.Append(Value);
        }

        bool closeBracket = false;

        if (IsFax)
        {
            _ = sb.Append(" (").Append(Res.Fax);
            closeBracket = true;
        }

        if (IsWork)
        {
            if (closeBracket)
            {
                _ = sb.Append(", ").Append(Res.WorkShort);
            }
            else
            {
                _ = sb.Append(" (").Append(Res.WorkShort);
                closeBracket = true;
            }
        }

        if (closeBracket)
        {
            _ = sb.Append(')');
        }

        return sb;
    }
    #endregion

}
