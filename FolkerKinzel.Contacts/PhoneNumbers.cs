using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Die Klasse ist ein für die Speicherung von Telefonnummern spezialisiertes Dictionary. 
    /// Die Collection ist serialisierbar.
    /// </summary>
    [Serializable()]
    public sealed class PhoneNumbers : ICloneable, ICleanable, IEquatable<PhoneNumbers>
    {
        private enum PhoneNumberMapping
        {
            Phone,
            MobilePhone,
            OtherPhone,
            Pager,
            PhoneWork,
            Fax,
            FaxWork
        }


        private readonly Dictionary<PhoneNumberMapping, string> _propDic = new Dictionary<PhoneNumberMapping, string>();


        private string? Get(PhoneNumberMapping prop)
        {
            return _propDic.ContainsKey(prop) ? _propDic[prop] : null;
        }


        private void Set(PhoneNumberMapping prop, string? value)
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
        /// Festnetz (privat)
        /// </summary>
        public string? Phone
        {
            get => Get(PhoneNumberMapping.Phone);
            set => Set(PhoneNumberMapping.Phone, value);
        }

        /// <summary>
        /// Handy
        /// </summary>
        public string? MobilePhone
        {
            get => Get(PhoneNumberMapping.MobilePhone);
            set => Set(PhoneNumberMapping.MobilePhone, value);
        }

        /// <summary>
        /// Alternative Telefonnummer
        /// </summary>
        public string? OtherPhone
        {
            get => Get(PhoneNumberMapping.OtherPhone);
            set => Set(PhoneNumberMapping.OtherPhone, value);
        }

        /// <summary>
        /// Pager
        /// </summary>
        public string? Pager
        {
            get => Get(PhoneNumberMapping.Pager);
            set => Set(PhoneNumberMapping.Pager, value);
        }

        /// <summary>
        /// Telefon (dienstlich)
        /// </summary>
        public string? PhoneWork
        {
            get => Get(PhoneNumberMapping.PhoneWork);
            set => Set(PhoneNumberMapping.PhoneWork, value);
        }

        /// <summary>
        /// Fax (privat)
        /// </summary>
        public string? Fax
        {
            get => Get(PhoneNumberMapping.Fax);
            set => Set(PhoneNumberMapping.Fax, value);
        }

        /// <summary>
        /// Fax (dienstlich)
        /// </summary>
        public string? FaxWork
        {
            get => Get(PhoneNumberMapping.FaxWork);
            set => Set(PhoneNumberMapping.FaxWork, value);
        }


        #region ctor

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der PhoneNumbers-Klasse.
        /// </summary>
        public PhoneNumbers() { }

        ///// <summary>
        ///// Initialisiert eine neue, leere Instanz der PhoneNumbersDictionary-Klasse 
        ///// mit der angegebenen Anfangskapazität.
        ///// </summary>
        ///// <param name="capacity">Die Anfangskapazität.</param>
        ///// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> ist kleiner als 0.</exception>
        //public PhoneNumbers(int capacity) : base(capacity) { }


        /// <summary>
        /// Initialisiert eine neue Instanz der PhoneNumbers-Klasse, die die aus der angegebenen Auflistung kopierten Elemente 
        /// enthält und eine ausrechende Kapazität aufweist.
        /// </summary>
        /// <param name="phoneNumbers">Die Auflistung mit den zu kopierenden Elementen.</param>
        /// <exception cref="ArgumentNullException"><paramref name="phoneNumbers"/> ist null.</exception>
        public PhoneNumbers(PhoneNumbers phoneNumbers)
        {
            if(phoneNumbers is null)
            {
                throw new ArgumentNullException(nameof(phoneNumbers));
            }

            foreach (var kvp in phoneNumbers._propDic)
            {
                this._propDic[kvp.Key] = kvp.Value;
            }
        }


        #endregion

        /// <summary>
        /// Erstellt eine String-Repräsentation des <see cref="PhoneNumbers"/>-Objekts.
        /// </summary>
        /// <returns>Der Inhalt des <see cref="PhoneNumbers"/>-Objekts als <see cref="string"/>.</returns>
        public override string ToString() => AppendTo(new StringBuilder()).ToString();
        

        internal StringBuilder AppendTo(StringBuilder sb)
        {
            foreach (var key in _propDic.Keys.OrderBy(x => x))
            {
                switch (key)
                {
                    case PhoneNumberMapping.Phone:
                        sb.Append("Tel. (privat):   ").AppendLine(Phone);
                        break;

                    case PhoneNumberMapping.MobilePhone:
                        sb.Append("Handy:           ").AppendLine(MobilePhone);
                        break;

                    case PhoneNumberMapping.OtherPhone:
                        sb.Append("Andere Tel.-Nr.: ").AppendLine(OtherPhone);
                        break;

                    case PhoneNumberMapping.Fax:
                        sb.Append("Fax (privat):    ").AppendLine(Fax);
                        break;

                    case PhoneNumberMapping.Pager:
                        sb.Append("Pager:           ").AppendLine(Pager);
                        break;

                    case PhoneNumberMapping.PhoneWork:
                        sb.Append("Tel. (dienstl.): ").AppendLine(PhoneWork);
                        break;

                    case PhoneNumberMapping.FaxWork:
                        sb.Append("Fax  (dienstl.): ").AppendLine(FaxWork);
                        break;
                }//switch
            }//foreach

            return sb;
        }


        #region Interfaces

        #region ICleanable

        /// <summary>
        /// Gibt an, ob das Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte
        /// <see cref="Clean"/> aufgerufen werden.
        /// </summary>
        public bool IsEmpty
        {
            get => this._propDic.Count == 0;
        }



        /// <summary>
        /// Reinigt die Collection von null-Werten und leeren Strings und entfernt überflüssige Leerzeichen.
        /// </summary>
        public void Clean()
        {
            var keys = this._propDic.Keys.ToArray();

            foreach (var key in keys)
            {
                string? val = StringCleaner.CleanComment(this._propDic[key]);

                if(val is null)
                {
                    _propDic.Remove(key);
                }
                else
                {
                    _propDic[key] = val;
                }
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
            return new PhoneNumbers(this);
        }

        #endregion


        #region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
        /// <summary>
        /// Vergleicht this mit einem anderen System.Object.
        /// </summary>
        /// <param name="obj">Das System.Object mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public override bool Equals(object? obj)
        {
            if (!(obj is PhoneNumbers p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen PhoneNumbersDictionary-Objekt.
        /// </summary>
        /// <param name="p">Das EmailList-Objekt mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public bool Equals(PhoneNumbers? p)
        {
            // If parameter is null return false:
            // Der Cast nach object ist unbedingt nötig, um eine
            // Endlosschleife zu vermeiden.
            if (p is null) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, p)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Erzeugt einen Hashcode für das Objekt.
        /// </summary>
        /// <returns>Der Hashcode.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        #region Überladen von == und !=
        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <param name="os1">linker Operand</param>
        /// <param name="os2">rechter Operand</param>
        /// <returns>true, wenn gleich</returns>
        public static bool operator ==(PhoneNumbers? os1, PhoneNumbers? os2)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(os1, os2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            // Der Cast nach object ist unbedingt nötig, um eine
            // Endlosschleife zu vermeiden.
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
        public static bool operator !=(PhoneNumbers? os1, PhoneNumbers? os2)
        {
            return !(os1 == os2);
        }

        /// <summary>
        /// Vergleicht den Inhalt  der Properties von this mit denen eines anderen PhoneNumbersDictionary-Objekts.
        /// </summary>
        /// <param name="p">Das PhoneNumbersDictionary-Objekt, mit dem verglichen wird.</param>
        /// <returns>True, wenn keine unterschiedlichen Inhalte gefunden werden.
        /// (null, string.empty und Whitespace werden als gleiche Inhalte behandelt.)</returns>
        private bool CompareBoolean(PhoneNumbers p)
        {
            if (this._propDic.Count != p._propDic.Count) return false;

            var thisKeys = _propDic.Keys.ToArray();
            var pKeys = p._propDic.Keys.ToArray();

            Array.Sort(thisKeys);
            Array.Sort(pKeys);

            if (!thisKeys.SequenceEqual(pKeys)) return false;

            foreach (var key in thisKeys)
            {
                if (!this._propDic[key].Equals(p._propDic[key], StringComparison.Ordinal)) return false;
            }
            return true;
        }
        #endregion

        #endregion

        #endregion

    }
}
