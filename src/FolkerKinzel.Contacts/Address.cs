using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Adressdaten.
/// </summary>
public sealed class Address : Mergeable<Address>, ICleanable, ICloneable, IEquatable<Address?>
{
    #region Prop Enum
    /// <summary>
    /// Benannte Konstanten, um die Properties eines <see cref="Address"/>-Objekts im Indexer zu adressieren.
    /// </summary>
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

    /// <summary>
    /// Initialisiert eine leere Instanz der <see cref="Address"/>-Klasse.
    /// </summary>
    public Address()
    {

    }

    /// <summary>
    /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller seiner Unterobjekte.
    /// </summary>
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

    /// <summary>
    /// Straße (+ Hausnummer)
    /// </summary>
    public string? Street
    {
        get => Get(Prop.Street);
        set => Set(Prop.Street, value);
    }

    /// <summary>
    /// Ort
    /// </summary>
    public string? City
    {
        get => Get(Prop.City);
        set => Set(Prop.City, value);
    }

    /// <summary>
    /// Postleitzahl
    /// </summary>
    public string? PostalCode
    {
        get => Get(Prop.PostalCode);
        set => Set(Prop.PostalCode, value);
    }

    /// <summary>
    /// Bundesland
    /// </summary>
    public string? State
    {
        get => Get(Prop.State);
        set => Set(Prop.State, value);
    }

    /// <summary>
    /// Staat
    /// </summary>
    public string? Country
    {
        get => Get(Prop.Country);
        set => Set(Prop.Country, value);
    }


    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Address"/>-Objekts.
    /// </summary>
    /// <returns>Der Inhalt des <see cref="Address"/>-Objekts als <see cref="string"/>.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();

    #endregion

    #region Mergeable<T>, ICleanable

    /// <inheritdoc/>
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
            if (!Strip.AreEqual(postalCode, otherPostalCode))
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

    /// <inheritdoc/>
    protected override void CompleteDataWith(Address source)
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

    ///// <summary>
    ///// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält.
    ///// </summary>
    /// <inheritdoc/>
    public override bool IsEmpty => _propDic.Any(x => !Strip.IsEmpty(x.Value));

    ///// <summary>
    ///// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    ///// und leere Unterobjekte auf <c>null</c>.
    ///// </summary>
    /// <inheritdoc/>
    public override void Clean()
    {
        Prop[]? keys = this._propDic.Keys.ToArray();

        foreach (Prop key in keys)
        {
            Set(key, StringCleaner.CleanDataEntry(this._propDic[key]));
        }

#if !NET40 && !NET461 && !NETSTANDARD2_0
        _propDic.TrimExcess();
#endif
    }

    #endregion

    #endregion

    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Address(this);

    #endregion

    #region IEquatable

    ///// <summary>
    ///// Vergleicht die Instanz mit <paramref name="obj"/>,
    ///// um festzustellen, ob <paramref name="obj"/> ein <see cref="Address"/>-Objekt ist, das
    ///// dieselbe Postanschrift darstellt.
    ///// </summary>
    ///// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Address"/>-Objekt ist, das dieselbe Postanschrift darstellt.</returns>
    /// <inheritdoc/>
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



    ///// <summary>
    ///// Vergleicht die Instanz mit einem anderen <see cref="Address"/>-Objekt,
    ///// um festzustellen, ob <paramref name="other"/> eine identische Postanschrift ist.
    ///// </summary>
    ///// <param name="other">Das <see cref="Address"/>-Objekt, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Postanschrift darstellt.</returns>
    /// <inheritdoc/>
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

    ///// <summary>
    ///// Erzeugt einen Hashcode für das Objekt.
    ///// </summary>
    ///// <returns>Der Hashcode.</returns>
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hash = -1;

        ModifyHash(PostalCode);
        ModifyHash(City);
        ModifyHash(Street);

        return hash;

        ////////////////////////////////////////////////

        void ModifyHash(string? s)
        {
            hash ^= StringCleaner.PrepareForComparison(s).GetHashCode();
        }
    }


    #region Überladen von == und !=

    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Address"/>-Objekte, um zu überprüfen, ob sie gleich sind.
    /// </remarks>
    /// <param name="address1">Linker Operand.</param>
    /// <param name="address2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="address1"/> und <paramref name="address2"/> gleich sind.</returns>
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


    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Address"/>-Objekte, um zu überprüfen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="address1">Linker Operand.</param>
    /// <param name="address2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="address1"/> und <paramref name="address2"/> ungleich sind.</returns>
    public static bool operator !=(Address? address1, Address? address2) => !(address1 == address2);


    /// <summary>
    /// Vergleicht die Eigenschaften mit denen eines anderen <see cref="Address"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="Address"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Address other)
    {
        StringComparer comparer = StringComparer.Ordinal;

        return comparer.Equals(StringCleaner.PrepareForComparison(PostalCode), StringCleaner.PrepareForComparison(other.PostalCode))
            && comparer.Equals(StringCleaner.PrepareForComparison(City), StringCleaner.PrepareForComparison(other.City))
            && comparer.Equals(StringCleaner.PrepareForComparison(Street), StringCleaner.PrepareForComparison(other.Street))
            && comparer.Equals(StringCleaner.PrepareForComparison(State), StringCleaner.PrepareForComparison(other.State))
            && comparer.Equals(StringCleaner.PrepareForComparison(Country), StringCleaner.PrepareForComparison(other.Country));
    }


    #endregion

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
