﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using FolkerKinzel.Contacts.Intls;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt personenbezogene Daten.
/// </summary>
public sealed class Person : Mergeable<Person>, ICleanable, ICloneable, IEquatable<Person?>
{
    #region Prop Enum
    private enum Prop
    {
        Name,
        NickName,
        Gender,
        BirthDay,
        Spouse,
        Anniversary
    }
    #endregion


    #region private Fields

    private readonly Dictionary<Prop, object> _propDic = new();

    #endregion


    #region Constructors

    /// <summary>
    /// Initialisiert ein leeres <see cref="Person"/>-Objekt.
    /// </summary>
    public Person()
    {

    }

    /// <summary>
    /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller seiner Unterobjekte.
    /// </summary>
    /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
    private Person(Person source)
    {
        foreach (KeyValuePair<Prop, object> kvp in source._propDic)
        {
            this._propDic[kvp.Key] = kvp.Value is ICloneable cloneable ? cloneable.Clone() : kvp.Value;
        }
    }

    #endregion


    #region Accessor Methods

    [return: MaybeNull]
    private T Get<T>(Prop prop) => _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;

    private void Set<T>(Prop prop, T value)
    {
        if (value is null || value.Equals(default))
        {
            _ = _propDic.Remove(prop);
        }
        else
        {
            _propDic[prop] = value;
        }
    }

    #endregion


    #region Public Methods and Properties

    /// <summary>
    /// Name der Person
    /// </summary>
    public Name? Name
    {
        get => Get<Name?>(Prop.Name);
        set => Set(Prop.Name, value);
    }

    /// <summary>
    /// Rufname
    /// </summary>
    public string? NickName
    {
        get => Get<string?>(Prop.NickName);
        set => Set(Prop.NickName, value);
    }

    /// <summary>
    /// Geschlecht
    /// </summary>
    public Sex Gender
    {
        get => Get<Sex>(Prop.Gender);
        set => Set(Prop.Gender, value);
    }

    /// <summary>
    /// Geburtstag
    /// </summary>
    public DateTime? BirthDay
    {
        get => Get<DateTime?>(Prop.BirthDay);
        set => Set(Prop.BirthDay, value);
    }

    /// <summary>
    /// Name des Ehepartners
    /// </summary>
    public string? Spouse
    {
        get => Get<string?>(Prop.Spouse);
        set => Set(Prop.Spouse, value);
    }

    /// <summary>
    /// Hochzeitstag
    /// </summary>
    public DateTime? Anniversary
    {
        get => Get<DateTime?>(Prop.Anniversary);
        set => Set(Prop.Anniversary, value);
    }

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Person"/>-Objekts.
    /// </summary>
    /// <returns>Der Inhalt des <see cref="Person"/>-Objekts als <see cref="string"/>.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();

    #endregion


