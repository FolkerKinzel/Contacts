using FolkerKinzel.Contacts.Intls;

namespace FolkerKinzel.Contacts;

public sealed partial class Contact : Mergeable<Contact>, ICleanable, IEquatable<Contact>, ICloneable
{
    #region Mergeable<T>, ICleanable

    /// <inheritdoc/>
    protected override bool DescribesForeignIdentity(Contact other)
    {
        StringComparer comp = StringComparer.Ordinal;

        if(UrlCollectionHasEvidence(EmailAddresses, other.EmailAddresses, comp, out bool isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (UrlCollectionHasEvidence(InstantMessengerHandles, other.InstantMessengerHandles, comp, out isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (HasEvidence(Person, other.Person, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (HasEvidence(Work, other.Work, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (DisplayNameHasEvidence(DisplayName, other.DisplayName, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if(PhoneNumberCollectionHasEvidence(PhoneNumbers, other.PhoneNumbers, out isDifferentIdentity) && !isDifferentIdentity)
        {
            return false;
        }

        if (UrlHasEvidence(WebPagePersonal, other.WebPagePersonal, comp, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        if (HasEvidence(AddressHome, other.AddressHome, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }


        if (UrlHasEvidence(WebPageWork, other.WebPageWork, comp, out isDifferentIdentity))
        {
            return isDifferentIdentity;
        }

        return false;

        /////////////////////////////////////

        static bool UrlCollectionHasEvidence(IEnumerable<string?>? urls1, IEnumerable<string?>? urls2, StringComparer comp, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (urls1 == null || urls2 == null)
            {
                return false;
            }

            var arr1 = urls1.Where(x => !Strip.IsEmpty(x)).ToArray();
            var arr2 = urls2.Where(x => !Strip.IsEmpty(x)).ToArray();

            if (arr1.Length != 0 && arr2.Length != 0)
            {
                isDifferentIdentity = !arr1.Intersect(arr2, comp).Any();
                return true;
            }

            return false;
        }

        static bool PhoneNumberCollectionHasEvidence(IEnumerable<PhoneNumber?>? urls1, IEnumerable<PhoneNumber?>? urls2, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (urls1 == null || urls2 == null)
            {
                return false;
            }

            PhoneNumber[] arr1 = urls1.Where(x => x != null && !x.IsEmpty).ToArray()!;
            PhoneNumber[] arr2 = urls2.Where(x => x != null && !x.IsEmpty).ToArray()!;

            if (arr1.Length != 0 && arr2.Length != 0)
            {
                isDifferentIdentity = !arr1.Intersect(arr2, PhoneNumberComparer.Instance).Any();
                return true;
            }

            return false;
        }


        static bool HasEvidence<T>(T? p1, T? p2, out bool isDifferentIdentity) where T : Mergeable<T>
        {
            isDifferentIdentity = true;

            if (p1 is null || p2 is null || p1.IsEmpty || p2.IsEmpty)
            {
                return false;
            }

            isDifferentIdentity = !p1.IsMergeableWith(p2);
            return true;
        }


        static bool DisplayNameHasEvidence(string? dp1, string? dp2, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (string.IsNullOrWhiteSpace(dp1) || string.IsNullOrWhiteSpace(dp2))
            {
                return false;
            }

            isDifferentIdentity = !Strip.StartEqual(dp1, dp2, true);
            return true;
        }

        static bool UrlHasEvidence(string? dp1, string? dp2, StringComparer comp, out bool isDifferentIdentity)
        {
            isDifferentIdentity = true;

            if (string.IsNullOrWhiteSpace(dp1) || string.IsNullOrWhiteSpace(dp2))
            {
                return false;
            }

            isDifferentIdentity = !comp.Equals(dp1, dp2);
            return true;

        }

    }

    /// <inheritdoc/>
    protected override void CompleteDataWith(Contact source)
    {
        Person? person = Person;
        Person? sourcePerson = source.Person;

        if (Person.AreMergeable(person, sourcePerson))
        {
            Person = person?.Merge(sourcePerson) ?? sourcePerson;
        }

        Work? work = Work;
        Work? sourceWork = source.Work;

        if (Work.AreMergeable(work, sourceWork))
        {
            Work = work?.Merge(sourceWork) ?? sourceWork;
        }

        Address? adr = AddressHome;
        Address? sourceAdr = source.AddressHome;

        if (Address.AreMergeable(adr, sourceAdr))
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
            ^ HashMergeable(Person)
            ^ HashPhoneNumbers(PhoneNumbers)
            ^ HashMergeable(Work)
            ^ HashStringCollection(InstantMessengerHandles)
            ^ StringCleaner.PrepareForComparison(WebPagePersonal).GetHashCode()
            ^ HashMergeable(AddressHome)
            ^ StringCleaner.PrepareForComparison(WebPageWork).GetHashCode();


        static int HashMergeable<T>(T? mergeable) where T : Mergeable<T>
            => mergeable is null || mergeable.IsEmpty ? nullValue : mergeable.GetHashCode();


        static int HashPhoneNumbers(IEnumerable<PhoneNumber?>? coll)
        {
            int collHash = nullValue;

            if (coll is null)
            {
                return collHash;
            }

            foreach (PhoneNumber? item in coll)
            {
                if(item is not null && !item.IsEmpty)
                {
                    collHash ^= item.GetHashCode();
                }
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

            foreach (string? item in coll)
            {
                if(!string.IsNullOrWhiteSpace(item))
                {
                    collHash ^= item.GetHashCode();
                }
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

        return TimeStamp == other.TimeStamp
            && comp.Equals(StringCleaner.PrepareForComparison(DisplayName), StringCleaner.PrepareForComparison(other.DisplayName))
            && EqualsStringCollections(EmailAddresses, other.EmailAddresses, comp)
            && EqualsMergeables(Person, other.Person)
            && EqualsPhoneNumbers(PhoneNumbers, other.PhoneNumbers)
            && EqualsMergeables(Work, other.Work)
            && EqualsStringCollections(InstantMessengerHandles, other.InstantMessengerHandles, comp)
            && EqualsMergeables(AddressHome, other.AddressHome)
            && comp.Equals(StringCleaner.PrepareForComparison(WebPagePersonal), StringCleaner.PrepareForComparison(other.WebPagePersonal))
            && comp.Equals(StringCleaner.PrepareForComparison(Comment), StringCleaner.PrepareForComparison(other.Comment))
            && comp.Equals(StringCleaner.PrepareForComparison(WebPageWork), StringCleaner.PrepareForComparison(other.WebPageWork));


        ////////////////////////////////////////////////////////////////////////////////////////////
        
        static bool EqualsMergeables<T>(T? x, T? y) where T : Mergeable<T>
        {
            if (x is null || y is null || x.IsEmpty || y.IsEmpty)
            {
                return (x is null || x.IsEmpty) && (y is null || y.IsEmpty);
            }

            return x.IsMergeableWith(y);
        }

        static bool EqualsStringCollections(IEnumerable<string?>? coll1, IEnumerable<string?>? coll2, StringComparer comp)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if (coll1 is null)
            {
                return !coll2!.Any(x => !string.IsNullOrWhiteSpace(x));
            }

            if (coll2 is null)
            {
                return !coll1!.Any(x => !string.IsNullOrWhiteSpace(x));
            }

            return coll1.Select(x => StringCleaner.PrepareForComparison(x))
                        .SequenceEqual(coll2.Select(x => StringCleaner.PrepareForComparison(x)), comp);
        }

        static bool EqualsPhoneNumbers(IEnumerable<PhoneNumber?>? coll1, IEnumerable<PhoneNumber?>? coll2)
        {
            if (ReferenceEquals(coll1, coll2))
            {
                return true;
            }

            if (coll1 is null)
            {
                return !coll2!.Any(x => x is not null && !x.IsEmpty);
            }

            if (coll2 is null)
            {
                return !coll1!.Any(x => x is not null && !x.IsEmpty);
            }

            return coll1.Select(x => x is null || x.IsEmpty ? null : x).SequenceEqual(coll2.Select(x => x is null || x.IsEmpty ? null : x));
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
