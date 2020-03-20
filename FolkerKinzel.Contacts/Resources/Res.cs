using System;
using System.Resources;


namespace FolkerKinzel.Contacts.Resources
{
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:CultureInfo angeben", Justification = "<Ausstehend>")]
    internal static class Res
    {
        private static global::System.Resources.ResourceManager? resourceMan;

        /// <summary>
        ///   Gibt die zwischengespeicherte <see cref="ResourceManager"/>-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        private static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan is null)
                {
                    var temp = new ResourceManager("FolkerKinzel.Contacts.Resources.Res", typeof(Res).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }


        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Home Address:" ähnelt.
        /// </summary>
        internal static string AddressHome => ResourceManager.GetString("AddressHome");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Work Address:" ähnelt.
        /// </summary>
        internal static string AddressWork => ResourceManager.GetString("AddressWork");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Anniversary:" ähnelt.
        /// </summary>
        internal static string Anniversary => ResourceManager.GetString("Anniversary");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Birthday:" ähnelt.
        /// </summary>
        internal static string BirthDay => ResourceManager.GetString("BirthDay");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Annotation:" ähnelt.
        /// </summary>
        internal static string Comment => ResourceManager.GetString("Comment");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Company:" ähnelt.
        /// </summary>
        internal static string Company => ResourceManager.GetString("Company");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Department:" ähnelt.
        /// </summary>
        internal static string Department => ResourceManager.GetString("Department");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Display Name:" ähnelt.
        /// </summary>
        internal static string DisplayName => ResourceManager.GetString("DisplayName");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "E-Mails:" ähnelt.
        /// </summary>
        internal static string EmailAddresses => ResourceManager.GetString("EmailAddresses");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "fax" ähnelt.
        /// </summary>
        internal static string Fax => ResourceManager.GetString("Fax");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "female" ähnelt.
        /// </summary>
        internal static string Female => ResourceManager.GetString("Female");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Gender:" ähnelt.
        /// </summary>
        internal static string Gender => ResourceManager.GetString("Gender");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Personal Homepage:" ähnelt.
        /// </summary>
        internal static string HomePagePersonal => ResourceManager.GetString("HomePagePersonal");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Business Homepage:" ähnelt.
        /// </summary>
        internal static string HomePageWork => ResourceManager.GetString("HomePageWork");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Instant Messengers:" ähnelt.
        /// </summary>
        internal static string InstantMessengers => ResourceManager.GetString("InstantMessengers");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "male" ähnelt.
        /// </summary>
        internal static string Male => ResourceManager.GetString("Male");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Name:" ähnelt.
        /// </summary>
        internal static string Name => ResourceManager.GetString("Name");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Nickname:" ähnelt.
        /// </summary>
        internal static string NickName => ResourceManager.GetString("NickName");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Office:" ähnelt.
        /// </summary>
        internal static string Office => ResourceManager.GetString("Office");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Personal Data:" ähnelt.
        /// </summary>
        internal static string Person => ResourceManager.GetString("Person");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Phone Numbers:" ähnelt.
        /// </summary>
        internal static string PhoneNumbers => ResourceManager.GetString("PhoneNumbers");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Position:" ähnelt.
        /// </summary>
        internal static string Position => ResourceManager.GetString("Position");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Spouse:" ähnelt.
        /// </summary>
        internal static string Spouse => ResourceManager.GetString("Spouse");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Last Changed:" ähnelt.
        /// </summary>
        internal static string TimeStamp => ResourceManager.GetString("TimeStamp");

        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "Company Data:" ähnelt.
        /// </summary>
        internal static string Work => ResourceManager.GetString("Work");


        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die "w." ähnelt.
        /// </summary>
        internal static string WorkShort => ResourceManager.GetString("WorkShort");
    }
}
