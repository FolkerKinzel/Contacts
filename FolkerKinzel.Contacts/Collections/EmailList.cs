using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net.Mail;


namespace FolkerKinzel.ContactData.Collections
{
    /// <summary>
    /// Die Klasse ist eine für die Speicherung von E-Mail-Adressen spezialisierte Collection. 
    /// Die Collection ist serialisierbar.
    /// </summary>
    [Serializable()]
    public sealed class EmailList : List<MailAddress?>, ICloneable
    {
        ///// <summary>
        ///// Regex zum Verifizieren von E-Mail-Adressen.
        ///// </summary>
        //public static readonly Regex EMAIL_REGEX = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
        //           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", RegexOptions.Compiled);

       

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der EmailList-Klasse.
        /// </summary>
        public EmailList() {}
        

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der EmailList-Klasse 
        /// mit der angegebenen Anfangskapazität.
        /// </summary>
        /// <param name="capacity">Die Anfangskapazität.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> ist kleiner als 0.</exception>
        public EmailList(int capacity) : base(capacity) {}
        

        /// <summary>
        /// Initialisiert eine neue Instanz der EmailList-Klasse, die die aus der angegebenen Auflistung kopierten Elemente 
        /// enthält und eine ausrechende Kapazität aufweist.
        /// </summary>
        /// <param name="collection">Die Auflistung mit den zu kopierenden Elementen.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> ist null.</exception>
        public EmailList(IEnumerable<MailAddress?> collection) : base(collection) {}


        /// <summary>
        /// Verschiebt das Element am angegebenen Index an eine neue Position in der Auflistung und löst das CollectionChanged-Event aus. Das 
        /// Event wird nicht ausgelöst, wenn <paramref name="oldIndex"/> gleich <paramref name="newIndex"/> ist.
        /// </summary>
        /// <param name="oldIndex">Der nullbasierte Index, der die Position des zu verschiebenden Elements angibt.</param>
        /// <param name="newIndex">Der nullbasierte Index, der die neue Position des Elements angibt.</param>
        /// <exception cref="ArgumentOutOfRangeException">Wird ausgelöst wenn <paramref name="oldIndex"/> oder <paramref name="newIndex"/> kleiner 0 sind, oder wenn <paramref name="oldIndex"/>
        /// oder <paramref name="newIndex"/> größer oder gleich der Anzahl der Elemente in der Collection ist.</exception>
        public void Move(int oldIndex, int newIndex)
        {
            var item = this[oldIndex];

            RemoveAt(oldIndex);
            Insert(newIndex, item);
        }


        # region Überladen des Gleichheitsoperators

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen.
        /// <summary>
        /// Vergleicht this mit einem anderen System.Object.
        /// </summary>
        /// <param name="obj">Das System.Object mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public override bool Equals(object? obj)
        {
            // If parameter cannot be cast to EmailList return false.
            // Der Cast nach object ist unbedingt nötig, um eine
            // Endlosschleife zu vermeiden.
            if (!(obj is EmailList p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen EmailList-Objekt.
        /// </summary>
        /// <param name="p">Das EmailList-Objekt mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public bool Equals(EmailList? p)
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
        public static bool operator ==(EmailList? os1, EmailList? os2)
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
        public static bool operator !=(EmailList? os1, EmailList? os2)
        {
            return !(os1 == os2);
        }

        /// <summary>
        /// Vergleicht den Inhalt  der Properties von this mit denen eines anderen EmailList-Objekts.
        /// </summary>
        /// <param name="p">Das EmailList-Objekt, mit dem verglichen wird.</param>
        /// <returns>True, wenn keine unterschiedlichen Inhalte gefunden werden.
        /// (null, string.empty und Whitespace werden als gleiche Inhalte behandelt.)</returns>
        private bool CompareBoolean(EmailList p)
        {
            if (this.Count != p.Count) return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] != p[i]) return false;
            }
            return true;
        }
        #endregion

        #endregion


        #region Interfaces

        #region IWabdata

        /// <summary>
        /// Gibt an, ob das Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte
        /// Clean() aufgerufen werden.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count != 0;
            }
        }



        /// <summary>
        /// Reinigt die Collection von ungültigen E-Mail-Adressen und Doubletten.
        /// </summary>
        public void Clean()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (this[i] == null)
                {
                    this.RemoveAt(i);
                    continue;
                }
            }

            for (int i = 0; i < this.Count - 1; i++)
            {
                for (int j = this.Count - 1; j > i; j--)
                {
                    if (this[i] == this[j])
                    {
                        this.RemoveAt(j);
                    }
                }
            }

            this.TrimExcess();
        }

        #endregion


        #region ICloneable

        /// <summary>
        /// Erstellt eine tiefe Kopie des Objekts.
        /// </summary>
        /// <returns>Eine tiefe Kopie des Objekts.</returns>
        public object Clone()
        {
            return new EmailList(this);
        }

        #endregion

        #endregion

 
    }
}
