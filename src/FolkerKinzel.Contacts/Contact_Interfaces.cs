using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : IEquatable<Contact>, ICloneable, ICleanable, IIdentityComparer<Contact>
{
    #region IIdentityComparer

    public bool CanBeMergedWith(Contact? other) => other is null || IsEmpty || other.IsEmpty || !BelongsToOtherIdentity(other);


    private bool BelongsToOtherIdentity(Contact other)
    {
        if (!(Person?.CanBeMergedWith(other.Person) ?? true))
        {
            return true;
        }

        if (!(Work?.CanBeMergedWith(other.Work) ?? true))
        {
            return true;
        }

        IEnumerable<string?>? emails = this.EmailAddresses;
        IEnumerable<string?>? otherEmails = other.EmailAddresses;
        if (emails != null && otherEmails != null)
        {
            var emailsArr = emails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var otherEmailsArr = otherEmails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (emailsArr.Length != 0 && otherEmailsArr.Length != 0)
            {
                if (emailsArr.Intersect(otherEmailsArr, StringComparer.Ordinal).Any())
                {
                    return false;
                }
            }
        }

        IEnumerable<string?>? ims = this.InstantMessengerHandles;
        IEnumerable<string?>? otherIMs = other.InstantMessengerHandles;
        if (ims != null && otherIMs != null)
        {
            var imsArr = ims.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var otherIMsArr = otherIMs.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (imsArr.Length != 0 && otherIMsArr.Length != 0)
            {
                if (imsArr.Intersect(otherIMsArr, StringComparer.Ordinal).Any())
                {
                    return false;
                }
            }
        }

        if (ItemStripper.StartEqual(DisplayName, other.DisplayName, true))
        {
            return false;
        }

        IEnumerable<PhoneNumber?>? phones = this.PhoneNumbers;
        IEnumerable<PhoneNumber?>? otherPhones = other.PhoneNumbers;
        if (phones != null && otherPhones != null)
        {
            PhoneNumber[] phonesArr = phones.Where(x => x != null && !x.IsEmpty).ToArray()!;
            PhoneNumber[] otherPhonesArr = otherPhones.Where(x => x != null && !x.IsEmpty).ToArray()!;

            if (phonesArr.Length != 0 && otherPhonesArr.Length != 0)
            {
                if (phonesArr.Intersect(otherPhonesArr, PhoneNumberComparer.Instance).Any())
                {
                    return false;
                }
            }
        }

        Address? adr = this.AddressHome;
        Address? otherAdr = other.AddressHome;
        if (adr != null && otherAdr != null && !adr.IsEmpty && !otherAdr.IsEmpty)
        {
            return !adr.CanBeMergedWith(otherAdr);
        }

        string? homePagePersonal = this.WebPagePersonal;
        string? otherHomePagePersonal = other.WebPagePersonal;
        if (!string.IsNullOrWhiteSpace(homePagePersonal) && !string.IsNullOrWhiteSpace(otherHomePagePersonal))
        {
            return !StringComparer.Ordinal.Equals(homePagePersonal, otherHomePagePersonal);
        }

        string? homePageWork = this.WebPageWork;
        string? otherHomePageWork = other.WebPageWork;
        if (!string.IsNullOrWhiteSpace(homePageWork) && !string.IsNullOrWhiteSpace(otherHomePageWork))
        {
            return !StringComparer.Ordinal.Equals(homePageWork, otherHomePageWork);
        }

        return false;
    }

    #endregion


    #region IEquatable

    //Überschreiben von Object.Equals um Vergleich zu ermöglichen
    /// <summary>
    /// Vergleicht die Instanz mit einem <see cref="object"/>, um festzustellen,
    /// ob es sich um ein <see cref="Contact"/>-Objekt handelt, das dieselbe Person oder Organisation repräsentiert.
    /// </summary>
    /// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="obj"/> ein <see cref="Contact"/>-Objekt ist, das dieselbe Person oder Organisation repräsentiert.</returns>
    public override bool Equals(object? obj)
    {
        // If parameter cannot be cast to Contact return false.
        if (obj is not Contact p)
        {
            return false;
        }

        // Referenzgleichheit
        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(p);
    }


    /// <summary>
    /// Vergleicht die Instanz mit einem anderen <see cref="Contact"/>-Objekt, um festzustellen,
    /// ob beide dieselbe Person oder Organisation repräsentieren.
    /// </summary>
    /// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Person oder Organisation repräsentiert.</returns>
    public bool Equals(Contact? other)
    {
        // If parameter is null return false:
        if (other is null)
        {
            return false;
        }

        // Referenzgleichheit
        if (object.ReferenceEquals(this, other))
        {
            return true;
        }

        // Return true if the fields match:
        return CompareBoolean(other);
    }


    /// <summary>
    /// Erzeugt einen Hashcode für das Objekt.
    /// </summary>
    /// <returns>Der Hashcode.</returns>
    public override int GetHashCode()
    {
        const int nullValue = -1;
        return TimeStamp.GetHashCode()
            ^ StringCleaner.PrepareForComparison(DisplayName).GetHashCode()
            ^ HashStringCollection(EmailAddresses)
            ^ (Person?.GetHashCode() ?? nullValue)
            ^ HashPhoneNumbers(PhoneNumbers)
            ^ (Work?.GetHashCode() ?? nullValue)
            ^ HashStringCollection(InstantMessengerHandles);


        static int HashPhoneNumbers(IEnumerable<object?>? coll)
        {
            int collHash = nullValue;

            if (coll is null)
            {
                return collHash;
            }

            foreach (var item in coll)
            {
                collHash ^= item?.GetHashCode() ?? nullValue;
            }
            return collHash;
        }

        static int HashStringCollection(IEnumerable<string?>? coll)
        {
            int collHash = nullValue;

            if (coll is null)
            {
                return collHash;
            }

            foreach (var item in coll)
            {
                collHash ^= StringCleaner.PrepareForComparison(item).GetHashCode();
            }
            return collHash;
        }



    }




    // Überladen von == und !=
    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>Vergleicht <paramref name="contact1"/> und <paramref name="contact2"/>, um festzustellen, ob beide dieselbe Person oder Organisation repräsentieren.</remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> dieselbe Person oder Organisation repräsentieren.</returns>
    public static bool operator ==(Contact? contact1, Contact? contact2)
    {
        // If both are null, or both are same instance, return true.
        if (object.ReferenceEquals(contact1, contact2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (contact1 is null)
        {
            return false; // auf Referenzgleichheit wurde oben geprüft
        }
        else
        {
            return contact2 is not null && contact1.CompareBoolean(contact2);
        }
    }

    /// <summary>
    /// Überladung des != Operators.
    /// </summary>
    /// <remarks>Vergleicht <paramref name="contact1"/> und <paramref name="contact2"/>, um festzustellen, ob beide unterschiedliche Personen oder Organisationen repräsentieren.</remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> verschiedene Personen oder Organisationen repräsentieren.</returns>
    public static bool operator !=(Contact? contact1, Contact? contact2) => !(contact1 == contact2);

    /// <summary>
    /// Vergleicht den Inhalt der Properties von this mit denen eines anderen <see cref="Contact"/>-Objekts darauf,
    /// ob das <see cref="Contact"/>-Objekt dieselbe Person oder Organisation repräsentieren könnte.
    /// </summary>
    /// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn <paramref name="other"/> dieselbe Person oder Organisation repräsentieren könnte, andernfalls <c>false</c>.</returns>
    private bool CompareBoolean(Contact other)
    {
        StringComparer comp = StringComparer.Ordinal;

        return TimeStamp != other.TimeStamp
            && comp.Equals(StringCleaner.PrepareForComparison(DisplayName), StringCleaner.PrepareForComparison(other.DisplayName))
            && CompareStringCollections(EmailAddresses, other.EmailAddresses)
            && Person == other.Person
            && ComparePhoneNumbers(PhoneNumbers, other.PhoneNumbers)
            && Work == other.Work
            && CompareStringCollections(InstantMessengerHandles, other.InstantMessengerHandles)
            && AddressHome == other.AddressHome
            && comp.Equals(StringCleaner.PrepareForComparison(WebPagePersonal), StringCleaner.PrepareForComparison(other.WebPagePersonal))
            && comp.Equals(StringCleaner.PrepareForComparison(Comment), StringCleaner.PrepareForComparison(other.Comment))
            && comp.Equals(StringCleaner.PrepareForComparison(WebPageWork), StringCleaner.PrepareForComparison(other.WebPageWork));


        ////////////////////////////////////////////////////////////////////////////////////////////

        static bool CompareStringCollections(IEnumerable<string?>? coll1, IEnumerable<string?>? coll2)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if(coll1 is null)
            {
                return !coll2!.Any();
            }

            if (coll2 is null)
            {
                return coll1!.Any();
            }

            return coll1.Select(x => StringCleaner.PrepareForComparison(x))
                        .SequenceEqual(coll2.Select(x => StringCleaner.PrepareForComparison(x)));
        }

        static bool ComparePhoneNumbers(IEnumerable<PhoneNumber?>? coll1, IEnumerable<PhoneNumber?>? coll2)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if (coll1 is null)
            {
                return !coll2!.Any();
            }

            if (coll2 is null)
            {
                return coll1!.Any();
            }

            return coll1.SequenceEqual(coll2);
        }

        if (this.IsEmpty)
        {
            if (other.IsEmpty)
            {
                return true;
            }
        }
        else if (other.IsEmpty)
        {
            return false;
        }

        Person? person = this.Person;
        Person? otherPerson = other.Person;
        if (person != null && otherPerson != null && !person.IsEmpty && !otherPerson.IsEmpty)
        {
            return person == otherPerson;
        }

        Work? work = this.Work;
        Work? otherWork = other.Work;
        if (work != null && otherWork != null && !work.IsEmpty && !otherWork.IsEmpty)
        {
            return work == otherWork;
        }

        IEnumerable<string?>? emails = this.EmailAddresses;
        IEnumerable<string?>? otherEmails = other.EmailAddresses;
        if (emails != null && otherEmails != null)
        {
            var emailsArr = emails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var otherEmailsArr = otherEmails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (emailsArr.Length != 0 && otherEmailsArr.Length != 0)
            {
                if (emailsArr.Intersect(otherEmailsArr, StringComparer.Ordinal).Any())
                {
                    return true;
                }
            }
        }


        IEnumerable<string?>? ims = this.InstantMessengerHandles;
        IEnumerable<string?>? otherIMs = other.InstantMessengerHandles;
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

        string? displayName = this.DisplayName;
        string? otherDisplayName = other.DisplayName;
        if (!string.IsNullOrWhiteSpace(this.DisplayName) && !string.IsNullOrWhiteSpace(otherDisplayName))
        {
            return StringComparer.CurrentCultureIgnoreCase.Equals(displayName, otherDisplayName);
        }


        IEnumerable<PhoneNumber?>? phones = this.PhoneNumbers;
        IEnumerable<PhoneNumber?>? otherPhones = other.PhoneNumbers;
        if (phones != null && otherPhones != null)
        {
            PhoneNumber[] phonesArr = phones.Where(x => x != null && !x.IsEmpty).ToArray()!;
            PhoneNumber[] otherPhonesArr = otherPhones.Where(x => x != null && !x.IsEmpty).ToArray()!;

            if (phonesArr.Length != 0 && otherPhonesArr.Length != 0)
            {
                if (phonesArr.Intersect(otherPhonesArr).Any())
                {
                    return true;
                }
            }
        }

        Address? adr = this.AddressHome;
        Address? otherAdr = other.AddressHome;
        if (adr != null && otherAdr != null && !adr.IsEmpty && !otherAdr.IsEmpty)
        {
            return adr.Equals(otherAdr);
        }

        string? homePagePersonal = this.WebPagePersonal;
        string? otherHomePagePersonal = other.WebPagePersonal;
        if (!string.IsNullOrWhiteSpace(homePagePersonal) && !string.IsNullOrWhiteSpace(otherHomePagePersonal))
        {
            return StringComparer.Ordinal.Equals(homePagePersonal, otherHomePagePersonal);
        }

        string? homePageWork = this.WebPageWork;
        string? otherHomePageWork = other.WebPageWork;
        if (!string.IsNullOrWhiteSpace(homePageWork) && !string.IsNullOrWhiteSpace(otherHomePageWork))
        {
            return StringComparer.Ordinal.Equals(homePageWork, otherHomePageWork);
        }

        return true;
    }

    #endregion


    #region ICleanable


    /// <summary>
    /// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält. Vor dem Abfragen der Eigenschaft sollte <see cref="Clean"/>
    /// aufgerufen werden.
    /// </summary>
    public bool IsEmpty => _propDic.Count == 0;

    /// <summary>
    /// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    /// und leere Unterobjekte auf <c>null</c>.
    /// </summary>
    public void Clean()
    {
        KeyValuePair<Prop, object>[]? props = _propDic.ToArray();

        for (int i = 0; i < props.Length; i++)
        {
            KeyValuePair<Prop, object> kvp = props[i];

            switch (kvp.Value)
            {
                case IEnumerable<string?> strings:
                    {
                        string[] arr = strings.Select(x => StringCleaner.CleanDataEntry(x)).Where(x => x != null).Distinct().ToArray()!;

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
                        List<PhoneNumber> numbers = phoneNumbers.Where(x => x != null && !x.IsEmpty).ToList()!;
                        numbers.ForEach(x => x.Clean());

                        IGrouping<PhoneNumber, PhoneNumber>[] groups = numbers.GroupBy(x => x, PhoneNumberComparer.Instance).ToArray();

                        numbers.Clear();

                        foreach (IGrouping<PhoneNumber, PhoneNumber> group in groups)
                        {
                            PhoneNumber number = group.Key;
                            numbers.Add(number);

                            foreach (PhoneNumber telNumber in group)
                            {
                                number.Merge(telNumber);
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

#if !NET40 && !NETSTANDARD2_0 && !NET461
        _propDic.TrimExcess();
#endif

    }



    #endregion


    #region ICloneable

    /// <summary>
    /// Erstellt eine tiefe Kopie des Objekts.
    /// </summary>
    /// <returns>Eine tiefe Kopie des Objekts.</returns>
    public object Clone() => new Contact(this);

    #endregion
}
