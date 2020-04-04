using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using FolkerKinzel.Contacts.Resources;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Die Klasse kapselt die Arbeitsstelle einer Person betreffende Daten.
    /// </summary>
    public sealed class Work : ICloneable, ICleanable, IEquatable<Work>
    {
        #region private Felder

        private readonly Dictionary<Prop, object> _propDic = new Dictionary<Prop, object>();

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
        /// Initialisiert eine neue, leere Instanz der <see cref="Work"/>-Klasse.
        /// </summary>
        public Work() { }


        /// <summary>
        /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller seiner Unterobjekte.
        /// </summary>
        /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
        private Work(Work source)
        {
            foreach (var kvp in source._propDic)
            {
                if (kvp.Value is ICloneable adr)
                {
                    this._propDic[kvp.Key] = adr.Clone();
                }
                else
                {
                    this._propDic[kvp.Key] = kvp.Value;
                }
            }
        }


        #endregion

        [return: MaybeNull]
        private T Get<T>(Prop prop)
        {
            return _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;
        }


        private void Set(Prop prop, object? value)
        {
            if (value is null)
            {
                _propDic.Remove(prop);
            }
            else
            {
                _propDic[prop] = value;
            }
        }

        #region öffentliche Eigenschaften und Methoden

        /// <summary>
        /// Name der Firma.
        /// </summary>
        public string? Company
        {
            get => Get<string?>(Prop.CompanyName);
            set => Set(Prop.CompanyName, value);
        }


        /// <summary>
        /// Abteilung (in der Firma).
        /// </summary>
        public string? Department
        {
            get => Get<string?>(Prop.Department);
            set => Set(Prop.Department, value);
        }


        /// <summary>
        /// Büro.
        /// </summary>
        public string? Office
        {
            get => Get<string?>(Prop.Office);
            set => Set(Prop.Office, value);
        }

        /// <summary>
        /// Titel bzw. Position des Mitarbeiters (in der Firma).
        /// </summary>
        public string? JobTitle
        {
            get => Get<string?>(Prop.JobTitle);
            set => Set(Prop.JobTitle, value);
        }

        /// <summary>
        /// Postanschrift (Firma)
        /// </summary>
        public Address? AddressWork
        {
            get => Get<Address?>(Prop.AddressWork);
            set => Set(Prop.AddressWork, value);
        }


        /// <summary>
        /// Erstellt eine String-Repräsentation des EnterpriseData-Objekts
        /// </summary>
        /// <returns>Der Inhalt des EnterpriseData-Objekts als String.</returns>
        public override string ToString() => this.AppendTo(new StringBuilder()).ToString();



        internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
        {
            if (IsEmpty) return sb;

            var keys = _propDic.Keys.Where(x => x != Prop.AddressWork).OrderBy(x => x).ToArray();

            string[] topics = new string[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];

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
                sb.Append(indent).Append(topics[i].PadRight(maxLength));
                sb.Append(_propDic[keys[i]]).AppendLine();
            }

            var adrWork = AddressWork;
            if(adrWork != null)
            {
                sb.Append(indent).Append(Res.AddressWork).AppendLine();
                adrWork.AppendTo(sb, indent + "        ").AppendLine();
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
        public object Clone()
        {
            return new Work(this);
        }

        #endregion


        #region ICleanable

        /// <summary>
        /// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
        /// und leere Unterobjekte auf <c>null</c>.
        /// </summary>
        public void Clean()
        {
            var props = _propDic.ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                var kvp = props[i];

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

#if !NET40
            _propDic.TrimExcess();
#endif
        }


        /// <summary>
        /// True gibt an, dass das EnterpriseData-Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte
        /// <see cref="Clean"/> aufgerufen werden.
        /// </summary>
        public bool IsEmpty
        {
            get => _propDic.Count == 0;

        }

        #endregion


        #region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen
        /// <summary>
        /// Vergleicht this mit einem anderen System.Object.
        /// </summary>
        /// <param name="obj">Das System.Object mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public override bool Equals(object? obj)
        {
            // If parameter cannot be cast to EnterpriseData return false.
            if (!(obj is Work p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen <see cref="Work"/>-Objekt.
        /// </summary>
        /// <param name="work">Das <see cref="Work"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public bool Equals(Work? work)
        {
            // If parameter is null return false:
            if (work is null) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, work)) return true;

            // Return true if the fields match:
            return CompareBoolean(work);
        }


        /// <summary>
        /// Erzeugt einen Hashcode für das Objekt.
        /// </summary>
        /// <returns>Der Hashcode.</returns>
        public override int GetHashCode()
        {
            int hash = -1;

            if (this.IsEmpty) return hash;

#if !NET40
            var comparison = StringComparison.OrdinalIgnoreCase;
#endif

            string? company = Company;
            if (!string.IsNullOrWhiteSpace(company))
            {
#if NET40
                hash ^= company.ToUpperInvariant().GetHashCode();
#else
                hash ^= company.GetHashCode(comparison);
#endif
                string? department = this.Department;
                if (!string.IsNullOrWhiteSpace(department))
                {
#if NET40
                    hash ^= department.ToUpperInvariant().GetHashCode();
#else
                    hash ^= department.GetHashCode(comparison);
#endif
                    string? office = this.Office;
                    if (!string.IsNullOrWhiteSpace(office))
                    {
#if NET40
                        hash ^= office.ToUpperInvariant().GetHashCode();
#else
                        hash ^= office.GetHashCode(comparison);
#endif
                    }
                }

                string? position = this.JobTitle;
                if (!string.IsNullOrWhiteSpace(position))
                {
#if NET40
                    hash ^= position.ToUpperInvariant().GetHashCode();
#else
                    hash ^= position.GetHashCode(comparison);
#endif
                }
            }



            var address = AddressWork;

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
        /// <param name="work1">linker Operand</param>
        /// <param name="work2">rechter Operand</param>
        /// <returns>true, wenn gleich</returns>
        public static bool operator ==(Work? work1, Work? work2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(work1, work2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (work1 is null)
                return false; // auf Referenzgleichheit wurde oben geprüft
            else
                return work2 is null ? false : work1.CompareBoolean(work2);
        }


        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <param name="work1">linker Operand</param>
        /// <param name="work2">rechter Operand</param>
        /// <returns>true, wenn ungleich</returns>
        public static bool operator !=(Work? work1, Work? work2)
        {
            return !(work1 == work2);
        }

        /// <summary>
        /// Vergleicht den Inhalt der <see cref="Company"/>- und <see cref="AddressWork"/>-Eigenschaften eines anderen <see cref="Work"/>-Objekts mit denen
        /// von this um zu bestimmen, ob beide auf dieselbe Arbeitsstelle verweisen.
        /// </summary>
        /// <param name="other">Das <see cref="Work"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte auf dieselbe Arbeitsstelle verweisen.</returns>
        private bool CompareBoolean(Work other)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

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

            var address = this.AddressWork;
            var otherAddress = other.AddressWork;
            if (address != null && otherAddress != null && !address.IsEmpty && !otherAddress.IsEmpty)
            {
                return address.Equals(other.AddressWork);
            }


            return true;
        }

        #endregion


        #endregion



    }
}
