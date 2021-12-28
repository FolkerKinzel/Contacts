using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FolkerKinzel.Contacts.Intls;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Informationen über eine Telefonnummer.
/// </summary>
public sealed class PhoneNumber : ICleanable, ICloneable, IEquatable<PhoneNumber?>, IEnumerable<PhoneNumber>, IIdentityComparer<PhoneNumber>
{
    [Flags]
    private enum Flags
    {
        IsWork = 1,
        IsMobile = 2,
        IsFax = 4
    }

    #region private Fields

    private Flags _flags;

    #endregion


    #region Ctors

    /// <summary>
    /// Initialisiert ein leeres <see cref="PhoneNumber"/>-Objekt.
    /// </summary>
    public PhoneNumber() { }

    /// <summary>
    /// Initialisiert ein <see cref="PhoneNumber"/>-Objekt mit der zu kapselnden Telefonnummer
    /// und optionalen Flags, die diese näher beschreiben.
    /// </summary>
    /// <param name="value">Die Telefonnummer.</param>
    /// <param name="isWork"><c>true</c> gibt an, dass es sich um eine dienstliche Telefonnummer handelt.</param>
    /// <param name="isMobile"><c>true</c> gibt an, dass es sich um eine Mobilfunknummer handelt.</param>
    /// <param name="isFax"><c>true</c> gibt an, dass die Nummer für den Fax-Empfang geeignet ist.</param>
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

    internal void Merge(PhoneNumber telNumber) => this._flags |= telNumber._flags;


    #region public Properties and Methods

    /// <summary>
    /// Telefonnummer
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// <c>true</c> gibt an, dass es sich bei <see cref="Value"/> um eine dienstliche Telefonnummer handelt.
    /// </summary>
    public bool IsWork
    {
        get => (_flags & Flags.IsWork) == Flags.IsWork;
        set => _flags = value ? _flags | Flags.IsWork : _flags & ~Flags.IsWork;
    }

    /// <summary>
    /// <c>true</c> gibt an, dass es sich bei <see cref="Value"/> um eine Mobilfunknummer handelt.
    /// </summary>
    public bool IsMobile
    {
        get => (_flags & Flags.IsMobile) == Flags.IsMobile;
        set => _flags = value ? _flags | Flags.IsMobile : _flags & ~Flags.IsMobile;
    }

    /// <summary>
    /// <c>true</c> gibt an, dass <see cref="Value"/> eine Telefonnummer ist, die für den Faxempfang geeignet ist.
    /// </summary>
    public bool IsFax
    {
        get => (_flags & Flags.IsFax) == Flags.IsFax;
        set => _flags = value ? _flags | Flags.IsFax : _flags & ~Flags.IsFax;
    }


    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="PhoneNumber"/>-Objekts.
    /// </summary>
    /// <returns>Der Inhalt des <see cref="PhoneNumber"/>-Objekts als <see cref="string"/>.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();


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


    #region Interfaces

    #region IEnumerable

    /// <inheritdoc/>
    IEnumerator<PhoneNumber> IEnumerable<PhoneNumber>.GetEnumerator()
    {
        yield return this;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<PhoneNumber>)this).GetEnumerator();

    #endregion

    #region ICleanable

    /// <summary>
    /// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält.
    /// </summary>
    public bool IsEmpty => ItemStripper.IsEmpty(this.Value);


    /// <summary>
    /// Entfernt leere Strings und überflüssige Leerzeichen.
    /// </summary>
    public void Clean() => this.Value = StringCleaner.CleanDataEntry(this.Value);


    #endregion


    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new PhoneNumber(this);

    #endregion


    #region IIdentityComparer

    public bool CanBeMergedWith(PhoneNumber? other) => other is null || IsEmpty || other.IsEmpty || ItemStripper.AreEqual(Value, other.Value);

    #endregion



    #region IEquatable

    //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
    /// <summary>
    /// Vergleicht die Instanz mit einem anderen <see cref="object"/>, um festzustellen, ob es sich bei <paramref name="obj"/>
    /// um ein <see cref="PhoneNumber"/>-Objekt handelt, das auf dieselbe
    /// Telefonnummer verweist.
    /// </summary>
    /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="PhoneNumber"/>-Objekt ist, das auf dieselbe Telefonnummer verweist.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not PhoneNumber p)
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


    /// <summary>
    /// Vergleicht die Instanz mit einem anderen <see cref="PhoneNumber"/>-Objekt, um festzustellen, ob beide
    /// auf dieselbe Telefonnummer verweisen.
    /// </summary>
    /// <param name="other">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> auf dieselbe Telefonnummer verweist.</returns>
    public bool Equals([NotNullWhen(true)] PhoneNumber? other)
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


    /// <summary>
    /// Erzeugt einen Hashcode für das Objekt.
    /// </summary>
    /// <returns>Der Hashcode.</returns>
    public override int GetHashCode()
    {
        int hashCode = StringCleaner.PrepareForComparison(Value).GetHashCode();
        return hashCode ^ _flags.GetHashCode();
    }


    #region Überladen von == und !=
    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht <paramref name="phone1"/> und <paramref name="phone2"/> um festzustellen, ob beide
    /// auf dieselbe Telefonnummer verweisen.
    /// </remarks>
    /// <param name="phone1">Linker Operand.</param>
    /// <param name="phone2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="phone1"/> und <paramref name="phone2"/> auf dieselbe Telefonnummer verweisen.</returns>
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


    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht <paramref name="phone1"/> und <paramref name="phone2"/> um festzustellen, ob beide
    /// auf unterschiedliche Telefonnummern verweisen.
    /// </remarks>
    /// <param name="phone1">Linker Operand.</param>
    /// <param name="phone2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="phone1"/> und <paramref name="phone2"/> auf unterschiedliche Telefonnummern verweisen.</returns>
    public static bool operator !=(PhoneNumber? phone1, PhoneNumber? phone2) => !(phone1 == phone2);


    /// <summary>
    /// Vergleicht den Inhalt der <see cref="Value"/>-Property von this mit denen eines anderen <see cref="PhoneNumber"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn beide Objekte auf dieselbe Telefonnummer verweisen.</returns>
    private bool CompareBoolean(PhoneNumber other)
        => StringComparer.Ordinal.Equals(StringCleaner.PrepareForComparison(Value), StringCleaner.PrepareForComparison(other.Value))
           && _flags == other._flags;

    #endregion

    #endregion

    #endregion


}
