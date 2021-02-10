using FolkerKinzel.Contacts.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Kapselt personenbezogene Daten.
    /// </summary>
    public sealed class Person : ICloneable, ICleanable, IEquatable<Person>
    {
        private enum Prop
        {
            Name,
            NickName,
            Gender,
            BirthDay,
            Spouse,
            Anniversary
        }


        #region private Felder

        private readonly Dictionary<Prop, object> _propDic = new Dictionary<Prop, object>();

        #endregion



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


        #region öffentliche Properties und Methoden

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

                switch (value)
                {
                    case Name name:
                        _ = name.AppendTo(sb).AppendLine();
                        break;
                    case Sex sex:
                        _ = sb.AppendLine(sex == Sex.Male ? Res.Male : Res.Female);
                        break;
                    case DateTime dt:
                        _ = sb.AppendLine(dt.ToShortDateString());
                        break;
                    default:
                        _ = sb.Append(value).AppendLine();
                        break;
                }
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
        public object Clone() => new Person(this);

        #endregion


        #region ICleanable

        /// <summary>
        /// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte <see cref="Clean"/>
        /// aufgerufen werden.
        /// </summary>
        public bool IsEmpty => _propDic.Count == 0;

        /// <summary>
        /// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
        /// und leere Unterobjekte auf <c>null</c>.
        /// </summary>
        public void Clean()
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
                else if(kvp.Value is Name name)
                {
                    name.Clean();
                    if(name.IsEmpty)
                    {
                        Set<Name?>(kvp.Key, null);
                    }
                }
            }

#if !NET40
            _propDic.TrimExcess();
#endif
        }


        #endregion


        #region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen
        /// <summary>
        /// Vergleicht die Instanz mit einem anderen <see cref="object"/>, um festzustellen,
        /// ob es sich bei <paramref name="obj"/> um ein <see cref="Person"/>-Objekt handelt, das
        /// auf dieselbe physische Person verweist. 
        /// </summary>
        /// <remarks>Zum 
        /// Vergleich werden die Eigenschaften <see cref="Name"/>, <see cref="NickName"/> und <see cref="BirthDay"/>
        /// verwendet.</remarks>
        /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn es sich bei <paramref name="obj"/> um ein <see cref="Person"/>-Objekt handelt, das
        /// auf dieselbe physische Person verweist.</returns>
        public override bool Equals(object? obj)
        {
            // If parameter cannot be cast to WabPerson return false.
            if (!(obj is Person p))
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
        /// Vergleicht die Instanz mit einem anderen 
        /// <see cref="Person"/>-Objekt,
        /// um festzustellen, ob beide auf dieselbe Person verweisen.
        /// </summary>
        /// <remarks>Zum 
        /// Vergleich werden die Eigenschaften <see cref="Name"/>, <see cref="NickName"/> und <see cref="BirthDay"/>
        /// verwendet.</remarks>
        /// <param name="other">Das <see cref="Person"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="other"/> auf dieselbe physische Person verweist.</returns>
        public bool Equals(Person? other)
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
            Name? name = Name;

            if (name is null || name.IsEmpty)
            {
                string? nickName = NickName;

                if (!string.IsNullOrWhiteSpace(nickName))
                {
#if NET40
                    hash ^= nickName.Trim().ToUpper(CultureInfo.CurrentUICulture).GetHashCode();
#else
                    hash ^= nickName.Trim().GetHashCode(StringComparison.CurrentCultureIgnoreCase);
#endif
                }
            }
            else
            {
                hash = name.GetHashCode();
            }

            // Wenn weder Name noch NickName angegeben sind, interessiert auch Birthday nicht.
            return hash;
        }


        // Überladen von == und !=
        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <remarks>
        /// Vergleicht die Eigenschaften <see cref="Name"/>, <see cref="NickName"/> und <see cref="BirthDay"/> zweier 
        /// <see cref="Person"/>-Objekte,
        /// um festzustellen, ob beide auf dieselbe physische Person verweisen.
        /// </remarks>
        /// <param name="person1">Linker Operand.</param>
        /// <param name="person2">Rechter Operand.</param>
        /// <returns><c>true</c>, wenn <paramref name="person1"/> und <paramref name="person2"/> auf dieselbe physische Person verweisen.</returns>
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
                return !(person2 is null) && person1.CompareBoolean(person2);
            }
        }

        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <remarks>
        /// Vergleicht die Eigenschaften <see cref="Name"/>, <see cref="NickName"/> und <see cref="BirthDay"/> zweier 
        /// <see cref="Person"/>-Objekte,
        /// um festzustellen, ob beide auf unterschiedliche Personen verweisen.
        /// </remarks>
        /// <param name="person1">Linker Operand.</param>
        /// <param name="person2">Rechter Operand.</param>
        /// <returns><c>true</c>, wenn <paramref name="person1"/> und <paramref name="person2"/> auf unterschiedliche Personen verweisen.</returns>
        public static bool operator !=(Person? person1, Person? person2) => !(person1 == person2);

        /// <summary>
        /// Vergleicht die Eigenschaften <see cref="Name"/>, <see cref="NickName"/> und <see cref="BirthDay"/> mit denen eines anderen <see cref="Person"/>-Objekts,
        /// um zu überprüfen, ob beide auf dieselbe physische Person verweisen.
        /// </summary>
        /// <param name="p">Das <see cref="Person"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide <see cref="Person"/>-Objekte auf dieselbe physische Person verweisen.</returns>
        private bool CompareBoolean(Person p)
        {
            Name? name = Name;
            Name? otherName = p.Name;

            if (name is null || otherName is null || name.IsEmpty || otherName.IsEmpty)
            {
                string? nickName = NickName;
                string? otherNickName = p.NickName;

                if (!string.IsNullOrWhiteSpace(nickName) && !string.IsNullOrWhiteSpace(otherNickName))
                {
                    return StringComparer.CurrentCultureIgnoreCase.Equals(nickName.Trim(), otherNickName.Trim());
                }
            }
            else if (name != otherName) 
            {
                return false;
            }

            DateTime? birthDay = this.BirthDay;

            if (birthDay.HasValue)
            {
                DateTime? otherBirthDay = p.BirthDay;
                if (otherBirthDay.HasValue)
                {
                    return birthDay.Value.Date == otherBirthDay.Value.Date;
                }
            }

            return true;
        }

        #endregion

        #endregion




    }//class
}//ns
