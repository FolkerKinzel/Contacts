using System;
using System.Collections.Generic;
using System.Linq;

namespace FolkerKinzel.Contacts
{
    public sealed partial class Contact : IEquatable<Contact>, ICloneable, ICleanable
    {

#region IEquatable

        //Überschreiben von Object.Equals um Vergleich zu ermöglichen
        /// <summary>
        /// Vergleicht den Inhalt der Properties von this mit denen eines anderen <see cref="object"/>s darauf,
        /// ob das andere <see cref="object"/> ein <see cref="Contact"/>-Objekt ist, das dieselbe Person oder Organisation repräsentieren könnte.
        /// </summary>
        /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Contact"/>-Objekt ist, das dieselbe Person oder Organisation repräsentiert.</returns>
        public override bool Equals(object? obj)
        {
            // If parameter cannot be cast to Contact return false.
            if (!(obj is Contact p)) return false;

            // Referenzgleichheit
            if (object.ReferenceEquals(this, obj)) return true;

            // Return true if the fields match:
            return CompareBoolean(p);
        }


        /// <summary>
        /// Vergleicht den Inhalt der Properties von this mit denen eines anderen <see cref="Contact"/>-Objekts darauf,
        /// ob das <see cref="Contact"/>-Objekt dieselbe Person oder Organisation repräsentieren könnte.
        /// </summary>
        /// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Person oder Organisation repräsentiert.</returns>
        public bool Equals(Contact? other)
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
            const int INITIAL_VALUE = -1;
            int hash = INITIAL_VALUE;

            if(this.IsEmpty)
            {
                return hash;
            }

            var person = this.Person;
            if(person != null && !person.IsEmpty)
            {
                return hash ^ person.GetHashCode();
            }

            var work = this.Work;
            if(work != null && !work.IsEmpty)
            {
                return hash ^ work.GetHashCode();
            }

            

            string? email = this.EmailAddresses?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (email != null)
            {
#if NET40
                return hash ^ email.GetHashCode();

#else
                return hash ^ email.GetHashCode(StringComparison.Ordinal);
#endif
            }

            string? im = this.InstantMessengerHandles?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
            if (im != null)
            {
#if NET40
                return hash ^ im.GetHashCode();

#else
                return hash ^ im.GetHashCode(StringComparison.Ordinal);
#endif
            }

            string? displayName = this.DisplayName;
            if(!string.IsNullOrWhiteSpace(displayName))
            {
#if NET40
                return hash ^ displayName.GetHashCode();

#else
                return hash ^ displayName.GetHashCode(StringComparison.CurrentCultureIgnoreCase);
#endif
            }

            if(hash == INITIAL_VALUE)
            {
                hash ^= 4711;
            }

            return hash;
        }


        // Überladen von == und !=
        /// <summary>
        /// Überladung des == Operators.
        /// </summary>
        /// <param name="cont1">linker Operand</param>
        /// <param name="cont2">rechter Operand</param>
        /// <returns><c>true</c>, wenn <paramref name="cont1"/> und <paramref name="cont2"/> dieselbe Person oder Organisation repräsentieren.</returns>
        public static bool operator ==(Contact? cont1, Contact? cont2)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(cont1, cont2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (cont1 is null)
                return false; // auf Referenzgleichheit wurde oben geprüft
            else
                return cont2 is null ? false : cont1.CompareBoolean(cont2);
        }

        /// <summary>
        /// Überladung des != Operators.
        /// </summary>
        /// <param name="cont1">linker Operand</param>
        /// <param name="cont2">rechter Operand</param>
        /// <returns><c>true</c>, wenn <paramref name="cont1"/> und <paramref name="cont2"/> verschiedene Personen oder Organisationen repräsentieren.</returns>
        public static bool operator !=(Contact? cont1, Contact? cont2)
        {
            return !(cont1 == cont2);
        }

