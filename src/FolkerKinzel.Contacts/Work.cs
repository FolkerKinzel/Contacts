using System.Diagnostics.CodeAnalysis;
using System.Text;
using FolkerKinzel.Contacts.Intls;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts;

/// <summary>
/// Kapselt Informationen über die Arbeitsstelle einer Person.
/// </summary>
public sealed class Work : ICloneable, ICleanable, IEquatable<Work?>, IIdentityComparer<Work>
{
    #region private Felder

    private readonly Dictionary<Prop, object> _propDic = new();

    #endregion

    private enum Prop
    {
        CompanyName,
        Department,
        Office,
        JobTitle,
        AddressWork
    }


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

    #region öffentliche Eigenschaften und Methoden

    /// <summary>
    /// Name der Firma oder Organisation
    /// </summary>
    public string? Company
    {
        get => Get<string?>(Prop.CompanyName);
        set => Set(Prop.CompanyName, value);
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
                case Prop.CompanyName:
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


    #region Interfaces

    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Work(this);

    #endregion


    #region ICleanable

    /// <summary>
    /// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    /// und leere Unterobjekte auf <c>null</c>.
    /// </summary>
    public void Clean()
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


    /// <summary>
    /// <c>true</c> gibt an, dass das <see cref="Work"/>-Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte
    /// <see cref="Clean"/> aufgerufen werden.
    /// </summary>
    public bool IsEmpty => _propDic.Count == 0;

    #endregion

    #region IIdentityComparer
    public bool CanBeMergedWith(Work? other) => other is null || IsEmpty || other.IsEmpty || !BelongsToOtherIdentity(other);


    private bool BelongsToOtherIdentity(Work other)
    {
        string? company = Company;
        string? otherCompany = other.Company;

        bool areAddressesMergeable = AddressWork?.CanBeMergedWith(other.AddressWork) ?? true;


        if ((ItemStripper.IsEmpty(company) || ItemStripper.IsEmpty(otherCompany)) && areAddressesMergeable)
        {
            return false;
        }

        if (!ItemStripper.StartEqual(company, otherCompany, true))
        {
            return true;
        }

        if (!ItemStripper.StartEqual(Department, other.Department, true))
        {
            return true;
        }

        if (!ItemStripper.StartEqual(Office, other.Office, true))
        {
            return true;
        }

        if (!ItemStripper.StartEqual(JobTitle, other.JobTitle, true))
        {
            return true;
        }

        return !areAddressesMergeable;
    }

    #endregion

    #region IEquatable

    //Überschreiben von Object.Equals um Vergleich zu ermöglichen
    /// <summary>
    /// Vergleicht die aktuelle Instanz mit einem anderen <see cref="object"/>, um festzustellen, ob beide <see cref="Work"/>-Objekte sind, die dieselbe
    /// Arbeitsstelle beschreiben.
    /// </summary>
    /// <param name="obj">Das <see cref="object"/>, mit dem die aktuelle Instanz verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Work"/>-Objekt ist, das dieselbe Arbeitsstelle beschreibt.</returns>
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


    /// <summary>
    /// Vergleicht die aktuelle Instanz mit einem anderen <see cref="Work"/>-Objekt, um festzustellen, ob beide dieselbe
    /// Arbeitsstelle beschreiben.
    /// </summary>
    /// <param name="other">Das <see cref="Work"/>-Objekt, mit dem die aktuelle Instanz verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Arbeitsstelle beschreibt.</returns>
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
    /// Erzeugt einen Hashcode für das Objekt.
    /// </summary>
    /// <returns>Der Hashcode.</returns>
    public override int GetHashCode()
    {
        int hash = -1;

        if (this.IsEmpty)
        {
            return hash;
        }

#if !NET40 && !NETSTANDARD2_0 && !NET461
        StringComparison comparison = StringComparison.OrdinalIgnoreCase;
#endif

        string? company = Company;
        if (!string.IsNullOrWhiteSpace(company))
        {
#if NET40 || NETSTANDARD2_0 || NET461
                hash ^= company.ToUpperInvariant().GetHashCode();
#else
            hash ^= company.GetHashCode(comparison);
#endif
            string? department = this.Department;
            if (!string.IsNullOrWhiteSpace(department))
            {
#if NET40 || NETSTANDARD2_0 || NET461
                    hash ^= department.ToUpperInvariant().GetHashCode();
#else
                hash ^= department.GetHashCode(comparison);
#endif
                string? office = this.Office;
                if (!string.IsNullOrWhiteSpace(office))
                {
#if NET40 || NETSTANDARD2_0 || NET461
                        hash ^= office.ToUpperInvariant().GetHashCode();
#else
                    hash ^= office.GetHashCode(comparison);
#endif
                }
            }

            string? position = this.JobTitle;
            if (!string.IsNullOrWhiteSpace(position))
            {
#if NET40 || NETSTANDARD2_0 || NET461
                    hash ^= position.ToUpperInvariant().GetHashCode();
#else
                hash ^= position.GetHashCode(comparison);
#endif
            }
        }

        Address? address = AddressWork;

        if (address != null)
        {
            hash ^= address.GetHashCode();
        }

        return hash;
    }


    // Überladen von == und !=
    /// <summary>
    /// Überladung des == Operators. 
    /// </summary>
    /// <remarks>
    /// Vergleicht <paramref name="work1"/> und <paramref name="work2"/>, um festzustellen,
    /// ob sie auf dieselbe Arbeitsstelle verweisen.</remarks>
    /// <param name="work1">Linker Operand.</param>
    /// <param name="work2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="work1"/> und <paramref name="work2"/> auf
    /// dieselbe Arbeitsstelle verweisen.</returns>
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
    /// Vergleicht <paramref name="work1"/> und <paramref name="work2"/>, um festzustellen,
    /// ob sie auf unterschiedliche Arbeitsstellen verweisen.
    /// </remarks>
    /// <param name="work1">Linker Operand.</param>
    /// <param name="work2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="work1"/> und <paramref name="work2"/>
    /// auf unterschiedliche Arbeitsstellen verweisen.</returns>
    public static bool operator !=(Work? work1, Work? work2) => !(work1 == work2);

    /// <summary>
    /// Vergleicht den Inhalt der <see cref="Company"/>- und <see cref="AddressWork"/>-Eigenschaften eines anderen <see cref="Work"/>-Objekts mit denen
    /// von this, um zu bestimmen, ob beide auf dieselbe Arbeitsstelle verweisen.
    /// </summary>
    /// <param name="other">Das <see cref="Work"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn beide Objekte auf dieselbe Arbeitsstelle verweisen.</returns>
    private bool CompareBoolean(Work other)
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        string? company = this.Company;
        string? otherCompany = other.Company;

        if (!string.IsNullOrWhiteSpace(company) && !string.IsNullOrWhiteSpace(otherCompany))
        {
            if (comparer.Equals(company, otherCompany))
            {
                string? department = this.Department;
                string? otherDepartment = other.Department;
                if (!string.IsNullOrWhiteSpace(department) && !string.IsNullOrWhiteSpace(otherDepartment))
                {
                    if (comparer.Equals(department, otherDepartment))
                    {
                        string? office = this.Office;
                        string? otherOffice = other.Office;
                        if (!string.IsNullOrWhiteSpace(office) && !string.IsNullOrWhiteSpace(otherOffice))
                        {
                            if (comparer.Equals(office, otherOffice))
                            {
                                string? position = this.JobTitle;
                                string? otherPosition = other.JobTitle;

                                if (!string.IsNullOrWhiteSpace(position) && !string.IsNullOrWhiteSpace(otherPosition))
                                {
                                    return comparer.Equals(position, otherPosition);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(office) && string.IsNullOrWhiteSpace(otherOffice))
                        {
                            string? position = this.JobTitle;
                            string? otherPosition = other.JobTitle;

                            if (!string.IsNullOrWhiteSpace(position) && !string.IsNullOrWhiteSpace(otherPosition))
                            {
                                return comparer.Equals(position, otherPosition);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (string.IsNullOrWhiteSpace(department) && string.IsNullOrWhiteSpace(otherDepartment))
                {
                    string? office = this.Office;
                    string? otherOffice = other.Office;
                    if (!string.IsNullOrWhiteSpace(office) && !string.IsNullOrWhiteSpace(otherOffice))
                    {
                        if (comparer.Equals(office, otherOffice))
                        {
                            string? position = this.JobTitle;
                            string? otherPosition = other.JobTitle;

                            if (!string.IsNullOrWhiteSpace(position) && !string.IsNullOrWhiteSpace(otherPosition))
                            {
                                return comparer.Equals(position, otherPosition);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        Address? address = this.AddressWork;
        Address? otherAddress = other.AddressWork;
        if (address != null && otherAddress != null && !address.IsEmpty && !otherAddress.IsEmpty)
        {
            return address.Equals(otherAddress);
        }


        return true;
    }


    #endregion


    #endregion



}
