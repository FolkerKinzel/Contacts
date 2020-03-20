using FolkerKinzel.Contacts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Kapselt Informationen über eine Telefonnummer.
    /// </summary>
    public sealed class PhoneNumber : ICleanable, ICloneable, IEquatable<PhoneNumber>
    {
        [Flags]
        private enum Flags
        {
            IsWork = 1,
            IsCell = 2,
            IsFax = 4
        }

        #region private Fields

        private Flags _flags;

        #endregion


        #region Ctors

        /// <summary>
        /// Initialisiert ein leeres <see cref="PhoneNumber"/>-Objekt.
        /// </summary>
        public PhoneNumber() { }

        /// <summary>
        /// Initialisiert ein <see cref="PhoneNumber"/>-Objekt mit der zu kapselnden Telefonnummer
        /// und optionalen Flags, die diese näher beschreiben.
        /// </summary>
        /// <param name="number">Telefonnummer</param>
        /// <param name="isWork"><c>true</c> gibt an, dass es sich um eine dienstliche Telefonnummer handelt.</param>
        /// <param name="isCell"><c>true</c> gibt an, dass es sich um eine Mobilfunknummer handelt.</param>
        /// <param name="isFax"><c>true</c> gibt an, dass die Nummer für den Fax-Empfang geeignet ist.</param>
        public PhoneNumber(string? number, bool isWork = false, bool isCell = false, bool isFax = false)
        {
            this.Value = number;
            this.IsWork = isWork;
            this.IsCell = isCell;
            this.IsFax = isFax;
        }

        private PhoneNumber(PhoneNumber other)
        {
            this.Value = other.Value;
            this._flags = other._flags;
        }

        #endregion

        internal void Merge(PhoneNumber telNumber)
        {
            this._flags |= telNumber._flags;
        }


        #region public Properties and Methods

        /// <summary>
        /// Telefonnummer
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// Gibt an, ob es sich bei <see cref="Value"/> um eine dienstliche Telefonnummer handelt.
        /// </summary>
        public bool IsWork
        { 
            get => (_flags & Flags.IsWork) == Flags.IsWork ; 
            set => _flags =  (value ? _flags | Flags.IsWork : _flags & ~Flags.IsWork);
        }

        /// <summary>
        /// Gibt an, ob es sich bei <see cref="Value"/> um eine Mobilfunknummer handelt.
        /// </summary>
        public bool IsCell
        {
            get => (_flags & Flags.IsCell) == Flags.IsCell;
            set => _flags = (value ? _flags | Flags.IsCell : _flags & ~Flags.IsCell);
        }

        /// <summary>
        /// Gibt an, ob <see cref="Value"/> eine Telefonnummer ist, die für den Faxempfang geeignet ist.
        /// </summary>
        public bool IsFax
        {
            get => (_flags & Flags.IsFax) == Flags.IsFax;
            set => _flags = (value ? _flags | Flags.IsFax : _flags & ~Flags.IsFax);
        }


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="PhoneNumber"/>-Objekts.
        /// </summary>
        /// <returns>Der Inhalt des <see cref="PhoneNumber"/>-Objekts als <see cref="string"/>.</returns>
        public override string ToString() => AppendTo(new StringBuilder()).ToString();


        internal StringBuilder AppendTo(StringBuilder sb, string? indent = null)
        {
            sb.Append(indent);
            if (string.IsNullOrWhiteSpace(Value))
            {
                sb.Append('_');
                return sb;
            }
            else
            {
                sb.Append(Value);
            }
            
                
            bool closeBracket = false;
            

            if(IsFax)
            {
                sb.Append(" (").Append(Res.Fax);
                closeBracket = true;
            }

            if(IsWork)
            {
                if(closeBracket)
                {
                    sb.Append(", ").Append(Res.WorkShort);
                }
                else
                {
                    sb.Append(" (").Append(Res.WorkShort);
                    closeBracket = true;
                }
            }

            if(closeBracket)
            {
                sb.Append(')');
            }

            return sb;
        }

        #endregion


        #region Interfaces

        #region ICleanable

        /// <summary>
        /// Gibt an, ob das Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte
        /// <see cref="Clean"/> aufgerufen werden.
        /// </summary>
        public bool IsEmpty => this.Value is null;


        /// <summary>
        /// Entfernt leere Strings und überflüssige Leerzeichen.
        /// </summary>
        public void Clean() => this.Value = StringCleaner.CleanDataEntry(this.Value);
        

        #endregion


        #region ICloneable

        /// <summary>
        /// Erstellt eine tiefe Kopie des Objekts.
        /// </summary>
        /// <returns>Eine tiefe Kopie des Objekts.</returns>
        public object Clone()
        {
            return new PhoneNumber(this);
        }

        #endregion


        #region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
        /// <summary>
        /// Vergleicht this mit einem anderen <see cref="object"/>.
        /// </summary>
        /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte <see cref="PhoneNumber"/>-Objekte sind, die dieselbe Telefonnummer kapseln.</returns>
        public override bool Equals(object? obj)
        {
            if (!(obj is PhoneNumber p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen <see cref="PhoneNumber"/>-Objekt.
        /// </summary>
        /// <param name="phone">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte auf dieselbe Telefonnummer verweisen.</returns>
        public bool Equals(PhoneNumber? phone)
        {
            // If parameter is null return false:
            // Der Cast nach object ist unbedingt nötig, um eine
            // Endlosschleife zu vermeiden.
            if (phone is null) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, phone)) return true;

            // Return true if the fields match:
            return CompareBoolean(phone);
        }


        /// <summary>
        /// Erzeugt einen Hashcode für das Objekt.
        /// </summary>
        /// <returns>Der Hashcode.</returns>
        public override int GetHashCode()
        {
            int hash = -1;

            if (string.IsNullOrWhiteSpace(Value)) return hash;

            for (int i = 0; i < Value.Length; i++)
            {
                char c = Value[i];

                if(Char.IsLetterOrDigit(c))
                {
                    hash ^= c;
                }
            }

            return hash;
        }


        #region Überladen von == und !=
        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <param name="phone1">linker Operand</param>
        /// <param name="phone2">rechter Operand</param>
        /// <returns><c>true</c>, wenn <paramref name="phone1"/> und <paramref name="phone2"/> auf dieselbe Telefonnummer verweisen.</returns>
        public static bool operator ==(PhoneNumber? phone1, PhoneNumber? phone2)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(phone1, phone2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            // Der Cast nach object ist unbedingt nötig, um eine
            // Endlosschleife zu vermeiden.
            if (phone1 is null)
                return false; // auf Referenzgleichheit wurde oben geprüft
            else
                return phone2 is null ? false : phone1.CompareBoolean(phone2);
        }

        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <param name="phone1">linker Operand</param>
        /// <param name="phone2">rechter Operand</param>
        /// <returns><c>true</c>, wenn nicht beide Objekte auf dieselbe Telefonnummer verweisen.</returns>
        public static bool operator !=(PhoneNumber? phone1, PhoneNumber? phone2)
        {
            return !(phone1 == phone2);
        }

        /// <summary>
        /// Vergleicht den Inhalt der <see cref="Value"/>-Property von this mit denen eines anderen <see cref="PhoneNumber"/>-Objekts.
        /// </summary>
        /// <param name="other">Das <see cref="PhoneNumber"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn beide Objekte auf dieselbe Telefonnummer verweisen.</returns>
        private bool CompareBoolean(PhoneNumber other)
        { 
            if(this.Value == other.Value) return true;

            //if (this._flags != p._flags) return false;

            if (this.Value is null || other.Value is null)
            {
                return false;
            }

            List<char> thisChars = new List<char>();
            List<char> otherChars = new List<char>();

            for (int i = 0; i < this.Value.Length; i++)
            {
                char c = this.Value[i];
                if (Char.IsLetterOrDigit(c))
                {
                    thisChars.Add(c);
                }
            }

            for (int i = 0; i < other.Value.Length; i++)
            {
                char c = other.Value[i];
                if (Char.IsLetterOrDigit(c))
                {
                    otherChars.Add(c);
                }
            }

            return thisChars.SequenceEqual(otherChars);
        }
        #endregion

        #endregion

        #endregion


    }
}
