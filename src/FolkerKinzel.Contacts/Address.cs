using System.Text;
using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

    /// <summary>Encapsulates address data.</summary>
public sealed class Address : MergeableObject<Address>, ICleanable, ICloneable, IEquatable<Address?>
{
    #region Prop Enum
    /// <summary> Benannte Konstanten, um die Properties eines <see cref="Address" />-Objekts
    /// im Indexer zu adressieren. </summary>
    private enum Prop
    {
        Street,
        PostalCode,
        City,
        State,
        Country,

    }

    #endregion

    #region Private Fields

    private readonly Dictionary<Prop, string> _propDic = new();

    #endregion

    #region Constructors

    /// <summary>Initializes an empty instance of the <see cref="Address" /> class.</summary>
    public Address() { }


    /// <summary> Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller
    /// seiner Unterobjekte. </summary>
    /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
    private Address(Address source)
    {
        Debug.Assert(source != null);

        foreach (KeyValuePair<Prop, string> kvp in source._propDic)
        {
            this._propDic[kvp.Key] = kvp.Value;
        }
    }

    #endregion

    #region Accessor Methods

    private string? Get(Prop prop) => _propDic.ContainsKey(prop) ? (string?)_propDic[prop] : null;


    private void Set(Prop prop, string? value)
    {
        if (value is null)
        {
            _ = _propDic.Remove(prop);
        }
        else
        {
            _propDic[prop] = value;
        }
    }

    #endregion

    #region Public Properties and Methods

    /// <summary>Street (+ house number)</summary>
    public string? Street
    {
        get => Get(Prop.Street);
        set => Set(Prop.Street, value);
    }

    /// <summary>City</summary>
    public string? City
    {
        get => Get(Prop.City);
        set => Set(Prop.City, value);
    }

    /// <summary>Postal code</summary>
    public string? PostalCode
    {
        get => Get(Prop.PostalCode);
        set => Set(Prop.PostalCode, value);
    }

    /// <summary>State</summary>
    public string? State
    {
        get => Get(Prop.State);
        set => Set(Prop.State, value);
    }

    /// <summary>Country</summary>
    public string? Country
    {
        get => Get(Prop.Country);
        set => Set(Prop.Country, value);
    }


    /// <summary>Creates a <see cref="string" /> representation of the object instance.</summary>
    /// <returns>The content of the object instance as <see cref="string" />.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();

    #endregion

