using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolkerKinzel.ContactData.Enums;

namespace FolkerKinzel.ContactData.Collections
{
    /// <summary>
    /// Die Klasse ist ein für die Speicherung von Telefonnummern spezialisiertes Dictionary. 
    /// Die Collection ist serialisierbar.
    /// </summary>
    [Serializable()]
    public sealed class HomePageDictionary : Dictionary<HomePageMapping, HomePageUrlProvider?>
    {
        #region ctor

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der HomePageDictionary-Klasse.
        /// </summary>
        public HomePageDictionary() { }

        /// <summary>
        /// Initialisiert eine neue, leere Instanz der HomePageDictionary-Klasse 
        /// mit der angegebenen Anfangskapazität.
        /// </summary>
        /// <param name="capacity">Die Anfangskapazität.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> ist kleiner als 0.</exception>
        public HomePageDictionary(int capacity) : base(capacity) { }


        /// <summary>
        /// Initialisiert eine neue Instanz der HomePageDictionary-Klasse, die die aus der angegebenen Auflistung kopierten Elemente 
        /// enthält und eine ausrechende Kapazität aufweist.
        /// </summary>
        /// <param name="dictionary">Die Auflistung mit den zu kopierenden Elementen.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> ist null.</exception>
        public HomePageDictionary(IDictionary<HomePageMapping, HomePageUrlProvider?> dictionary) : base(dictionary) 
        {
            // Die HomePageUrlProvider-Objekte müssen nicht geklont werden, da sie schreibgeschützt sind.
        }


        #endregion

        /// <summary>
        /// Erstellt eine String-Repräsentation des HomePageDictionary-Objekts
        /// </summary>
        /// <returns>Der Inhalt der Collection als String.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(128);
            ToString(sb);
            return sb.ToString();
        }

        internal void ToString(StringBuilder sb)
        {
            var keys = this.Keys.ToArray();
            Array.Sort(keys);

            foreach (var key in keys)
            {
                switch (key)
                {
                    case HomePageMapping.Personal:
                        {
                            HomePageUrlProvider? uri = this[key];
                            if (uri != null)
                            {
                                sb.Append("Homepage (privat):     ");
                                sb.AppendLine(uri.ToString());
                            }
                            break;
                        }

                    case HomePageMapping.Business:
                        {
                            HomePageUrlProvider? uri = this[key];
                            if (uri != null)
                            {
                                sb.Append("Homepage (geschäftl.): ");
                                sb.AppendLine(uri.ToString());
                            }
                            break;
                        }
                }//switch
            }//foreach
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
            if (!(obj is HomePageDictionary p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht this mit einem anderen HomePageDictionary-Objekt.
        /// </summary>
        /// <param name="p">Das EmailList-Objekt mit dem verglichen wird.</param>
        /// <returns>true, wenn gleich</returns>
        public bool Equals(HomePageDictionary? p)
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
        public static bool operator ==(HomePageDictionary? os1, HomePageDictionary? os2)
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
        public static bool operator !=(HomePageDictionary? os1, HomePageDictionary? os2)
        {
            return !(os1 == os2);
        }

        /// <summary>
        /// Vergleicht den Inhalt  der Properties von this mit denen eines anderen HomePageDictionary-Objekts.
        /// </summary>
        /// <param name="p">Das HomePageDictionary-Objekt, mit dem verglichen wird.</param>
        /// <returns>True, wenn keine unterschiedlichen Inhalte gefunden werden.
        /// (null, string.empty und Whitespace werden als gleiche Inhalte behandelt.)</returns>
        private bool CompareBoolean(HomePageDictionary p)
        {
            var thisKeys = this.Keys.ToArray();
            var pKeys = p.Keys.ToArray();

            if (pKeys.Length != thisKeys.Length) return false;

            Array.Sort(thisKeys);
            Array.Sort(pKeys);

            if (!thisKeys.SequenceEqual(pKeys)) return false;

            foreach (var kvp in this)
            {
                if (this[kvp.Key] != p[kvp.Key]) return false;
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
        /// Reinigt die Collection von null-Werten.
        /// </summary>
        public void Clean()
        {
            var keys = this.Keys.ToArray();

            foreach (var key in keys)
            {
                var uri = this[key];
                if (uri == null)
                {
                    this.Remove(key);
                }
            }
        }

        #region ICloneable

        /// <summary>
        /// Erstellt eine tiefe Kopie des Objekts.
        /// </summary>
        /// <returns>Eine tiefe Kopie des Objekts.</returns>
        public object Clone()
        {
            return new HomePageDictionary(this);
        }

        #endregion

        #endregion


        #endregion

    }
}
