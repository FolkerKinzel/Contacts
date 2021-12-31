using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : Mergeable<Contact>, ICleanable, IEquatable<Contact>, ICloneable
{
    #region Mergeable<T>, ICleanable

    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Contact other)
    {
        IEnumerable<string?>? emails = this.EmailAddresses;
        IEnumerable<string?>? otherEmails = other.EmailAddresses;
        if (emails != null && otherEmails != null)
        {
            var emailsArr = emails.Where(x => !Strip.IsEmpty(x)).ToArray();
            var otherEmailsArr = otherEmails.Where(x => !Strip.IsEmpty(x)).ToArray();

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

        if (!Person.CanBeMerged(Person, other.Person))
        {
            return true;
        }

        if (!Work.CanBeMerged(Work, other.Work))
        {
            return true;
        }

        if (Strip.StartEqual(DisplayName, other.DisplayName, true))
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
            return !adr.CanBeMerged(otherAdr);
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

    /// <inheritdoc/>
    protected override void CompleteDataWith(Contact source)
    {
        Person? person = Person;
        Person? sourcePerson = source.Person;

        if (Person.CanBeMerged(person, sourcePerson))
        {
            Person = person?.Merge(sourcePerson) ?? sourcePerson;
        }

        Work? work = Work;
        Work? sourceWork = source.Work;

        if (Work.CanBeMerged(work, sourceWork))
        {
            Work = work?.Merge(sourceWork) ?? sourceWork;
        }

        Address? adr = AddressHome;
        Address? sourceAdr = source.AddressHome;

        if (Address.CanBeMerged(adr, sourceAdr))
        {
            AddressHome = adr?.Merge(sourceAdr) ?? sourceAdr;
        }

        if (Strip.IsEmpty(DisplayName))
        {
            DisplayName = source.DisplayName;
        }

        if (Strip.IsEmpty(WebPagePersonal))
        {
            WebPagePersonal = source.WebPagePersonal;
        }

        if (Strip.IsEmpty(WebPageWork))
        {
            WebPageWork = source.WebPageWork;
        }

        string? comment = Comment;

        if (string.IsNullOrWhiteSpace(comment))
        {
            Comment = source.Comment;
        }
        else
        {
            string? sourceComment = source.Comment;

            if (!string.IsNullOrWhiteSpace(source.Comment))
            {
                Comment = string.Concat(comment, Environment.NewLine, Environment.NewLine, sourceComment);
            }
        }

        MergeEmailAddresses(source);

        MergeInstantMessengerHandles(source);

        MergePhoneNumbers(source);

        /////////////////////////////////////////////////

        void MergeEmailAddresses(Contact source)
        {
            IEnumerable<string?>? emailAdresses = EmailAddresses;

            if (emailAdresses is null)
            {
                EmailAddresses = source.EmailAddresses;
            }
            else
            {
                IEnumerable<string?>? sourceEmailAdresses = source.EmailAddresses;

                if (sourceEmailAdresses is not null)
                {
                    var list = emailAdresses.ToList();
                    list.AddRange(sourceEmailAdresses);
                    EmailAddresses = list.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                }
            }
        }

        void MergeInstantMessengerHandles(Contact source)
        {
            IEnumerable<string?>? imAddresses = InstantMessengerHandles;

            if (imAddresses is null)
            {
                InstantMessengerHandles = source.InstantMessengerHandles;
            }
            else
            {
                IEnumerable<string?>? sourceIMAddresses = source.InstantMessengerHandles;

                if (sourceIMAddresses is not null)
                {
                    var list = imAddresses.ToList();
                    list.AddRange(sourceIMAddresses);
                    InstantMessengerHandles = list.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
                }
            }
        }

        void MergePhoneNumbers(Contact source)
        {
            IEnumerable<PhoneNumber?>? phoneNumbers = PhoneNumbers;

            if (phoneNumbers is null)
            {
                PhoneNumbers = source.PhoneNumbers;
            }
            else
            {
                IEnumerable<PhoneNumber?>? sourcePhoneNumbers = source.PhoneNumbers;

                if (sourcePhoneNumbers is not null)
                {
                    var list = phoneNumbers.ToList();
                    list.AddRange(sourcePhoneNumbers);

                    IGrouping<PhoneNumber, PhoneNumber>[] groups = list.Where(x => x != null && !x.IsEmpty).GroupBy(x => x, PhoneNumberComparer.Instance).ToArray()!;

                    list.Clear();

                    foreach (IGrouping<PhoneNumber, PhoneNumber> group in groups)
                    {
                        PhoneNumber number = group.Key;
                        list.Add(number);

                        foreach (PhoneNumber telNumber in group)
                        {
                            _ = number.Merge(telNumber);
                        }
                    }

                    PhoneNumbers = list;
                }
            }
        }
    }

    #region ICleanable


    ///// <summary>
    ///// <c>true</c> gibt an, dass das Objekt keine verwertbaren Daten enthält.
    ///// </summary>
    /// <inheritdoc/>
    public override bool IsEmpty
    {
        get
        {
            foreach (KeyValuePair<Prop, object> kvp in _propDic)
            {
                switch (kvp.Value)
                {
                    case ICleanable cleanable when !cleanable.IsEmpty:
                    case string s when !string.IsNullOrWhiteSpace(s):
                    case DateTime dt when dt != default:
                    case IEnumerable<string?> strColl when strColl.Any(x => !string.IsNullOrWhiteSpace(x)):
                    case IEnumerable<ICleanable?> cleanableColl when cleanableColl.Any(x => !(x?.IsEmpty ?? true)):
                        return false;
                }
            }

            return true;
        }
    }

    ///// <summary>
    ///// Reinigt alle Strings in allen Feldern des Objekts von ungültigen Zeichen und setzt leere Strings
    ///// und leere Unterobjekte auf <c>null</c>.
    ///// </summary>
    /// <inheritdoc/>
    public override void Clean()
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

    #endregion


    #region IEquatable

    ///// <summary>
    ///// Vergleicht die Instanz mit einem anderen <see cref="object"/>, um festzustellen,
    ///// ob es sich bei <paramref name="obj"/> um ein <see cref="Contact"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat. 
    ///// </summary>
    ///// <param name="obj">Das <see cref="object"/>, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn es sich bei <paramref name="obj"/> um ein <see cref="Contact"/>-Objekt handelt, das
    ///// gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
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


    ///// <summary>
    ///// Vergleicht die Instanz mit einem anderen 
    ///// <see cref="Contact"/>-Objekt,
    ///// um festzustellen, ob beide gleich sind.
    ///// </summary>
    ///// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
    ///// <returns><c>true</c>, wenn <paramref name="other"/> gleiche Eigenschaften hat.</returns>
    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Contact? other)
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


    ///// <summary>
    ///// Erzeugt einen Hashcode für das Objekt.
    ///// </summary>
    ///// <returns>Der Hashcode.</returns>
    /// <inheritdoc/>
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


    /// <summary>
    /// Überladung des == Operators.
    /// </summary>
    /// <remarks>
    /// Vergleicht zwei <see cref="Contact"/>-Objekte, um zu überprüfen, ob sie gleich sind.
    /// </remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> gleich sind.</returns>
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
    /// <remarks>
    /// Vergleicht zwei <see cref="Contact"/>-Objekte, um zu überprüfen, ob sie ungleich sind.
    /// </remarks>
    /// <param name="contact1">Linker Operand.</param>
    /// <param name="contact2">Rechter Operand.</param>
    /// <returns><c>true</c>, wenn <paramref name="contact1"/> und <paramref name="contact2"/> ungleich sind.</returns>
    public static bool operator !=(Contact? contact1, Contact? contact2) => !(contact1 == contact2);


    /// <summary>
    /// Vergleicht die Eigenschaften mit denen eines anderen <see cref="Contact"/>-Objekts.
    /// </summary>
    /// <param name="other">Das <see cref="Contact"/>-Objekt, mit dem verglichen wird.</param>
    /// <returns><c>true</c>, wenn alle Eigenschaften übereinstimmen.</returns>
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

            if (coll1 is null)
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
