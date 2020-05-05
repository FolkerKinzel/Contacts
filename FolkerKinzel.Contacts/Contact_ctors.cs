using System;
using System.Collections.Generic;
using System.Linq;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Einfaches Datenmodell zum Speichern von Kontaktdaten.
    /// </summary>
    /// <example>
    /// <para>Initialisieren von <see cref="Contact"/>-Objekten:</para>
    /// <code language="cs" source="..\Examples\ContactExample.cs" />
    /// </example>
    public sealed partial class Contact
    {
        /// <summary>
        /// Initialisiert eine neue, leere Instanz der <see cref="Contact"/>-Klasse.
        /// </summary>
        public Contact() { }


        /// <summary>
        /// Kopierkonstruktor: Erstellt eine tiefe Kopie des Objekts und aller seiner Unterobjekte.
        /// </summary>
        /// <param name="source">Quellobjekt, dessen Inhalt kopiert wird.</param>
        private Contact(Contact source)
        {
            foreach (var kvp in source._propDic)
            {
                this._propDic[kvp.Key] = kvp.Value switch
                {
                    IEnumerable<PhoneNumber?> phones => phones.Select(x => (PhoneNumber?)x?.Clone()).ToList(),
                    ICloneable adr => adr.Clone(),
                    IEnumerable<string?> strings => strings.ToList(),
                    _ => kvp.Value,
                };
            }
        }

    }//class
}//ns