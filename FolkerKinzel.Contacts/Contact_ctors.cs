using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Net.Mail;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Einfaches Datenmodell zum Speichern von Kontaktdaten.
    /// </summary>
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
                    IEnumerable<string?> strings => strings.ToList(),
                    ICloneable adr => adr.Clone(),
                    _ => kvp.Value,
                };
            }
        }

    }//class
}//ns