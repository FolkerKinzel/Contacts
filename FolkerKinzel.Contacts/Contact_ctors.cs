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
    /// <code language="cs">
    /// using FolkerKinzel.Contacts;
    /// using System;
    /// 
    /// namespace Examples
    /// {
    ///     static class ContactExample
    ///     {
    ///         public static Contact[] InitializeContacts() => new Contact[]
    ///             {
    ///                 new Contact
    ///                 {
    ///                     DisplayName = "John Doe",
    ///                     Person = new Person
    ///                     {
    ///                         Name = new Name
    ///                         {
    ///                             FirstName = "John",
    ///                             MiddleName = "William",
    ///                             LastName = "Doe",
    ///                             Suffix = "jr."
    ///                         },
    /// 
    ///                         BirthDay = new DateTime(1972, 1, 3),
    ///                         Spouse = "Jane Doe",
    ///                         Anniversary = new DateTime(2001, 6, 15)
    ///                     },
    /// 
    ///                     Work = new Work
    ///                     {
    ///                         JobTitle = "Facility Manager",
    ///                         Company = "Contoso"
    ///                     },
    /// 
    ///                     PhoneNumbers = new PhoneNumber[]
    ///                     {
    ///                         new PhoneNumber
    ///                         {
    ///                             Value = "0123-45678",
    ///                             IsWork = true
    ///                         }
    ///                     },
    /// 
    ///                     EmailAddresses = new string[]
    ///                     {
    ///                         "john.doe@contoso.com"
    ///                     }
    ///                 },//new Contact()
    /// 
    ///                 ///////////
    /// 
    ///                 new Contact
    ///                 {
    ///                     DisplayName = "Jane Doe",
    ///                     Person = new Person
    ///                     {
    ///                         Name = new Name
    ///                         {
    ///                             FirstName = "Jane",
    ///                             LastName = "Doe",
    ///                             Prefix = "Dr."
    ///                         },
    ///                         BirthDay = new DateTime(1981, 5, 4),
    ///                         Spouse = "John Doe",
    ///                         Anniversary = new DateTime(2001, 6, 15)
    ///                     },
    /// 
    ///                     Work = new Work
    ///                     {
    ///                         JobTitle = "CEO",
    ///                         Company = "Contoso"
    ///                     },
    /// 
    ///                     PhoneNumbers = new PhoneNumber[]
    ///                     {
    ///                         new PhoneNumber
    ///                         {
    ///                             Value = "876-54321",
    ///                             IsMobile = true
    ///                         }
    ///                     }
    ///                 }//new Contact()
    ///             };//new Contact[]
    ///     }
    /// }
    /// </code>
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