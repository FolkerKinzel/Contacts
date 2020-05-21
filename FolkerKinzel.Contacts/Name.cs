using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Kapselt Informationen über den Namen einer Person.
    /// </summary>
    public class Name : ICloneable, ICleanable, IEquatable<Name>
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

        private readonly Dictionary<Prop, string> _propDic = new Dictionary<Prop, string>();

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
            foreach (var kvp in other._propDic)
            {
                this._propDic.Add(kvp.Key, kvp.Value);
            }
        }

        #endregion


        private string? Get(Prop prop)
        {
            return _propDic.ContainsKey(prop) ? _propDic[prop] : null;
        }

        private void Set(Prop prop, string? value)
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
            sb.Append(indent);

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
                        sb.Append(' ');
                    }
                    sb.Append(namePart);
                }
            }

            return sb;
        }

        #region Interfaces

        #region ICleanable

        /// <summary>
        /// Gibt an, ob das Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte
        /// <see cref="Clean"/> aufgerufen werden.
        /// </summary>
        public bool IsEmpty => _propDic.Count == 0;


        /// <summary>
        /// Entfernt leere Strings und überflüssige Leerzeichen.
        /// </summary>
        public void Clean()
        {
            var props = _propDic.ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                var kvp = props[i];
                Set(kvp.Key, StringCleaner.CleanDataEntry(kvp.Value));
            }

#if !NET40
            _propDic.TrimExcess();
#endif
        }


        #endregion


        #region ICloneable

        /// <summary>
        /// Erstellt eine tiefe Kopie des Objekts.
        /// </summary>
        /// <returns>Eine tiefe Kopie des Objekts.</returns>
        public object Clone()
        {
            return new Name(this);
        }

        #endregion


        #region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
        /// <summary>
        /// Vergleicht die Instanz mit einem anderen <see cref="object"/> um festzustellen,
        /// ob es sich bei <paramref name="obj"/> um ein <see cref="Name"/>-Objekt handelt, das denselben Namen darstellt.
        /// </summary>
        /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte <see cref="Name"/>-Objekte sind, die denselben Namen darstellen.</returns>
        public override bool Equals(object? obj)
        {
            if (!(obj is Name p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen <see cref="Name"/>-Objekt, um zu ermitteln,
        /// ob beide denselben Namen darstellen.
        /// </summary>
        /// <param name="other">Das <see cref="Name"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte denselben Namen darstellen.</returns>
        public bool Equals(Name? other)
        {
            // If parameter is null return false:
            if (other is null) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, other)) return true;

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

#if NET40
            var culture = CultureInfo.CurrentUICulture;
#else
            var comparison = StringComparison.CurrentCultureIgnoreCase;
#endif

            ModifyHash(LastName);
            ModifyHash(FirstName);
            ModifyHash(Suffix);

            void ModifyHash(string? s)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
#if NET40
                    hash ^= s.ToUpper(culture).GetHashCode();

#else
                    hash ^= s.GetHashCode(comparison);

#endif
                }
            }


            return hash;
        }


        #region Überladen von == und !=
        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <remarks>
        /// Vergleicht <paramref name="name1"/> und <paramref name="name2"/> um festzustellen, ob diese denselben Namen darstellen.
        /// </remarks>
        /// <param name="name1">Linker Operand.</param>
        /// <param name="name2">Rechter Operand.</param>
        /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> auf denselben Namen darstellen.</returns>
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
                return !(name2 is null) && name1.CompareBoolean(name2);
            }
        }

        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <remarks>
        /// Vergleicht <paramref name="name1"/> und <paramref name="name2"/> um festzustellen, ob diese verschiedene Namen darstellen.
        /// </remarks>
        /// <param name="name1">Linker Operand.</param>
        /// <param name="name2">Rechter Operand.</param>
        /// <returns><c>true</c>, wenn <paramref name="name1"/> und <paramref name="name2"/> verschiedene Namen darstellen.</returns>
        public static bool operator !=(Name? name1, Name? name2)
        {
            return !(name1 == name2);
        }

        /// <summary>
        /// Vergleicht den Inhalt von this mit dem eines anderen <see cref="Name"/>-Objekts, um zu ermitteln,
        /// ob beide den Namen derselben physischen Person darstellen.
        /// </summary>
        /// <param name="other">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte auf den Namen derselben Person verweisen.</returns>
        private bool CompareBoolean(Name other)
        {

            var comparer = StringComparer.CurrentCultureIgnoreCase;

            string? lastName = this.LastName;
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                string? otherLastName = other.LastName;
                if (!string.IsNullOrWhiteSpace(otherLastName) && !comparer.Equals(otherLastName, lastName))
                {
                    return false;
                }
            }

            string? firstName = this.FirstName;
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                string? otherFirstName = other.FirstName;
                if (!string.IsNullOrWhiteSpace(otherFirstName) && !comparer.Equals(otherFirstName, firstName))
                {
                    return false;
                }
            }

            string? middleName = this.MiddleName;
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                string? otherMiddleName = other.MiddleName;
                if (!string.IsNullOrWhiteSpace(otherMiddleName) && !comparer.Equals(otherMiddleName, middleName))
                {
                    return false;
                }
            }

            string? suffix = this.Suffix;
            string? otherSuffix = other.Suffix;
            if (string.IsNullOrWhiteSpace(suffix) && string.IsNullOrWhiteSpace(otherSuffix))
            {
                return true;
            }

            if (!comparer.Equals(otherSuffix, suffix))
            {
                return false;
            }

            return true;
        }
        #endregion

        #endregion

        #endregion


    }
}