        /// <summary>
        /// Vergleicht den Inhalt der Properties von this mit denen eines anderen <see cref="Contact"/>-Objekts darauf,
        /// ob das <see cref="Contact"/>-Objekt dieselbe Person oder Organisation repräsentieren könnte.
        /// </summary>
        /// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
        /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Person oder Organisation repräsentiert.</returns>
        private bool CompareBoolean(Contact other)
        {
            if (this.IsEmpty)
            {
                if(other.IsEmpty) return true;
            }
            else if(other.IsEmpty)
            {
                return false;
            }

            var person = this.Person;
            var otherPerson = other.Person;
            if(person != null && otherPerson != null && !person.IsEmpty && !otherPerson.IsEmpty)
            {
                return person == otherPerson;
            }

            var work = this.Work;
            var otherWork = other.Work;
            if (work != null && otherWork != null && !work.IsEmpty && !otherWork.IsEmpty)
            {
                return work == otherWork;
            }

            var emails = this.EmailAddresses;
            var otherEmails = other.EmailAddresses;
            if(emails != null && otherEmails != null)
            {
                var emailsArr = emails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                var otherEmailsArr = otherEmails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                if(emailsArr.Length != 0 && otherEmailsArr.Length != 0)
                {
                    if(emailsArr.Intersect(otherEmailsArr, StringComparer.Ordinal).Any())
                    {
                        return true;
                    }
                }
            }


            var ims = this.InstantMessengerHandles;
            var otherIMs = other.InstantMessengerHandles;
            if (ims != null && otherIMs != null)
            {
                var imsArr = ims.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                var otherIMsArr = otherIMs.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                if (imsArr.Length != 0 && otherIMsArr.Length != 0)
                {
                    if (imsArr.Intersect(otherIMsArr, StringComparer.Ordinal).Any())
                    {
                        return true;
                    }
                }
            }

            var displayName = this.DisplayName;
            var otherDisplayName = other.DisplayName;
            if(!string.IsNullOrWhiteSpace(this.DisplayName) && !string.IsNullOrWhiteSpace(otherDisplayName))
            {
                return StringComparer.CurrentCultureIgnoreCase.Equals(displayName, otherDisplayName);
            }


            var phones = this.PhoneNumbers;
            var otherPhones = other.PhoneNumbers;
            if (phones != null && otherPhones != null)
            {
                var phonesArr = phones.Where(x => x != null && !x.IsEmpty).ToArray();
                var otherPhonesArr = otherPhones.Where(x => x != null && !x.IsEmpty).ToArray();

                if (phonesArr.Length != 0 && otherPhonesArr.Length != 0)
                {
                    if (phonesArr.Intersect(otherPhonesArr).Any())
                    {
                        return true;
                    }
                }
            }

            var adr = this.AddressHome;
            var otherAdr = other.AddressHome;
            if(adr != null && otherAdr != null && !adr.IsEmpty && !otherAdr.IsEmpty)
            {
                return adr.Equals(otherAdr);
            }

            var homePagePersonal = this.WebPagePersonal;
            var otherHomePagePersonal = other.WebPagePersonal;
            if(!string.IsNullOrWhiteSpace(homePagePersonal) && !string.IsNullOrWhiteSpace(otherHomePagePersonal))
            {
                return StringComparer.Ordinal.Equals(homePagePersonal, otherHomePagePersonal);
            }

            var homePageWork = this.WebPageWork;
            var otherHomePageWork = other.WebPageWork;
            if (!string.IsNullOrWhiteSpace(homePageWork) && !string.IsNullOrWhiteSpace(otherHomePageWork))
            {
                return StringComparer.Ordinal.Equals(homePageWork, otherHomePageWork);
            }

            return true;
        }

#endregion


#region ICleanable


        /// <summary>
        /// Gibt an, ob das <see cref="Contact"/>-Objekt verwertbare Daten enthält. Vor dem Abfragen der Eigenschaft sollte <see cref="Clean"/>
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
            var props = _propDic.ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                var kvp = props[i];

                switch (kvp.Value)
                {
                    case IEnumerable<string?> strings:
                        {
                            string?[] arr = strings.Select(x => StringCleaner.CleanDataEntry(x)).Where(x => x != null).Distinct().ToArray();

                            if (arr.Length == 0)
                            {
                                    Set(kvp.Key, null);
                            }
                            else
                            {
                                    Set(kvp.Key, arr);
                            }
                        }
                        break;
                    case IEnumerable<PhoneNumber?> phoneNumbers:
                        {
                            List<PhoneNumber> numbers = phoneNumbers.Where(x => x != null).ToList()!;
                            numbers.ForEach(x => x.Clean());

                            var groups = numbers.GroupBy(x => x).ToArray();

                            numbers.Clear();

                            foreach (var group in groups)
                            {
                                if (!group.Key.IsEmpty)
                                {
                                    var number = group.Key;
                                    numbers.Add(number);

                                    foreach (var telNumber in group)
                                    {
                                        number.Merge(telNumber);
                                    }
                                }
                            }

                            if (numbers.Count == 0)
                            {
                                Set(kvp.Key, null);
                            }
                            else
                            {
                                numbers.TrimExcess();
                                Set(kvp.Key, numbers);
                            }
                        }
                        break;
                    case string s:
                        {
                            if (kvp.Key == Prop.Comment)
                            {
                                Set(kvp.Key, StringCleaner.CleanComment(s));
                            }
                            else
                            {
                                Set(kvp.Key, StringCleaner.CleanDataEntry(s));
                            }
                        }
                        break;
                    case DateTime dt when dt < new DateTime(1900, 1, 1):
                        {
                            Set(kvp.Key, null);
                        }
                        break;
                    case ICleanable adr:
                        {
                            adr.Clean();
                            if (adr.IsEmpty)
                            {
                                Set(kvp.Key, null);
                            }
                        }
                        break;
                    default:
                        break;
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
            return new Contact(this);
        }

#endregion
    }
}