    #region Operators
    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Person"/>-Objekte, um zu überprüfen, ob sie gleich sind.
    /// </remarks>
    /// <param name="person1">Linker Operand.</param>
    /// <param name="person2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="person1"/> und <paramref name="person2"/> gleich sind.</returns>
    public static bool operator ==(Person? person1, Person? person2)
    {
        // If both are null, or both are same instance, return true.
        if (object.ReferenceEquals(person1, person2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (person1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return person2 is not null && person1.CompareBoolean(person2);
        }
    }

    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei<see cref="Person"/>-Objekte, um zu überprüfen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="person1">Linker Operand.</param>
    /// <param name="person2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="person1"/> und <paramref name="person2"/> ungleich sind.</returns>
    public static bool operator !=(Person? person1, Person? person2) => !(person1 == person2);

    #endregion


    #region Mergeable<T>, ICleanable


    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Person other)
    {
        if (Name?.IsMergeableWith(other.Name) ?? true)
        {
            DateTime? birthDay = this.BirthDay;

            if (birthDay.HasValue)
            {
                DateTime? otherBirthDay = other.BirthDay;
                if (otherBirthDay.HasValue)
                {
                    if (birthDay.Value.Date != otherBirthDay.Value.Date)
                    {
                        return true;
                    }
                }
            }

            return AreDifferent(NickName, other.NickName);
        }

        return true;

        //////////////////////////////////////////////////////

        static bool AreDifferent(string? s1, string? s2)
            => !Strip.IsEmpty(s1) && !Strip.IsEmpty(s2) && !Strip.AreEqual(s1, s2, true);
    }

    /// <inheritdoc/>
    protected override void CompleteDataWith(Person source)
    {
        Name? name = Name;
        Name? sourceName = source.Name;

        if (Name.CanBeMerged(name, sourceName))
        {
            Name = name?.Merge(sourceName) ?? sourceName;
        }

        if (Gender == default)
        {
            Gender = source.Gender;
        }

        if (Strip.IsEmpty(NickName))
        {
            NickName = source.NickName;
        }

        if (!BirthDay.HasValue)
        {
            BirthDay = source.BirthDay;
        }

        if (!Anniversary.HasValue)
        {
            Anniversary = source.Anniversary;
        }

        if (Strip.IsEmpty(Spouse))
        {
            Spouse = source.Spouse;
        }

    }


    #region ICleanable

    ///// <summary>
    ///// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte <see cref="Clean"/>
    ///// aufgerufen werden.
    ///// </summary>
    /// <inheritdoc/>
    public override bool IsEmpty => CheckIsEmpty();

    private bool CheckIsEmpty()
    {
        foreach (KeyValuePair<Prop, object> kvp in _propDic)
        {
            switch (kvp.Value)
            {
                case string s:
                    if (!Strip.IsEmpty(s))
                    {
                        return false;
                    }
                    break;
                case ICleanable adr:
                    if (!adr.IsEmpty)
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }
        }

        return true;
    }

    ///// <summary>
    ///// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    ///// und leere Unterobjekte auf <c>null</c>.
    ///// </summary>
    /// <inheritdoc/>
    public override void Clean()
    {
        KeyValuePair<Prop, object>[] props = _propDic.ToArray();

        DateTime MIN_DATE = DateTime.MinValue.AddDays(1);

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, object> kvp = props[i];

            if (kvp.Value is string s)
            {
                Set(kvp.Key, StringCleaner.CleanDataEntry(s));
            }
            else if (kvp.Value is DateTime dt)
            {
                if (dt < MIN_DATE) // sonst ggf. Exception bei Umwandlung in DateTimeOffset
                {
                    Set<DateTime?>(kvp.Key, null);
                }
                else
                {
                    // Entferne Zeitkomponente:
                    Set<DateTime?>(kvp.Key, dt.Date);
                }
            }
            else if (kvp.Value is Name name)
            {
                name.Clean();
                if (name.IsEmpty)
                {
                    Set<Name?>(kvp.Key, null);
                }
            }
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
    public object Clone() => new Person(this);

    #endregion


    #region IEquatable

    ///// <summary>
    ///// Vergleicht die Instanz mit einem anderen <see cref="object"/>, um festzustellen,
    ///// ob es sich bei <paramref name="obj"/> um ein <see cref="Person"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat. 
    ///// </summary>
    ///// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn es sich bei <paramref name="obj"/> um ein <see cref="Person"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        // If parameter cannot be cast to WabPerson return false.
        if (obj is not Person p)
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
    ///// Vergleicht die Instanz mit einem anderen 
    ///// <see cref="Person"/>-Objekt,
    ///// um festzustellen, ob beide gleich sind.
    ///// </summary>
    ///// <param name="other">Das <see cref="Person"/>-Objekt, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn <paramref name="other"/> gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Person? other)
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
    /// Vergleicht die Eigenschaften mit denen eines anderen <see cref="Person"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="Person"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Person other)
    {
        StringComparer comp = StringComparer.Ordinal;

        return Name == other.Name
        && BirthDay == other.BirthDay
        && comp.Equals(StringCleaner.PrepareForComparison(NickName), StringCleaner.PrepareForComparison(other.NickName))
        && Gender == other.Gender
        && Anniversary == other.Anniversary
        && comp.Equals(StringCleaner.PrepareForComparison(Spouse), StringCleaner.PrepareForComparison(other.Spouse));
    }

    ///// <summary>
    ///// Erzeugt einen Hashcode für das Objekt.
    ///// </summary>
    ///// <returns>Der Hashcode.</returns>
    /// <inheritdoc/>
    public override int GetHashCode() => (Name?.GetHashCode() ?? -1) ^ BirthDay.GetHashCode() ^ StringCleaner.PrepareForComparison(NickName).GetHashCode();

    #endregion


    #region internal


    internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
    {
        if (IsEmpty)
        {
            return sb;
        }

        Prop[] keys = _propDic.Keys.OrderBy(x => x).ToArray();

        string[] topics = new string[keys.Length];

        for (int i = 0; i < keys.Length; i++)
        {
            Prop key = keys[i];

            switch (key)
            {
                case Prop.Name:
                    topics[i] = Res.Name;
                    break;
                case Prop.NickName:
                    topics[i] = Res.NickName;
                    break;
                case Prop.Gender:
                    topics[i] = Res.Gender;
                    break;
                case Prop.BirthDay:
                    topics[i] = Res.BirthDay;
                    break;
                case Prop.Spouse:
                    topics[i] = Res.Spouse;
                    break;
                case Prop.Anniversary:
                    topics[i] = Res.Anniversary;
                    break;
                default:
                    break;
            }
        }


        int maxLength = topics.Select(x => x.Length).Max();
        maxLength++;

        for (int i = 0; i < topics.Length; i++)
        {
            _ = sb.Append(indent).Append(topics[i].PadRight(maxLength));

            object value = _propDic[keys[i]];

            _ = value switch
            {
                Name name => name.AppendTo(sb).AppendLine(),
                Sex sex => sb.AppendLine(sex == Sex.Male ? Res.Male : Res.Female),
                DateTime dt => sb.AppendLine(dt.ToShortDateString()),
                _ => sb.Append(value).AppendLine(),
            };
        }

        sb.Length -= Environment.NewLine.Length;
        return sb;
    }

    #endregion

}//class
