using System.Text;
using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Informationen über den Namen einer Person.
/// </summary>
public sealed class Name : ICloneable, ICleanable, IEquatable<Name?>, IIdentityComparer<Name>
{
    private enum Prop
    {
        FirstName,
        MiddleName,
        LastName,
        Prefix,
        Suffix
    }

    #region private Felder

    private readonly Dictionary<Prop, string> _propDic = new();

    #endregion

    #region Ctors

    /// <summary>
    /// Initialisiert ein leeres <see cref="Name"/>-Objekt.
    /// </summary>
    public Name() { }

    /// <summary>
    /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <param name="other">Das zu kopierende <see cref="Name"/>-Objekt.</param>
    private Name(Name other)
    {
        foreach (KeyValuePair<Prop, string> kvp in other._propDic)
        {
            this._propDic.Add(kvp.Key, kvp.Value);
        }
    }

    #endregion


    private string? Get(Prop prop) => _propDic.ContainsKey(prop) ? _propDic[prop] : null;

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


    /// <summary>
    /// Familienname
    /// </summary>
    public string? LastName
    {
        get => Get(Prop.LastName);
        set => Set(Prop.LastName, value);
    }

    /// <summary>
    /// Vorname
    /// </summary>
    public string? FirstName
    {
        get => Get(Prop.FirstName);
        set => Set(Prop.FirstName, value);
    }

    /// <summary>
    /// Zweiter Vorname
    /// </summary>
    public string? MiddleName
    {
        get => Get(Prop.MiddleName);
        set => Set(Prop.MiddleName, value);
    }

    /// <summary>
    /// Namenspräfix (z.B. "Prof. Dr.")
    /// </summary>
    public string? Prefix
    {
        get => Get(Prop.Prefix);
        set => Set(Prop.Prefix, value);
    }

    /// <summary>
    /// Namenssuffix (z.B. "jr.")
    /// </summary>
    public string? Suffix
    {
        get => Get(Prop.Suffix);
        set => Set(Prop.Suffix, value);
    }

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Name"/>-Objekts.
    /// </summary>
    /// <returns>Der Inhalt des <see cref="Name"/>-Objekts als <see cref="string"/>.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();


    internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
    {
        _ = sb.Append(indent);

        int initialStringBuilderLength = sb.Length;

        AppendNamePart(Prefix);
        AppendNamePart(FirstName);
        AppendNamePart(MiddleName);
        AppendNamePart(LastName);
        AppendNamePart(Suffix);


        void AppendNamePart(string? namePart)
        {
            if (!string.IsNullOrWhiteSpace(namePart))
            {
                if (sb.Length != initialStringBuilderLength)
                {
                    _ = sb.Append(' ');
                }
                _ = sb.Append(namePart);
            }
        }

        return sb;
    }

    #region Interfaces

    #region ICleanable

    /// <summary>
    /// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte
    /// <see cref="Clean"/> aufgerufen werden.
    /// </summary>
    public bool IsEmpty => _propDic.Count == 0;


    /// <summary>
    /// Entfernt leere Strings und überflüssige Leerzeichen.
    /// </summary>
    public void Clean()
    {
        KeyValuePair<Prop, string>[] props = _propDic.ToArray();

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, string> kvp = props[i];
            Set(kvp.Key, StringCleaner.CleanDataEntry(kvp.Value));
        }

#if !NET40 && !NET461 && !NETSTANDARD2_0
        _propDic.TrimExcess();
#endif
    }


    #endregion


    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Name(this);

    #endregion

    #region IIdentityComparer

    public bool CanBeMergedWith(Name? other) => other is null || IsEmpty || other.IsEmpty || !BelongsToOtherIdentity(other);

    private bool BelongsToOtherIdentity(Name other)
    {
        if (!ItemStripper.StartEqual(this.FirstName, other.FirstName, true))
        {
            return true;
        }

        return !ItemStripper.StartEqual(this.LastName, other.LastName, true);
    }

    #endregion


    #region IEquatable

    //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
    /// <summary>
    /// Vergleicht die Instanz mit einem anderen <see cref="object"/> um festzustellen,
    /// ob es sich bei <paramref name="obj"/> um ein <see cref="Name"/>-Objekt handelt, das denselben Namen darstellt.
    /// </summary>
    /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Name"/>-Objekt ist, das denselben Namen darstellt.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is not Name p)
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
    /// Vergleicht this mit einem anderen <see cref="Name"/>-Objekt, um zu ermitteln,
    /// ob beide denselben Namen darstellen.
    /// </summary>
    /// <param name="other">Das <see cref="Name"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> denselben Namen darstellt.</returns>
    public bool Equals([NotNullWhen(true)] Name? other)
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
        int hash = -1;

        ModifyHash(LastName);
        ModifyHash(FirstName);
        ModifyHash(MiddleName);
        ModifyHash(Suffix);
        //ModifyHash(Prefix);

        return hash;

        void ModifyHash(string? s)
        {
            hash ^= s?.GetHashCode() ?? string.Empty.GetHashCode();
        }
    }


    #region Überladen von == und !=
    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht <paramref name="name1"/> und <paramref name="name2"/>, um festzustellen, ob diese denselben Namen darstellen.
    /// </remarks>
    /// <param name="name1">Linker Operand.</param>
    /// <param name="name2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> denselben Namen darstellen.</returns>
    public static bool operator ==(Name? name1, Name? name2)
    {
        // If both are null, or both are same instance, return true.
        if (object.ReferenceEquals(name1, name2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (name1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return name2 is not null && name1.CompareBoolean(name2);
        }
    }

    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht <paramref name="name1"/> und <paramref name="name2"/>, um festzustellen, ob diese verschiedene Namen darstellen.
    /// </remarks>
    /// <param name="name1">Linker Operand.</param>
    /// <param name="name2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> verschiedene Namen darstellen.</returns>
    public static bool operator !=(Name? name1, Name? name2) => !(name1 == name2);

    /// <summary>
    /// Vergleicht den Inhalt von this mit dem eines anderen <see cref="Name"/>-Objekts, um zu ermitteln,
    /// ob beide den Namen derselben physischen Person darstellen.
    /// </summary>
    /// <param name="other">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn beide Objekte auf den Namen derselben Person verweisen.</returns>
    private bool CompareBoolean(Name other)
    {
        StringComparer comparer = StringComparer.Ordinal;

        return comparer.Equals(Prepare(LastName), Prepare(other.LastName))
            && comparer.Equals(Prepare(FirstName), Prepare(other.FirstName))
            && comparer.Equals(Prepare(MiddleName), Prepare(other.MiddleName))
            && comparer.Equals(Prepare(Suffix), Prepare(other.Suffix))
            && comparer.Equals(Prepare(Prefix), Prepare(other.Prefix));

        static string Prepare(string? s) => s ?? string.Empty;
    }

    #endregion

    #endregion

    #endregion


}
