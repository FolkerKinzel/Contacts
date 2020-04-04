using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Die Klasse kapselt Adressdaten.
    /// </summary>
    //[Serializable()]
    public sealed class Address : ICloneable, ICleanable, IEquatable<Address>
    {
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

        #region private Felder

        private readonly Dictionary<Prop, string> _propDic = new Dictionary<Prop, string>();

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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> ist null.</exception>
        private Address(Address source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (var kvp in source._propDic)
            {
                this._propDic[kvp.Key] = kvp.Value;
            }
        }

        #endregion



        private string? Get(Prop prop)
        {
            return _propDic.ContainsKey(prop) ? (string?)_propDic[prop] : null;
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

#region öffentliche Eigenschaften und Methoden


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
        /// Erstellt eine String-Repräsentation des <see cref="Address"/>-Objekts
        /// </summary>
        /// <returns>Der Inhalt des <see cref="Address"/>-Objekts als <see cref="string"/>.</returns>
        public override string ToString() => AppendTo(new StringBuilder()).ToString();
        

        
        internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
        {
            bool writeLineBreak = true;
            bool writeIndent = true;

            foreach (var key in _propDic.Keys.OrderBy(x => x))
            {
                switch (key)
                {
                    case Prop.Street:
                        sb.Append(indent).AppendLine(Street);
                        writeLineBreak = false;
                         break;
                    case Prop.PostalCode:
                        sb.Append(indent).Append(PostalCode);
                        writeLineBreak = true;
                        writeIndent = false;
                        break;
                    case Prop.City:
                        if (writeIndent)
                        {
                            sb.Append(indent).AppendLine(City);
                        }
                        else
                        {
                            sb.Append(' ').AppendLine(City);
                            writeIndent = true;
                        }
                        writeLineBreak = false;
                        break;
                    case Prop.State:
                        if (writeLineBreak)
                        {
                            sb.AppendLine();
                        }
                        sb.Append(indent).AppendLine(State);
                        writeLineBreak = false;
                        break;
                    case Prop.Country:
                        if (writeLineBreak)
                        {
                            sb.AppendLine();
                        }
                        sb.Append(indent).AppendLine(Country);
                        writeLineBreak = false;
                        break;
                    
                    default:
                        break;
                }
            }

            if(!writeLineBreak)
            {
                sb.Length -= Environment.NewLine.Length;
            }

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
            return new Address(this);
        }

        #endregion


#region ICleanable

        /// <summary>
        /// Gibt an, ob das <see cref="Address"/>-Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte <see cref="Clean"/>
        /// aufgerufen werden.
        /// </summary>
        public bool IsEmpty
        {
            get => _propDic.Count == 0;
        }

        /// <summary>
        /// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
        /// und leere Unterobjekte auf <c>null</c>.
        /// </summary>
        public void Clean()
        {
            var keys = this._propDic.Keys.ToArray();

            foreach (var key in keys)
            {
                Set(key, StringCleaner.CleanDataEntry(this._propDic[key]));
            }

#if !NET40
            _propDic.TrimExcess();
#endif
        }

        #endregion


#region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen
        /// <summary>
        /// Vergleicht this mit <paramref name="obj"/>,
        /// um zu bestimmen, ob <paramref name="obj"/> ein <see cref="Address"/>-Objekt ist, das
        /// eine identische Adresse darstellt.
        /// </summary>
        /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Address"/>-Objekt ist, das dieselbe Adresse darstellt.</returns>
        public override bool Equals(object? obj)
        {
            // If parameter cannot be cast to WabAddress return false.
            if (!(obj is Address p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }



        /// <summary>
        /// Vergleicht this mit einem anderen <see cref="Address"/>-Objekt,
        /// um zu bestimmen, ob <paramref name="other"/> eine identische Adresse ist.
        /// </summary>
        /// <param name="other">Das <see cref="Address"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Adresse darstellt.</returns>
        public bool Equals(Address? other)
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

            if (this.IsEmpty) return hash;

#if !NET40
            var comparison = StringComparison.OrdinalIgnoreCase;
#endif

            ModifyHash(PostalCode);

            if (hash != -1)
            {
                ModifyHash(Street);
                return hash;
            }

            ModifyHash(City);
            ModifyHash(Street);

            return hash;


            void ModifyHash(string? s)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
#if NET40
                    hash ^= s.ToUpperInvariant().GetHashCode();
#else
                    hash ^= s.GetHashCode(comparison);
#endif
                }
            }
        }


        #region Überladen von == und !=

        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <param name="os1">linker Operand</param>
        /// <param name="os2">rechter Operand</param>
        /// <returns>true, wenn gleich</returns>
        public static bool operator ==(Address? os1, Address? os2)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(os1, os2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (os1 is null)
                return false; // auf Referenzgleichheit wurde oben geprüft
            else
                return os2 is null ? false : os1.CompareBoolean(os2);
        }

        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <param name="os1">linker Operand</param>
        /// <param name="os2">rechter Operand</param>
        /// <returns>true, wenn ungleich</returns>
        public static bool operator !=(Address? os1, Address? os2)
        {
            return !(os1 == os2);
        }

        /// <summary>
        /// Vergleicht den Inhalt der Properties <see cref="PostalCode"/>, <see cref="Street"/> und <see cref="City"/>,
        /// um zu bestimmen, ob <paramref name="other"/> eine identische Adresse ist.
        /// </summary>
        /// <param name="other">Das <see cref="Address"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Adresse darstellt.</returns>
        private bool CompareBoolean(Address other)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            string? postalCode = PostalCode;
            string? otherPostalCode = other.PostalCode;

            if (!string.IsNullOrWhiteSpace(postalCode) && !string.IsNullOrWhiteSpace(otherPostalCode))
            {
                if (!comparer.Equals(postalCode, otherPostalCode))
                {
                    return false;
                }

                string? street = Street;
                string? otherStreet = other.Street;

                if (!string.IsNullOrWhiteSpace(street) && !string.IsNullOrWhiteSpace(otherStreet))
                {
                    return comparer.Equals(street, otherStreet);
                }

            }
            else
            {
                string? city = City;
                string? otherCity = other.City;

                if (!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(otherCity))
                {
                    if (!comparer.Equals(city, otherCity))
                    {
                        return false;
                    }

                    string? street = Street;
                    string? otherStreet = other.Street;

                    if (!string.IsNullOrWhiteSpace(street) && !string.IsNullOrWhiteSpace(otherStreet))
                    {
                        return comparer.Equals(street, otherStreet);
                    }
                }
            }

            return true;


        }
        #endregion

        #endregion

#endregion


    }//class
}//ns
