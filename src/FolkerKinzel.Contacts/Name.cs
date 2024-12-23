using System.Text;
using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Informationen über den Namen einer Person.
/// </summary>
public sealed class Name : MergeableObject<Name>, ICleanable, ICloneable, IEquatable<Name?>
{
    #region Prop Enum
    private enum Prop
    {
        FirstName,
        MiddleName,
        LastName,
        Prefix,
        Suffix
    }
    #endregion

    #region Private Fields

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

    #region AccessorMethods

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

    #endregion

    #region Public Properties and Methods

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

    #endregion

    #region Operators


    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Name"/>-Objekte, um festzustellen, ob sie gleich sind.
    /// </remarks>
    /// <param name="name1">Linker Operand.</param>
    /// <param name="name2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> gleich sind.</returns>
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
    /// Vergleicht zwei <see cref="Name"/>-Objekte, um festzustellen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="name1">Linker Operand.</param>
    /// <param name="name2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> ungleich sind.</returns>
    public static bool operator !=(Name? name1, Name? name2) => !(name1 == name2);

    #endregion


    #region MergeableObject<T>, ICleanable

    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Name other)
    {
        if (AreDifferent(FirstName, other.FirstName))
        {
            return true;
        }

        if (AreDifferent(LastName, other.LastName))
        {
            return true;
        }

        return false;

        //////////////////////////////////////////////////////

        static bool AreDifferent(string? s1, string? s2)
            => !Strip.IsEmpty(s1) && !Strip.IsEmpty(s2) && !Strip.StartEqual(s1, s2, true);
    }


    /// <inheritdoc/>
    protected override void SupplementWith(Name source)
    {
        if (Strip.IsEmpty(FirstName))
        {
            FirstName = source.FirstName;
        }

        if (Strip.IsEmpty(LastName))
        {
            LastName = source.LastName;
        }

        if (Strip.IsEmpty(MiddleName))
        {
            MiddleName = source.MiddleName;
        }

        if (Strip.IsEmpty(Suffix))
        {
            Suffix = source.Suffix;
        }

        if (Strip.IsEmpty(Prefix))
        {
            Prefix = source.Prefix;
        }
    }


    #region ICleanable

    /// <inheritdoc/>
    public override bool IsEmpty => !_propDic.Any(x => !Strip.IsEmpty(x.Value));


    /// <inheritdoc/>
    public override void Clean()
    {
        KeyValuePair<Prop, string>[] props = _propDic.ToArray();

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, string> kvp = props[i];
            Set(kvp.Key, StringCleaner.CleanDataEntry(kvp.Value));
        }

#if !NET462 && !NETSTANDARD2_0
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
    public object Clone() => new Name(this);

    #endregion


    #region IEquatable

    /// <inheritdoc/>
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


    /// <inheritdoc />
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
    /// Vergleicht die Eigenschaften mit denen eines anderen <see cref="Name"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="Name"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Name other)
    {
        StringComparer comparer = StringComparer.Ordinal;

        return comparer.Equals(StringCleaner.PrepareForComparison(LastName), StringCleaner.PrepareForComparison(other.LastName))
            && comparer.Equals(StringCleaner.PrepareForComparison(FirstName), StringCleaner.PrepareForComparison(other.FirstName))
            && comparer.Equals(StringCleaner.PrepareForComparison(MiddleName), StringCleaner.PrepareForComparison(other.MiddleName))
            && comparer.Equals(StringCleaner.PrepareForComparison(Suffix), StringCleaner.PrepareForComparison(other.Suffix))
            && comparer.Equals(StringCleaner.PrepareForComparison(Prefix), StringCleaner.PrepareForComparison(other.Prefix));
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(
            StringCleaner.PrepareForComparison(LastName),
            StringCleaner.PrepareForComparison(FirstName),
            StringCleaner.PrepareForComparison(MiddleName),
            StringCleaner.PrepareForComparison(Suffix));
    }

    #endregion


    #region internal

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

    #endregion
}
