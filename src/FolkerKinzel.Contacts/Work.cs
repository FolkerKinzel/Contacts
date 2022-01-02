using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using FolkerKinzel.Contacts.Intls;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Informationen über die Arbeitsstelle einer Person.
/// </summary>
public sealed class Work : MergeableObject<Work>, ICleanable, ICloneable, IEquatable<Work?>
{
    #region Prop Enum
    private enum Prop
    {
        Company,
        Department,
        Office,
        JobTitle,
        AddressWork
    }
    #endregion

    #region private Fields

    private readonly Dictionary<Prop, object> _propDic = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Initialisiert eine leere Instanz der <see cref="Work"/>-Klasse.
    /// </summary>
    public Work() { }


    /// <summary>
    /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller seiner Unterobjekte.
    /// </summary>
    /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
    private Work(Work source)
    {
        foreach (KeyValuePair<Prop, object> kvp in source._propDic)
        {
            this._propDic[kvp.Key] = kvp.Value is ICloneable adr ? adr.Clone() : kvp.Value;
        }
    }


    #endregion

    #region Accessor Methods

    [return: MaybeNull]
    private T Get<T>(Prop prop) => _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;


    private void Set(Prop prop, object? value)
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
    /// Name der Firma oder Organisation
    /// </summary>
    public string? Company
    {
        get => Get<string?>(Prop.Company);
        set => Set(Prop.Company, value);
    }

    /// <summary>
    /// Abteilung
    /// </summary>
    public string? Department
    {
        get => Get<string?>(Prop.Department);
        set => Set(Prop.Department, value);
    }

    /// <summary>
    /// Büro
    /// </summary>
    public string? Office
    {
        get => Get<string?>(Prop.Office);
        set => Set(Prop.Office, value);
    }

    /// <summary>
    /// Titel bzw. Position des Mitarbeiters
    /// </summary>
    public string? JobTitle
    {
        get => Get<string?>(Prop.JobTitle);
        set => Set(Prop.JobTitle, value);
    }

    /// <summary>
    /// Postanschrift der Firma oder Organisation
    /// </summary>
    public Address? AddressWork
    {
        get => Get<Address?>(Prop.AddressWork);
        set => Set(Prop.AddressWork, value);
    }

    /// <summary>
    /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="Work"/>-Objekts.
    /// </summary>
    /// <returns>Der Inhalt des <see cref="Work"/>-Objekts als <see cref="string"/>.</returns>
    public override string ToString() => AppendTo(new StringBuilder()).ToString();

    #endregion

    #region Operators

    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Work"/>-Objekte, um zu überprüfen, ob sie gleich sind.
    /// </remarks>
    /// <param name="work1">Linker Operand.</param>
    /// <param name="work2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="work1"/> und <paramref name="work2"/> gleich sind.</returns>
    public static bool operator ==(Work? work1, Work? work2)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(work1, work2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (work1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return work2 is not null && work1.CompareBoolean(work2);
        }
    }


    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Work"/>-Objekte, um zu überprüfen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="work1">Linker Operand.</param>
    /// <param name="work2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="work1"/> und <paramref name="work2"/> ungleich sind.</returns>
    public static bool operator !=(Work? work1, Work? work2) => !(work1 == work2);

    #endregion

    #region MergeableObject<T>, ICleanable

    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Work other)
    {
        //if (StringHasEvidence(Company, other.Company, out bool isDifferentIdentity))
        //{

        //}
        if (AreDifferent(Company, other.Company))
        {
            return true;
        }

        if (AreDifferent(Department, other.Department))
        {
            return true;
        }

        if (AreDifferent(Office, other.Office))
        {
            return true;
        }

        if (AreDifferent(JobTitle, other.JobTitle))
        {
            return true;
        }

        return !Address.AreMergeable(AddressWork, other.AddressWork);


        //////////////////////////////////////////////////////
        
        //static bool StringHasEvidence(string? s1, string? s2, out bool isDifferentIdentity)
        //{
        //    isDifferentIdentity = true;

        //    if (Strip.IsEmpty(s1) && Strip.IsEmpty(s2))
        //    {
        //        return false;
        //    }

        //    isDifferentIdentity = !Strip.StartEqual(s1, s2, true);
        //    return true;
        //}

        static bool AreDifferent(string? s1, string? s2)
            => !Strip.IsEmpty(s1) && !Strip.IsEmpty(s2) && !Strip.StartEqual(s1, s2, true);
    }

    /// <inheritdoc/>
    protected override void CompleteDataWith(Work source)
    {
        Address? adr = AddressWork;
        Address? sourceAdr = source.AddressWork;

        if (Address.AreMergeable(adr, sourceAdr))
        {
            AddressWork = adr?.Merge(sourceAdr) ?? (Address?)sourceAdr?.Clone();
        }

        if (Strip.IsEmpty(Company))
        {
            Company = source.Company;
        }

        if (Strip.IsEmpty(Department))
        {
            Department = source.Department;
        }

        if (Strip.IsEmpty(Office))
        {
            Office = source.Office;
        }

        if (Strip.IsEmpty(JobTitle))
        {
            JobTitle = source.JobTitle;
        }
    }

    #region ICleanable

    ///// <summary>
    ///// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    ///// und leere Unterobjekte auf <c>null</c>.
    ///// </summary>
    /// <inheritdoc/>
    public override void Clean()
    {
        KeyValuePair<Prop, object>[] props = _propDic.ToArray();

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, object> kvp = props[i];

            if (kvp.Value is string s)
            {
                Set(kvp.Key, StringCleaner.CleanDataEntry(s));
            }
            else if (kvp.Value is ICleanable adr)
            {
                adr.Clean();
                if (adr.IsEmpty)
                {
                    Set(kvp.Key, null);
                }
            }
        }