    #region Operators

    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="Address" /> objects to determine whether they
    /// are equal.</remarks>
    /// <param name="address1">Left operand.</param>
    /// <param name="address2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="address1" /> and <paramref name="address2"
    /// /> are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(Address? address1, Address? address2)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(address1, address2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (address1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return address2 is not null && address1.CompareBoolean(address2);
        }
    }


    /// <summary>Overloads the != operator.</summary>
    /// <remarks>Compares two <see cref="Address" /> objects to determine whether they
    /// are not equal.</remarks>
    /// <param name="address1">Left operand.</param>
    /// <param name="address2">Right operand.</param>
    /// <returns> <c>true</c> if <paramref name="address1" /> and <paramref name="address2"
    /// /> are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(Address? address1, Address? address2) => !(address1 == address2);

    #endregion

    #region MergeableObject<T>, ICleanable

    /// <inheritdoc />
    protected override bool DescribesForeignIdentity(Address other)
    {
        string? postalCode = PostalCode;
        string? otherPostalCode = other.PostalCode;

        if (Strip.IsEmpty(postalCode) || Strip.IsEmpty(otherPostalCode))
        {
            if (AreDifferent(City, other.City))
            {
                return true;
            }
        }
        else
        {
            if (!Strip.Equals(postalCode, otherPostalCode))
            {
                return true;
            }
        }

        if (AreDifferent(Street, other.Street))
        {
            return true;
        }

        if (AreDifferent(Country, other.Country))
        {
            return true;
        }

        if (AreDifferent(State, other.State))
        {
            return true;
        }

        return false;

        //////////////////////////////////////////////////////

        static bool AreDifferent(string? s1, string? s2)
            => !Strip.IsEmpty(s1) && !Strip.IsEmpty(s2) && !Strip.StartEqual(s1, s2, true);
    }


    /// <inheritdoc />
    protected override void SupplementWith(Address source)
    {
        if (Strip.IsEmpty(PostalCode))
        {
            PostalCode = source.PostalCode;
        }

        if (Strip.IsEmpty(City))
        {
            City = source.City;
        }

        if (Strip.IsEmpty(Street))
        {
            Street = source.Street;
        }

        if (Strip.IsEmpty(State))
        {
            State = source.State;
        }

        if (Strip.IsEmpty(Country))
        {
            Country = source.Country;
        }
    }

    #region ICleanable

    /// <inheritdoc />
    public override bool IsEmpty => !_propDic.Any(x => !Strip.IsEmpty(x.Value));


    /// <inheritdoc />
    public override void Clean()
    {
        Prop[]? keys = this._propDic.Keys.ToArray();

        foreach (Prop key in keys)
        {
            Set(key, StringCleaner.CleanDataEntry(this._propDic[key]));
        }

#if !NET462 && !NETSTANDARD2_0
        _propDic.TrimExcess();
#endif
    }

    #endregion

    #endregion

    #region ICloneable

    /// <summary>Creates a deep copy of the object instance.</summary>
    /// <returns>Deep copy of the object instance.</returns>
    public object Clone() => new Address(this);

    #endregion

    #region IEquatable

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        // If parameter cannot be cast to Address return false.
        if (obj is not Address p)
        {
            return false;
        }

        // Referenzgleichheit
        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(p);
    }


    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Address? other)
    {
        // If parameter is null return false:
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

    /// <summary> Vergleicht die Eigenschaften mit denen eines anderen <see cref="Address"
    /// />-Objekts. </summary>
    /// <param name="other">Das <see cref="Address" />-Objekt, mit dem verglichen wird.</param>
    /// <returns> <c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Address other)
    {
        StringComparer comparer = StringComparer.Ordinal;

        return comparer.Equals(StringCleaner.PrepareForComparison(PostalCode), StringCleaner.PrepareForComparison(other.PostalCode))
            && comparer.Equals(StringCleaner.PrepareForComparison(City), StringCleaner.PrepareForComparison(other.City))
            && comparer.Equals(StringCleaner.PrepareForComparison(Street), StringCleaner.PrepareForComparison(other.Street))
            && comparer.Equals(StringCleaner.PrepareForComparison(State), StringCleaner.PrepareForComparison(other.State))
            && comparer.Equals(StringCleaner.PrepareForComparison(Country), StringCleaner.PrepareForComparison(other.Country));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(
            StringCleaner.PrepareForComparison(PostalCode),
            StringCleaner.PrepareForComparison(City),
            StringCleaner.PrepareForComparison(Street));
    }

    #endregion

    #region internal

    internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
    {
        bool writeLineBreak = true;
        bool writeIndent = true;

        foreach (Prop key in _propDic.Keys.OrderBy(x => x))
        {
            switch (key)
            {
                case Prop.Street:
                    _ = sb.Append(indent).AppendLine(Street);
                    writeLineBreak = false;
                    break;
                case Prop.PostalCode:
                    _ = sb.Append(indent).Append(PostalCode);
                    writeLineBreak = true;
                    writeIndent = false;
                    break;
                case Prop.City:
                    if (writeIndent)
                    {
                        _ = sb.Append(indent).AppendLine(City);
                    }
                    else
                    {
                        _ = sb.Append(' ').AppendLine(City);
                        writeIndent = true;
                    }
                    writeLineBreak = false;
                    break;
                case Prop.State:
                    if (writeLineBreak)
                    {
                        _ = sb.AppendLine();
                    }
                    _ = sb.Append(indent).AppendLine(State);
                    writeLineBreak = false;
                    break;
                case Prop.Country:
                    if (writeLineBreak)
                    {
                        _ = sb.AppendLine();
                    }
                    _ = sb.Append(indent).AppendLine(Country);
                    writeLineBreak = false;
                    break;

                default:
                    break;
            }
        }

        if (!writeLineBreak)
        {
            sb.Length -= Environment.NewLine.Length;
        }

        return sb;
    }

    #endregion
}//class
