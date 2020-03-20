using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FolkerKinzel.Contacts
{
    public sealed partial class Contact
    {
        private readonly Dictionary<Prop, object> _propDic = new Dictionary<Prop, object>();


        private enum Prop
        {
            DisplayName,
            Person,
            AddressHome,
            EmailAdresses,
            PhoneNumbers,
            InstantMessengerHandles,
            HomePagePersonal,
            HomePageWork,
            Work,
            Comment,
            TimeStamp
        }

        [return: MaybeNull]
        private T Get<T>(Prop prop)
        {
            return _propDic.ContainsKey(prop) ? (T)_propDic[prop] : default;
        }


        private void Set(Prop prop, object? value)
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
        /// Bezeichnung des <see cref="Contact"/>s (entspricht der Anzeige im Adressbuch).
        /// </summary>
        public string? DisplayName
        {
            get => Get<string?>(Prop.DisplayName);
            set => Set(Prop.DisplayName, value);
        }



        /// <summary>
        /// Personenbezogene Daten.
        /// </summary>
        public Person? Person
        {
            get => Get<Person?>(Prop.Person);
            set => Set(Prop.Person, value);
        }

        /// <summary>
        /// E-Mail-Adressen
        /// </summary>
        public IEnumerable<string?>? EmailAddresses
        {
            get => Get<IEnumerable<string?>?>(Prop.EmailAdresses);
            set => Set(Prop.EmailAdresses, value);
        }

        /// <summary>
        /// Instant-Messenger-Handles
        /// </summary>
        public IEnumerable<string?>? InstantMessengerHandles
        {
            get => Get<IEnumerable<string?>?>(Prop.InstantMessengerHandles);
            set => Set(Prop.InstantMessengerHandles, value); 
        }

        /// <summary>
        /// Telefonnummern
        /// </summary>
        public IEnumerable<PhoneNumber?>? PhoneNumbers
        {
            get => Get<IEnumerable<PhoneNumber?>?>(Prop.PhoneNumbers);
            set => Set(Prop.PhoneNumbers, value);
        }

        /// <summary>
        /// Postanschrift (privat)
        /// </summary>
        public Address? AddressHome
        {
            get => Get<Address?>(Prop.AddressHome);
            set => Set(Prop.AddressHome, value);
        }

        /// <summary>
        /// Berufliche Daten.
        /// </summary>
        public Work? Work
        {
            get => Get<Work?>(Prop.Work);
            set => Set(Prop.Work, value);
        }

        /// <summary>
        /// Private Homepage
        /// </summary>
        public string? HomePagePersonal
        {
            get => Get<string?>(Prop.HomePagePersonal);
            set => Set(Prop.HomePagePersonal, value);
        }

        /// <summary>
        /// Firmenwebseite
        /// </summary>
        public string? HomePageWork
        {
            get => Get<string?>(Prop.HomePageWork);
            set => Set(Prop.HomePageWork, value);
        }

        /// <summary>
        /// Kommentar
        /// </summary>
        public string? Comment
        {
            get => Get<string?>(Prop.Comment);
            set => Set(Prop.Comment, value);
        }

        /// <summary>
        /// Letzte Änderung
        /// </summary>
        public DateTime TimeStamp
        {
            get => Get<DateTime>(Prop.TimeStamp);
            set => Set(Prop.TimeStamp, value == default ? null : (object?)value);
        }





    }
}