#if !NET40 && !NET461 && !NETSTANDARD2_0
        _propDic.TrimExcess();
#endif
    }


    ///// <summary>
    ///// <c>true</c> gibt an, dass das <see cref="Work"/>-Objekt keine verwertbaren Daten enthält.
    ///// </summary>
    /// <inheritdoc/>
    public override bool IsEmpty
    {
        get
        {
            foreach (KeyValuePair<Prop, object> kvp in _propDic)
            {
                if ((kvp.Value is string s && !Strip.IsEmpty(s)) || (kvp.Value is ICleanable adr && !adr.IsEmpty))
                {
                    return false;
                }
            }

            return true;
        }
    }
    #endregion
    
    #endregion

    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Work(this);

    #endregion

    #region IEquatable

    ///// <summary>
    ///// Vergleicht die Instanz mit einem anderen <see cref="object"/>, um festzustellen,
    ///// ob es sich bei <paramref name="obj"/> um ein <see cref="Work"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat. 
    ///// </summary>
    ///// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn es sich bei <paramref name="obj"/> um ein <see cref="Work"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        // If parameter cannot be cast to EnterpriseData return false.
        if (obj is not Work p)
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
    ///// <see cref="Work"/>-Objekt,
    ///// um festzustellen, ob beide gleich sind.
    ///// </summary>
    ///// <param name="other">Das <see cref="Work"/>-Objekt, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn <paramref name="other"/> gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Work? other)
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
    /// Vergleicht die Eigenschaften mit denen eines anderen <see cref="Work"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="Work"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
    private bool CompareBoolean(Work other)
    {
        StringComparer comparer = StringComparer.Ordinal;

        return comparer.Equals(StringCleaner.PrepareForComparison(Company), StringCleaner.PrepareForComparison(other.Company))
               && comparer.Equals(StringCleaner.PrepareForComparison(Department), StringCleaner.PrepareForComparison(other.Department))
               && comparer.Equals(StringCleaner.PrepareForComparison(Office), StringCleaner.PrepareForComparison(other.Office))
               && comparer.Equals(StringCleaner.PrepareForComparison(JobTitle), StringCleaner.PrepareForComparison(other.JobTitle))
               && AddressWork == other.AddressWork;
    }


    ///// <summary>
    ///// Erzeugt einen Hashcode für das Objekt.
    ///// </summary>
    ///// <returns>Der Hashcode.</returns>
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hash = -1
                   ^ GetHash(Company)
                   ^ GetHash(Department)
                   ^ GetHash(Office)
                   ^ GetHash(JobTitle);

        Address? adr = AddressWork;

        if (adr is not null)
        {
            hash ^= adr.GetHashCode();
        }

        return hash;

        ////////////////////////////////////////

        static int GetHash(string? s) => StringCleaner.PrepareForComparison(s).GetHashCode();
    }

    #endregion

    #region internal

    internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
    {
        if (IsEmpty)
        {
            return sb;
        }

        Prop[] keys = _propDic.Keys.Where(x => x != Prop.AddressWork).OrderBy(x => x).ToArray();

        string[] topics = new string[keys.Length];

        for (int i = 0; i < keys.Length; i++)
        {
            Prop key = keys[i];

            switch (key)
            {
                case Prop.Company:
                    topics[i] = Res.Company;
                    break;
                case Prop.JobTitle:
                    topics[i] = Res.Position;
                    break;
                case Prop.Department:
                    topics[i] = Res.Department;
                    break;
                case Prop.Office:
                    topics[i] = Res.Office;
                    break;
                //case Prop.AddressWork:
                //    topics[i] = Res.AddressWork;
                //    break;
                default:
                    break;
            }
        }

        int maxLength = topics.Select(x => x.Length).Max();
        maxLength++;

        for (int i = 0; i < topics.Length; i++)
        {
            _ = sb.Append(indent).Append(topics[i].PadRight(maxLength));
            _ = sb.Append(_propDic[keys[i]]).AppendLine();
        }

        Address? adrWork = AddressWork;
        if (adrWork != null)
        {
            _ = sb.Append(indent).Append(Res.AddressWork).AppendLine();
            _ = adrWork.AppendTo(sb, indent + "        ").AppendLine();
        }

        sb.Length -= Environment.NewLine.Length;
        return sb;
    }
    #endregion
}
